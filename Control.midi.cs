using System.Collections.Generic;
using System.Windows;
using NAudio.Midi;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	public class MidiDev			// must be public for Settings.cs
	{
		internal string deviceName, butName;
		internal uint devMessage;	// lMidiIn index | data2 | data 1 | status
	}

	/// <summary>
	/// MIDI interaction code for Control.xaml
	/// </summary>
	public partial class Control
	{
		// index xaml event strings by devMessages
		static SortedList<uint, string> click = new SortedList<uint, string>() {};
		static uint recent, latest; // MidiDev messages;  recent has data2 masked out
		static bool button,         // state variables
					_learn = false;
		static bool learn { get { return _learn; }
							set {
									_learn = value;
									Model.Forget = _learn ? Visibility.Visible : Visibility.Hidden;
							}}

		// https://github.com/blekenbleu/OpenKneeboard-SimHub-plugin-menu/blob/MIDI/Channel.md#midi-device-name-handling
		void Learn(string bName)	// associate MIDI messages with xaml events
		{
			// search for butName in Settings.midiDevs
			int dev = (int) recent >> 24;

			// search for recent in Settings.midiDevs
			OKSHmenu.Settings.midiDevs.Add(new MidiDev()
			{
				deviceName = MidiIn.DeviceInfo(dev).ProductName,
				butName = bName,
				devMessage = recent
			});
			if (bName != "SB" && !button)
				Status.Text = Model.StatusText  + "\nMIDI control >>only<< for slider;  ignored";
			else if (bName == "SB" && button)
				Status.Text = Model.StatusText + "\nMIDI control >>only<< for button; ignored";
			// To Do:  check for latest or bName already in click
			else click.Add(latest, bName);
		}

		void Unlearn()				// handle "bm" == butName
		{
			learn = !learn;
			if (learn)
			{
 				if (null == MIDI.Model)
					MIDI.Start(Model);
				Status.Text = Model.StatusText + "\n\twaiting for MIDI input";
			} else
				Status.Text = Model.StatusText;
		}

		// Handle Control Change (0xB0), Patch Change (0xC0) and Bank Select (0xB0) channel messages
		// https://github.com/naudio/NAudio/blob/master/NAudio.Midi/Midi/MidiEvent.cs#L24
		// https://www.hobbytronics.co.uk/wp-content/uploads/2023/07/9_MIDI_code.pdf
		internal static void Process(uint MidiMessage)
		{
/*			NAudio bytes are reversed from e.g. MidiView and WetDry:  Status byte is least significant..
			var channel = 0x0F & MidiMessage;		// most likely always 0 for real control surfaces
			var d1 = (MidiMessage >> 8) & 0xff;		// e.g. CC number
			var d2 = (MidiMessage >> 16) & 0xff;		// data value
			var dev = (MidiMessage >> 24) & 0x0f;	// NAudio-detected MIDI device list index
 */
			switch (0xF0 & MidiMessage)  // channel_type 0x80 <= (0xF0 & e.RawMessage) < 0xF0
			{
				case 0x80:
				case 0x90:
				case 0xA0:
				case 0xF0:
					OKSHmenu.Info($"Process.({MidiMessage:X8}) ignored");
					break;
				default:
					byte value = (byte)(MidiMessage >> 16);

					OKSHmenu.Info($"Process({MidiMessage:X8}) to do");
					if (recent != (0x0F00FFFF & MidiMessage))
					{
						button = 0 == value % 127;
						recent = 0x0F00FFFF & MidiMessage;
					}
					else if (0 != value % 127)
						button = false;				// Learn() should assign only to slider
					latest = MidiMessage;
					if (!learn)						// learn waits for xaml event to Learn()
					{
						if (click.ContainsKey(recent))
							ButHandle(click[recent]);
						else OKSHmenu.Info($"Process.({MidiMessage:X8}) not learned");
					}
					break;
			}
		}
	}
}
