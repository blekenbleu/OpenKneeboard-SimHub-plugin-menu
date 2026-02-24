using System;
using System.Collections.Generic;
using NAudio.Midi;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	public class MidiDev			// must be public for Settings.cs
	{
		public string devName, butName;
		public uint devMessage;	// {4-bit dev = 1-bit button | 3-bit lMidiIn index} | data2 | data 1 | status
	}

	internal class Device			// NAudio MidiIn lacks device name
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
		static readonly EventHandler<MidiInMessageEventArgs>[] RcvArray
			= new EventHandler<MidiInMessageEventArgs>[3] { MidiIn0, MidiIn1, MidiIn2 };
		internal static ViewModel Model;

		internal static void Start(ViewModel m)
		{
			Model = m;
			InputMidiDevices();
			ReadMidi();
		}

		// populate Control.midi.cs SortedList click from Settings.cs midiDevs
		// Update MidiDev devMessage 3-bit lMidiIn indices to (j)
		// for devName matching MidiIn.DeviceInfo(j).ProductName
		internal static void Resume(ViewModel m)
		{
			Control.click.Clear();
			for (int i = 0; i < OKSHmenu.Settings.midiDevs.Count; i++)
			{
				for (int j = 0; j < MidiIn.NumberOfDevices; j++)
				{
					if (OKSHmenu.Settings.midiDevs[i].devName == MidiIn.DeviceInfo(j).ProductName)
					{
						uint recent = OKSHmenu.Settings.midiDevs[i].devMessage;
						uint mDev = (uint)j << 24;		// updated 3-bit lMidiIn index
						uint dev = recent;

						dev &= 0x07000000;				// breaks if 7 < NumberOfDevices
						recent &= 0xFFFF;
						recent |= mDev;
						Control.click.Add(recent, OKSHmenu.Settings.midiDevs[i].butName);
						for (int k = 1 + i; k < OKSHmenu.Settings.midiDevs.Count; k++)
						{
							if (OKSHmenu.Settings.midiDevs[k].devMessage == OKSHmenu.Settings.midiDevs[i].devMessage)
							{
								i = k;
								continue;	// ignore duplicate midiDevs
							}
							// likely multiple OKSHmenu.Settings.midiDevs per dev
							recent = OKSHmenu.Settings.midiDevs[k].devMessage;
							if (dev == (0x07000000 & recent))
							{
								i = k;
								recent &= 0xFFFF;
								recent |= mDev;
								Control.click.Add(recent, OKSHmenu.Settings.midiDevs[i].butName);
							}
							else break;
						}
						break;
					}
				}
			}
			OKSHmenu.Info($"Resume():  {Control.click.Count} configured clicks");
			Start(m);
		}

// shutting down and restarting between games
// https://github.com/naudio/NAudio/blob/master/NAudioDemo/MidiInDemo/MidiInPanel.cs#L67
		internal static void Stop(int i)
		{
			lMidiIn[i].m.Stop();
			lMidiIn[i].m.Dispose();
			lMidiIn[i].m.MessageReceived -= RcvArray[i];
			lMidiIn[i].m.ErrorReceived -= MidiIn_ErrorReceived;
			lMidiIn.RemoveAt(i);
		}

		internal static bool Stop()			// called by OKSHmenu.cs End()
		{
			OKSHmenu.Settings.midiDevs = new List<MidiDev>() {};
			for (int j = 0; j < Control.click.Count; j++)
			{
				uint key = Control.click.Keys[j];
				int i = (int)(0x07000000 & key);
				i >>= 24;
				OKSHmenu.Settings.midiDevs.Add(new MidiDev()
            	{
					butName = Control.click.Values[j],
					devName = MidiIn.DeviceInfo(i).ProductName,
					devMessage = key
				});
			}

			for (int j = lMidiIn.Count -1 ; j >= 0; j--)
				Stop(j);
			return 0 < OKSHmenu.Settings.midiDevs.Count;
		}

		static void InputMidiSetup(int deviceNumber, string ProductName)
		{
			int j = lMidiIn.Count;

			if (j >= RcvArray.Length)
				return;

			MidiIn mMidiIn = new MidiIn(deviceNumber);
			mMidiIn.MessageReceived += RcvArray[j];
			mMidiIn.ErrorReceived += MidiIn_ErrorReceived;
			mMidiIn.Start();
			lMidiIn.Add(new Device { id = ProductName, m = mMidiIn });
		}

		static void InputMidiDevices()
		{
			string s = $"InputMidiDevices():  NAudio MidiIn device count {MidiIn.NumberOfDevices}";
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
				if (input.StartsWith("loopMIDI"))
				{
					s += "\tignored";
					continue;
				}	
				if (lMidiIn.Count < RcvArray.Length
				 && ("nanoKONTROL2" == input
				 || input.StartsWith("USB-Midi")))
				{
					s += "\thandled";
					InputMidiSetup(i, input);
				}
			}
			OKSHmenu.Info(s);
			Model.MidiStatus = s;
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
	}
}
