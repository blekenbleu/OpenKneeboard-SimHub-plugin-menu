// https://medium.com/@abhirajgawai/c-channels-explained-from-producer-consumer-basics-to-high-performance-net-systems-f8ab610c0639

using System;
using System.Threading.Channels;
using System.Threading.Tasks;

// https://www.csharptutorial.net/csharp-tutorial/csharp-record/


namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	internal class MidiChannelEvent
	{ internal UInt32 Payload; } // pass from Naudio MIDI device input events

	public sealed class ChannelEventBus : IDisposable
	{
		private readonly Channel<MidiChannelEvent> _channel;

		public ChannelEventBus()
		{
			// Multiple producers, single consumer 
			_channel = Channel.CreateBounded<MidiChannelEvent>(
				// Bounded to 100. If full, drop oldest.
				new BoundedChannelOptions(100)
				{
					SingleReader = true,  // Optimization hint
					SingleWriter = false,  // Multiple producers

					FullMode = BoundedChannelFullMode.DropOldest
				},
				(MidiChannelEvent dropped) =>
					   OKSHmenu.Info($"MidiChannelEvent Dropped: {dropped}")
			);
		}

		internal ChannelReader<MidiChannelEvent> Reader => _channel.Reader;
		internal ChannelWriter<MidiChannelEvent> Writer => _channel.Writer;

		internal async Task ReadAsync()
		{
			while (await Reader.WaitToReadAsync())
    			while (Reader.TryRead(out MidiChannelEvent item))
					MIDI.Process(item);
		}

        public void Dispose()
		{
			throw new NotImplementedException();
		}
	}   //		class ChannelEventBus

	/*
		public class WebhookProcessorService : BackgroundService
		{
			private readonly IEventBus _eventBus;

			public WebhookProcessorService(IEventBus eventBus)
			{
				_eventBus = eventBus;
			}
		}
	 */
	internal partial class MIDI
	{
		static readonly ChannelEventBus CEB = new();
		
        internal void Enqueue(UInt32 inDevice, uint payload)
        // https://learn.microsoft.com/en-us/dotnet/core/extensions/channels#producer-patterns
        {
            payload |= (inDevice << 24);
            // Fire-and-forget
            if (CEB.Writer.TryWrite(new MidiChannelEvent() { Payload = payload }))
                return;
            OKSHmenu.Info(lMidiIn[3].id + ".Enqueue(" + payload.ToString() + ") failed");
        }

        // Handle Control Change (0xB0), Patch Change (0xC0) and Bank Select (0xB0) channel messages
        // https://github.com/naudio/NAudio/blob/master/NAudio.Midi/Midi/MidiEvent.cs#L24
        // https://www.hobbytronics.co.uk/wp-content/uploads/2023/07/9_MIDI_code.pdf
        internal static void Process(MidiChannelEvent item)
		{
			// NAudio bytes are reversed from e.g. MidiView and WetDry:  Status byte is least significant..
			UInt32 RawMessage = item.Payload;

			var c = 0x0F & RawMessage;            // 0x0F & e.RawMessage
			var d1 = (RawMessage >> 8) & 0xff;
			var d2 = (RawMessage >> 16) & 0xff;
			var dev = (RawMessage >> 24) & 0x0f;
			switch (0xF0 & RawMessage)  // 0x80 <= (0xF0 & e.RawMessage) < 0xF0
			{
				case 0x80:
				case 0x90:
				case 0xA0:
					OKSHmenu.Info($"Process({RawMessage:X}) ignored");
					break;
				case 0xF0:
					OKSHmenu.Info($"Process({RawMessage:X}) ignored");
					break;
				default:
					OKSHmenu.Info($"Process({RawMessage:X}) to do");
					break;
			}
		}
	}
}
