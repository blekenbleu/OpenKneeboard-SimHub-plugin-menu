using System.Collections.Generic;
using System.Text;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	partial class HttpServer	// works in .NET Framework 4.8 WPF User Control library (SimHub plugin)
	{
		internal static string HTMLtable(List<Values> sV)
		{
			StringBuilder builder = new StringBuilder();	// https://jonskeet.uk/csharp/stringbuilder.html

			builder.Append("<table id=tok style='font-size: 25px;'>\n<tr><th>Property</th><th>Current</th><th>Previous</th><th>Default</th></tr>");
			for (int i = 0; i < sV.Count; i++)
				builder.Append($"\n<tr><td>{sV[i].Name}</td><td>{sV[i].Current}</td><td>{sV[i].Previous}</td><td>{sV[i].Default}</td></tr>");
			builder.Append("\n</table>");

			// add a paragraph for messages
			builder.Append("\n<p id=msg style='width:600; color:navy; background-color:silver'><i>messages here</i></p>");	// replace 'messages here'
			// add a slider and label
			builder.Append("\n<label id=active for='myRange'>unset:</label>");		// replace 'unset:'
			builder.Append("\n<input type='range' value='50' id='myRange' style='width:500'>");			// replace	'50'
			builder.Append(JavaScript);
			return builder.ToString();
		}
	}
}
