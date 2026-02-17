using System;
using System.Collections.Generic;
using NAudio.Midi;


namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	internal class Device
	{
		internal string id;
		internal MidiIn m;
	}

	// https://github.com/naudio/NAudio/blob/master/Docs/MidiInAndOut.md
	// https://deepwiki.com/naudio/NAudio/7-midi-support#midi-io-operations
	partial class MIDI
	{
		// https://truelogic.org/wordpress/2021/01/28/using-midi-with-naudio/
        static List<Device> lMidiIn = new List<Device> { };

		// an array of MidiIn message event handlers
		// to distinguish which device sourced each message
		// https://github.com/naudio/NAudio/NAudio.Midi/Midi/MidiInMessageEventArgs.cs
		static readonly System.EventHandler<NAudio.Midi.MidiInMessageEventArgs>[] RcvArray
			= new System.EventHandler<NAudio.Midi.MidiInMessageEventArgs>[3] { MidiIn0, MidiIn1, MidiIn2 };
		internal static ViewModel Model;


		internal static void Start(ViewModel m)
		{
			Model = m;
			InputMidiDevices();
			ReadMidi();
		}

// shutting down and restarting between games
// check out https://github.com/naudio/NAudio/blob/master/NAudioDemo/MidiInDemo/MidiInPanel.cs#L67
		internal static void Stop(int i)
		{
			lMidiIn[i].m.Stop();
			lMidiIn[i].m.Dispose();
			lMidiIn[i].m.MessageReceived -= RcvArray[i];
			lMidiIn[i].m.ErrorReceived -= MidiIn_ErrorReceived;
			lMidiIn.RemoveAt(i);
		}

		internal static void Stop()
		{
			for (int j = lMidiIn.Count -1 ; j >= 0; j--)
				Stop(j);
		}

		static void HandleMidiMessages(int deviceNumber, string ProductName)
		{
			int j = lMidiIn.Count;

			if (j > RcvArray.Length)
				return;
			OKSHmenu.Info("HandleMidiMessages(): ");
			MidiIn mMidiIn = new MidiIn(deviceNumber);
			mMidiIn.MessageReceived += RcvArray[j];
			mMidiIn.ErrorReceived += MidiIn_ErrorReceived;
			mMidiIn.Start();
			lMidiIn.Add(new Device { id = ProductName, m = mMidiIn });
		}

		static void InputMidiDevices()
		{
			string s = $"InputMidiDevices():  NAudio MIDI In device count {MidiIn.NumberOfDevices}";
/*
			if (0 < lMidiIn.Count)
				for (int i = 0; i < MidiIn.NumberOfDevices; i++)
				{
					string input = MidiIn.DeviceInfo(i).ProductName;
					s += ("\n\t" + input);
					if (lMidiIn.Exists(x => x.id == input))
						s += "\thandled";
				}
			else
 */
			for (int i = 0; i < MidiIn.NumberOfDevices; i++)
			{
				string input = MidiIn.DeviceInfo(i).ProductName;
				s += ("\n\t" + input);
				if (lMidiIn.Count < RcvArray.Length
				 && ("nanoKONTROL2" == input
				 || input.StartsWith("USB-Midi")))
				{
					s += "\thandled";
					HandleMidiMessages(i, input);
				}
			}
			OKSHmenu.Info(s);
			Model.StatusText = s;
		}

        // e.MidiEvent = FromRawMessage(e.RawMessage);
        static void MidiIn0(object sender, MidiInMessageEventArgs e)
		{
			Enque(0, (uint)e.RawMessage);
//			OKSHmenu.Info(lMidiIn[0].id + String.Format(" Msg 0x{0:X8} Event {1}", e.RawMessage, e.MidiEvent));
		}

		static void MidiIn1(object sender, MidiInMessageEventArgs e)
		{
			Enque(1, (uint)e.RawMessage);
//			OKSHmenu.Info(lMidiIn[1].id + String.Format(" Msg 0x{0:X8} Event {1}", e.RawMessage, e.MidiEvent));
		}

		static void MidiIn2(object sender, MidiInMessageEventArgs e)
		{
			Enque(2, (uint)e.RawMessage);
//			OKSHmenu.Info(lMidiIn[2].id + String.Format(" Msg 0x{0:X8} Event {1}", e.RawMessage, e.MidiEvent));
		}

		static void MidiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
		{
			OKSHmenu.Info(String.Format("MidiIn_ErrorReceived():  Message 0x{0:X8} Event {1}",
				e.RawMessage, e.MidiEvent));
		}

		// Handle Control Change (0xB0), Patch Change (0xC0) and Bank Select (0xB0) channel messages
		// https://github.com/naudio/NAudio/blob/master/NAudio.Midi/Midi/MidiEvent.cs#L24
		// https://www.hobbytronics.co.uk/wp-content/uploads/2023/07/9_MIDI_code.pdf
		internal static void Sort(uint RawMessage)
		{
			// NAudio bytes are reversed from e.g. MidiView and WetDry:  Status byte is least significant..
			var c = 0x0F & RawMessage;			// 0x0F & e.RawMessage
			var d1 = (RawMessage >> 8) & 0xff;
			var d2 = (RawMessage >> 16) & 0xff;
			var dev = (RawMessage >> 24) & 0x0f;
			switch (0xF0 & RawMessage)  // 0x80 <= (0xF0 & e.RawMessage) < 0xF0
			{
				case 0x80:
				case 0x90:
				case 0xA0:
					OKSHmenu.Info($"Sort.({RawMessage:X8}) ignored");
					break;
				case 0xF0:
					OKSHmenu.Info($"Sort.({RawMessage:X8}) ignored");
					break;
				default:
					OKSHmenu.Info($"Process({RawMessage:X8}) to do");
					break;
			}
		}
	}
}
