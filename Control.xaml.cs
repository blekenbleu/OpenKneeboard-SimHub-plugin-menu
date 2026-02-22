using SimHub.Plugins.DataPlugins.DataCore;
using System.Windows;
using System.Windows.Controls;

/* XAML DataContext:  Binding source
 ;	https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-specify-the-binding-source?view=netframeworkdesktop-4.8
 ;	https://www.codeproject.com/articles/126249/mvvm-pattern-in-wpf-a-simple-tutorial-for-absolute
 ;	alternatively, DataContext in XAML	https://dev.to/mileswatson/a-beginners-guide-to-mvvm-using-c-wpf-241b
 */

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	/// <summary>
	/// Interaction code for Control.xaml
	/// </summary>
	public partial class Control : UserControl
	{
		public static OKSHmenu OK;
		public static ViewModel Model;				// reference XAML controls
		internal byte Selection;					// changes only in OKSHmenu.Select() on UI thread
		internal static string version = "1.71";

		public Control() {							// called before simValues are initialized
			Model = new ViewModel(this);
			InitializeComponent();
			DataContext = Model;					// StaticControl events change Control.xaml binds
			learn = false;                          // Control.midi.cs
		}

		public Control(OKSHmenu plugin) : this()
		{
			OK = plugin;							// Control.xaml button events call OKSHmenu methods
			dg.ItemsSource = OKSHmenu.simValues;	// bind XAML DataGrid to OKSHmenu.cs List<Values> simValues
		}

		private void Hyperlink_RequestNavigate(object sender,
									System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
		}

		internal static void OOpsMB()
		{
			Model.StatusText = OKSHmenu.Msg;
			System.Windows.Forms.MessageBox.Show(OKSHmenu.Msg, "OKSHmenu");
		}

		// highlights selected property cell
		internal void Selected()					// crashes if called from other threads
		{
			if ((dg.Items.Count > Selection) && (dg.Columns.Count > 2))
			{
				//Select the item.
				dg.CurrentCell = new DataGridCellInfo(dg.Items[Selection], dg.Columns[1]);
				dg.SelectedCells.Clear();
				dg.SelectedCells.Add(dg.CurrentCell);
				HttpServer.SSEscroll(Selection);
			}
		}

		// xaml DataGrid:  Loaded="DgSelect"
		private void DgSelect(object sender, RoutedEventArgs e)
		{
			Selected();
		}

		// handle all button events in one method
		internal void ButEvent(object sender, RoutedEventArgs e)
		{
			string butName = (e.OriginalSource as FrameworkElement).Name;

			if ("bm" == butName)
				Unlearn();
			else if (learn)		 // alternative event handling
				Learn(butName);
			else ButHandle(butName);
		}

		internal static void ButHandle(string butName)
		{
			Model.MidiStatus = "";
			switch(butName)
			{
				case "b0":
					OK.Select(false);
					break;
				case "b1":
					OK.Select(true);
					break;
				case "b2":
					OK.Ment(1);
					break;
				case "b3":
					OK.Ment(-1);
					break;
				case "b4":
					OK.Swap();
					break;
				case "b5":
					OK.SetDefault();
					break;
				case "SB":
					OK.SelectSlider();
					break;
				default:
					Model.StatusText = "ButHandle(): unconfigured button '{butName)'";
					OOpsMB();
					break;
			}
		}

		// handle slider changes
		private void Slider_DragCompleted(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			OK.FromSlider(0.5 + SL.Value);
		}
	}
}
