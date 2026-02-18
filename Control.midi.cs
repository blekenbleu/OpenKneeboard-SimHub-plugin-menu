
using NAudio.Midi;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	public class MidiDev
	{
		internal string deviceName, butName;
		internal uint devMessage;	// lMidiIn index | data2 | data 1 | status
	}

	/// <summary>
	/// MIDI interaction code for Control.xaml
	/// </summary>
	public partial class Control
	{
		static string sb4;			// prompt string when !learn
		// https://github.com/blekenbleu/OpenKneeboard-SimHub-plugin-menu/blob/MIDI/Channel.md#midi-device-name-handling
		void Learn(string bName)	// manage MIDI messages
		{
            // search for butName in Settings.midiDevs
			int dev = (int)latest >> 24;
			// search for latest & 0x0F00FFFF in Settings.midiDevs
			OKSHmenu.Settings.midiDevs.Add(new MidiDev()
			{
				deviceName = MidiIn.DeviceInfo(dev).ProductName,
				butName = bName,
				devMessage = latest & 0x0F00FFFF
			});
			if (!button)
				Model.StatusText = sb4 + "\nMIDI control for slider";
        }

		void Unlearn()				// handle "bm" == butName
		{
			learn = !learn;
			if (learn)
			{
				sb4 = Model.StatusText;
 				if (null == MIDI.Model)
					MIDI.Start(Model);
				Model.StatusText += "\n\twaiting for MIDI input";
			} else if (null != b4) {
				Model.StatusText = sb4;
				sb4 = null;
			}
		}

		static uint recent, latest;
		static bool button;
		internal static void Process(uint RawMessage)
		{
			OKSHmenu.Info($"Process({RawMessage:X8}) to do");
			if (recent != (0x0F00FFFF & RawMessage))
			{
				button = true;
				recent = 0x0F00FFFF & RawMessage;
			}
			byte value = (byte)(RawMessage >> 16);
			if (0 < value && 127 != value)
				button = false;				// Learn() should assign only to slider
			latest = RawMessage;
		}
	}
}
