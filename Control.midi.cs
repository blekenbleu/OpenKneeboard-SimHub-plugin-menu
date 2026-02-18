
namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	/// <summary>
	/// MIDI interaction code for Control.xaml
	/// </summary>
	public partial class Control
	{
		static string sb4;			// prompt string when !learn
		// https://github.com/blekenbleu/OpenKneeboard-SimHub-plugin-menu/blob/main/Channel.md#midi-device-name-handling
		void Learn(string butName)	// manage MIDI messages
		{
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
	}
}
