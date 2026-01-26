using System.Collections.Generic;
using System.Text;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
    partial class HttpServer    // works in .NET Framework 4.8 WPF User Control library (SimHub plugin)
    {
		internal static string HTMLtable(List<Values> sV)
		{
			StringBuilder builder = new StringBuilder();	// https://jonskeet.uk/csharp/stringbuilder.html

			builder.Append("<table id=tok style='font-size: 25px;'>\n<tr><th>Property</th><th>Current</th><th>Previous</th><th>Default</th></tr>");
			for (int i = 0; i < sV.Count; i++)
				builder.Append($"\n<tr><td>{sV[i].Name}</td><td>{sV[i].Current}</td><td>{sV[i].Previous}</td><td>{sV[i].Default}</td></tr>");
			builder.Append("</table>");

			// add a slider and label
			builder.Append("<label id=active for='myRange'>unset:</label>");		// replace 'unset:'
			builder.Append("<input type='range' value='50' id='myRange'>");			// replace	'50'
			// add a paragraph for messages
			builder.Append("<p id=msg><i>messages here</i></p>");					// replace 'messages here'
//			builder.Append(JavaScript);
			return builder.ToString();
		}
	}
}
