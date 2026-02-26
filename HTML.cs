using System.Collections.Generic;
using System.Text;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	partial class HttpServer	// works in .NET Framework 4.8 WPF User Control library (SimHub plugin)
	{
		internal static string HTMLtable(List<Values> sV, string tableStyle = "font-size: 25px;",
				 string messageStyle = "width:600; color:navy; background-color:silver")
		{
			StringBuilder builder = new StringBuilder();	// https://jonskeet.uk/csharp/stringbuilder.html

			builder.Append("<table id=tok style='").Append(tableStyle).Append("'>\n<tr><th>Property</th><th>Current</th><th>Previous</th><th>Default</th></tr>");
			foreach (var row in sV)
				builder.Append($"\n<tr><td>{row.Name}</td><td>{row.Current}</td><td>{row.Previous}</td><td>{row.Default}</td></tr>");
			builder.Append("\n</table>");

			// add a paragraph for messages
			builder.Append("\n<p id=msg style='{messageStyle}'><i>messages here</i></p>");				// replace 'messages here'
			// add a slider and label
			builder.Append("\n<label id=active for='myRange'>unset:</label>");							// replace 'unset:'
			builder.Append("\n<input type='range' value='50' id='myRange' style='width:500'>");			// replace	'50'
			builder.Append(JavaScript);
			return builder.ToString();
		}
	}
}
