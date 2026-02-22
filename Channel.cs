// https://medium.com/@abhirajgawai/c-channels-explained-from-producer-consumer-basics-to-high-performance-net-systems-f8ab610c0639
using System.Threading.Channels;
using System.Threading.Tasks;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	internal partial class MIDI
	{
		// Multiple producers, single consumer 
		readonly static Channel<uint> _channel = Channel.CreateBounded(
			new BoundedChannelOptions(100)	// Bounded to 100. If full, drop oldest.
			{
				SingleReader = true,	// Optimization hint
				SingleWriter = false,	// Multiple producers

				FullMode = BoundedChannelFullMode.DropOldest
			},
			(uint dropped) =>
					   OKSHmenu.Info($"MIDI.Channel dropped: {dropped:X}")
		);

		internal static ChannelReader<uint> Reader => _channel.Reader;
		internal static ChannelWriter<uint> Writer => _channel.Writer;

		internal static async Task ReadAsync()
		{
			while (await Reader.WaitToReadAsync())
				while (Reader.TryRead(out uint item))
					Control.Process(item);				// Control.midi.cs

			OKSHmenu.Info($"ReadAsync() ended");
		}

		internal static void ReadMidi()
		{ Task.Run(() => ReadAsync()); }
		
		internal static void Enque(uint inDevice, uint payload)
		// https://learn.microsoft.com/en-us/dotnet/core/extensions/channels#producer-patterns
		{
			payload |= (inDevice << 24);
			// Fire-and-forget
			if (Writer.TryWrite(payload))
				return;
			OKSHmenu.Info(lMidiIn[3].id + ".Enqueue(" + payload.ToString() + ") failed");
		}
	}
}
