using System.Threading;
using System.Threading.Tasks;

namespace blekenbleu.SimHub_Remote_menu
{
	public partial class Control
	{
		SemaphoreSlim semaphore = new SemaphoreSlim(1);		// Only 1 task at a time

		internal async Task EventHandler(string name, int payload)
		{
			await semaphore.WaitAsync();
			try
			{
				await Task.Run(() => PayloadHandler(name, payload));
			}
			finally
			{
				semaphore.Release();
			}
		}

		void PayloadHandler(string name, int payload)
		{
			if ("MIDI" == name)
				Process(payload);
			else if (-1 == payload)
			{
				if ("bm" == name)
					NotEarn();
				else if (Earn)				// alternative event handling
					Learn(name);
				else ClickHandle(name);
			}
			else if (-2 == payload)
			{
				if (Earn)	// map a MIDI axis to slider via click list
				{
					if (button)
						Model.MidiStatus = "\nMIDI control >>only<< for button; ignored";
					else ListClick(name);	// Control.midi.cs
				}
				else OK.FromSlider(SL.Value);
			}
		}
	}
}
