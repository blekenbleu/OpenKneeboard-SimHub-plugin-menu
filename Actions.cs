
namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	public partial class OKSHmenu
	{
		/// <summary>
		/// Helper functions used in Init() AddAction()s and Control.xaml.cs button Clicks
		/// </summary>
		/// <param name="sign"></param> should be 1 or -1
		/// <param name="prefix"></param> should be "in" or "de"
		public void Ment(int sign)
		{
			if (0 == Gname.Length || null == CurrentCar || 0 == CurrentCar.Length)
				return;

			int step = Steps[View.Selection];
			int iv = (int)(0.004 + 100 * float.Parse(simValues[View.Selection].Current));

			iv += sign * step;
			if (0 <= iv)
			{
				Current(View.Selection, (0 != step % 100) ? $"{(float)(0.01 * iv)}"
										: $"{(int)(0.004 + 0.01 * iv)}");
				Changed();
				if (slider == View.Selection)
					ToSlider();
			}
		}

		// Control.xaml actions -------------------------------------------------
		internal void FromSlider(double value)
		{
			Current(slider, (SliderFactor[0] * (int)value).ToString());
			Changed();
			Control.Model.SliderProperty =  simValues[slider].Name + ":  " + simValues[slider].Current;
		}

		internal void ToSlider()
		{
			if(0 > slider)
				return;

			Control.Model.SliderProperty = HttpServer.SliderProperty = simValues[slider].Name + ":  " + simValues[slider].Current;
			Control.Model.SliderValue = HttpServer.SliderValue = SliderFactor[1] * System.Convert.ToDouble(simValues[slider].Current);
		}

		private void SelectedStatus()
		{
			Control.Model.SelectedProperty = (0 > View.Selection) ? "unKnown" : simValues[View.Selection].Name;
			Control.Model.StatusText = Gname + " " + CurrentCar + ":\t" + Control.Model.SelectedProperty;
		}

		/// <summary>
		/// Select next or prior property; exception if invoked on other than UI thread
		/// </summary>
		/// <param name="next"></param> false for prior
		public void Select(bool next)
		{
			if (0 == Gname.Length || null == CurrentCar || 0 == CurrentCar.Length)
				return;

			if (next)
			{
				if (++View.Selection >= simValues.Count)
					View.Selection = 0;
			}
			else if (0 < View.Selection)	// prior
				View.Selection--;
			else View.Selection = (byte)(simValues.Count - 1);
			SelectedStatus();		// Select()
		}

		public void Swap()
		{
			string temp;
			for (int i = 0; i < simValues.Count; i++)
			{
				temp = simValues[i].Previous;
				Previous(i, simValues[i].Current);
				Current(i, temp);
			}
			ToSlider();		// Swap()
			Changed();
		}

		// set "CurrentAsDefaults" action
		internal void SetDefault()	// List<GameList> Glist)
		{
			if (0 == Gname.Length)
				OOps("SetDefault: no Gname");
			else {
				int p = View.Selection;

				Default(p, simValues[p].Current);	// End() sorts per-game changes
				Changed();
			}
		}

		// set "SelectedAsSlider" action
		internal void SelectSlider()	// List<GameList> Glist)
		{
			slider = View.Selection;
			SetSlider();
		}

		// simValues set methods
		string Default(int i, string value)
		{
			simValues[i].Default = value;
			HttpServer.SSEcell(1 + i, 3, value);
			return value;
		}

		string Current(int i, string value)
		{
			simValues[i].Current = value;
			HttpServer.SSEcell(1 + i, 1, value);
			return value;
		}

		string Previous(int i, string value)
		{
			simValues[i].Previous = value;
			HttpServer.SSEcell(1 + i, 2, value);
			return value;
		}
	}
}
