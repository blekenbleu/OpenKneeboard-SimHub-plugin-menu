using System.Collections.Generic;
using System.Text;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
    partial class HttpServer    // works in .NET Framework 4.8 WPF User Control library (SimHub plugin)
    {
		internal static string HTMLtable(List<Values> sV)
		{
			StringBuilder builder = new StringBuilder();	// https://jonskeet.uk/csharp/stringbuilder.html
			builder.Append("<table id=tok style=\"font-size: 25px;\">\n<tr><th>Property</th><th>Current</th><th>Previous</th><th>Default</th></tr>");
			for (int i = 0; i < sV.Count; i++)
				builder.Append($"\n<tr><td>{sV[i].Name}</td><td>{sV[i].Current}</td><td>{sV[i].Previous}</td><td>{sV[i].Default}</td></tr>");
			builder.Append("</table>");
			return builder.ToString();
		}
	}
}
