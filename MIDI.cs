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
			OKSHmenu.Info(lMidiIn[0].id + String.Format(" Msg 0x{0:X8} Event {1}", e.RawMessage, e.MidiEvent));
		}

		static void MidiIn1(object sender, MidiInMessageEventArgs e)
		{
			// Enque(1, e.RawMessage);
			OKSHmenu.Info(lMidiIn[1].id + String.Format(" Msg 0x{0:X8} Event {1}", e.RawMessage, e.MidiEvent));
		}

		static void MidiIn2(object sender, MidiInMessageEventArgs e)
		{
			OKSHmenu.Info(lMidiIn[2].id + String.Format(" Msg 0x{0:X8} Event {1}", e.RawMessage, e.MidiEvent));
		}

		static void MidiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
		{
			OKSHmenu.Info(String.Format("MidiIn_ErrorReceived():  Message 0x{0:X8} Event {1}",
				e.RawMessage, e.MidiEvent));
		}
	}
}
