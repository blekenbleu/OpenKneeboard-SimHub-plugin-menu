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
		internal static SortedList<uint, string> click = new SortedList<uint, string>() {};
		static uint recent, mVal;	// MidiDev messages;  recent has (mVal = data2) masked out
		static bool button,	_learn = false;		// state variables
		static bool learn
		{
			get { return _learn; }
			set {
					_learn = value;
					Model.Forget = _learn ? Visibility.Visible : Visibility.Hidden;
				}
		}

		void Ok(string bName)
		{
			if (0xFF0000FF == recent) {			// Forget?
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
			else if (click.ContainsValue(bName))
				Model.MidiStatus = $"\n'{bName}' already in click list;  first Forget it";
			else if (click.ContainsKey(recent))
				Model.MidiStatus = $"\n{recent:X8} already in click list;  first Forget it";
			else {
				click.Add(recent, bName);
				Model.MidiStatus = $"\n'{bName}' {recent:X8} added to click list";
			}
		}

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
			else if (!button)
				Model.MidiStatus = "\nMIDI control >>only<< for slider;  ignored";
			else Ok(bName);
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
 ;			var channel = 0x0F & MidiMessage;		// most likely always 0 for real control surfaces
 ;			var d1 = (MidiMessage >> 8) & 0xff;		// e.g. CC number
 ;			var d2 = (MidiMessage >> 16) & 0xff;		// data value
 ;			var dev = (MidiMessage >> 24) & 0x0f;	// NAudio-detected MIDI device list index
 */
			if (0xB0 == (0xFF00F0 & MidiMessage))	// ignore CC button releases
				return;

			uint channel_type = 0xF0 & MidiMessage;
			if (0xB0 > channel_type || 0xD0 < channel_type)
			{
				Model.MidiStatus = $"\nProcess({MidiMessage:X8}) ignored";
				return;
			}

			mVal = 127 & (MidiMessage >> 16);
			if (learn) {
				uint latest = 0x0F00FFFF & MidiMessage;
				if (recent != latest)
				{
					button = 0 == mVal % 127;
					recent = latest;
				}
				else if (0 != mVal % 127)
					button = false;
				if (click.ContainsKey(recent))
					Model.MidiStatus = $"\nProcess.({MidiMessage:X8}) already in click list;  Forget?";
				else Model.MidiStatus = $"\nclick to learn for ({MidiMessage:X8})";
			} else if (click.ContainsKey(recent)) {
				Model.MidiStatus = "";
				ClickHandle(click[recent]);
			} else Model.MidiStatus = $"\nProcess.({MidiMessage:X8}) not learned";
		}
	}
}
