// https://medium.com/@abhirajgawai/c-channels-explained-from-producer-consumer-basics-to-high-performance-net-systems-f8ab610c0639

using System.Threading.Channels

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	// https://www.csharptutorial.net/csharp-tutorial/csharp-record/
	public record MidiChannelEvent( uint32 Payload );	// pass from Naudio MIDI device input events

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
		    		SingleWriter = false  // Multiple producers
			    	FullMode = BoundedChannelFullMode.DropOldest
				},
				static void (MidiChannelEvent dropped) +>
   				{
					OKSHmenu.Info($"MidiChannelEvent Dropped: {dropped}");
				}
			}
		);

		public ChannelReader<MidiChannelEvent> Reader => _channel.Reader;

		while (await Reader.WaitToReadAsync(token))
		{
    		while (reader.TryRead(out var item))
    		{
        		Process(item);
    		}
		}
	}			class ChannelEventBus

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
		void Enqueue (uint32 inDevice, uint32 payload)
		// https://learn.microsoft.com/en-us/dotnet/core/extensions/channels#producer-patterns
		{
			uint32 payload |= (inDevice << 24);
			// Fire-and-forget
			if (!ChannelWriter<MidiChannelEvent>.TryWrite(new MidiChannelEvent(payload)))
				OKSHmenu.Info(lMidiIn[3].id + ".Enqueue(" + e.MidiEvent.ToString() + ") failed");
		}
	}
}
