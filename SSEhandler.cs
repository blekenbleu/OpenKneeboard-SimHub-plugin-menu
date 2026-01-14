using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
    // Source - https://stackoverflow.com/questions/28899954/net-server-sent-events-using-httphandler-not-working
    // Posted by GreysonTyrus, modified by community. See post 'Timeline' for change history
    // Retrieved 2026-01-13, License - CC BY-SA 4.0

    public class SSEhandler : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			// https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Headers
			context.Response.ContentType = "text/event-stream";
			context.Response.AddHeader("Cache-Control", "no-cache");
			context.Response.AddHeader("Access-Control-Allow-Origin", "*");
			context.Response.AddHeader("Connection", "keep-alive");
			HttpResponse response = context.Response;

			DateTime startdate = DateTime.Now;

			while (startdate.AddMinutes(10) > DateTime.Now)
			{
				JavaScriptSerializer js = new JavaScriptSerializer();

				string responseText = DateTime.Now.TimeOfDay.ToString();

				response.Write(string.Format("data: {0}\n\n", js.Serialize(responseText)));
				response.Flush();
				System.Threading.Thread.Sleep(1000);
			}
			response.Close();
		}

		public bool IsReusable
		{
			get
			{
				return false;	// true requires thread-safe:  variables local to functions
			}
		}
	}
}
