using System.Collections.Generic;
using System.Windows;
using NAudio.Midi;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	/// <summary>
	/// MIDI interaction code for Control.xaml
	/// </summary>
	public partial class Control
	{
		// index xaml event strings by devMessages
		internal static SortedList<int, string> click = new SortedList<int, string>() {};
		static uint recent, latest; // MidiDev messages;  recent has data2 masked out
		static bool button,         // state variables
					_learn = false;
		static bool learn { get { return _learn; }
							set {
									_learn = value;
									Model.Forget = _learn ? Visibility.Visible : Visibility.Hidden;
							}}

		// https://github.com/blekenbleu/OpenKneeboard-SimHub-plugin-menu/blob/MIDI/Channel.md#midi-device-name-handling
		// https://learn.microsoft.com/en-us/dotnet/api/system.collections.sortedlist?view=netframework-4.8
		// https://www.hobbytronics.co.uk/wp-content/uploads/2023/07/9_MIDI_code.pdf
		void Learn(string bName)	// associate MIDI messages with xaml events
		{
			if ( "bf" == bName)					// Forget button?
			{
				if (0 == click.Count)
					Model.MidiStatus = "\nNo listed clicks to forget";
				else {
					recent = 0xFF0000FF;
					Model.MidiStatus = "\nSelect a click to forget";
				}
			}
			else if (0xFF0000FF == recent) {			// Forget?
				if (!click.ContainsValue(bName))
					Model.MidiStatus = $"\n'{bName}' not in click list";
				else {
					click.RemoveAt(click.IndexOfValue(bName));
					Model.MidiStatus = $"\n'{bName}' removed";
				}
				recent = 0;
			}
			else if (0 == recent)
				Model.MidiStatus = "\nMIDI input missing";
			else if (bName != "SL" && !button)
				Model.MidiStatus = "\nMIDI control >>only<< for slider;  ignored";
			else if (bName == "SL" && button)
				Model.MidiStatus = "\nMIDI control >>only<< for button; ignored";
			else if (click.ContainsValue(bName))
				Model.MidiStatus = $"\n'{bName}' already in click list;  first Forget it";
			else if (click.ContainsKey((int)recent))
				Model.MidiStatus = $"\n{recent:X8} already in click list;  first Forget it";
			else {
				click.Add((int)latest, bName);
				Model.MidiStatus = $"\n'{bName}' {recent:X8} added to click list";
			}
		}

		void Unlearn()				// handle "bm" == butName
		{
			learn = !learn;
			if (learn)
			{
 				if (null == MIDI.Model)
					MIDI.Start(Model);
				Model.MidiStatus = "\n\twaiting for MIDI input";
			} else Model.MidiStatus = "";
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
					Model.MidiStatus = $"\nProcess.({MidiMessage:X8}) ignored";
					break;
				default:
					byte value = (byte)(MidiMessage >> 16);

					if (learn && 0xB0 != (0xFF00F0 & MidiMessage))	// ignore CC button releases
						Model.MidiStatus = $"\nclick to learn for ({MidiMessage:X8})";
					if (recent != (0x0F00FFFF & MidiMessage))
					{
						button = 0 == value % 127;
						recent = 0x0F00FFFF & MidiMessage;
					}
					else if (0 != value % 127)
						button = false;										// Learn():  assign only to slider
					latest = MidiMessage;									// Learn():  wait for xaml event
					if (!learn && (!button || 0xB0 != (0xFF00F0 & latest)))	// ignore CC button release
					{
						if (click.ContainsKey((int)recent))
						{
							Model.MidiStatus = "";
							ButHandle(click[(int)recent]);
						}
						else Model.MidiStatus = $"\nProcess.({MidiMessage:X8}) not learned";
					}
					break;
			}
		}
	}
}
