using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	// Source - https://stackoverflow.com/questions/28899954/net-server-sent-events-using-httphandler-not-working
	// Posted by GreysonTyrus, modified by community. See post 'Timeline' for change history
	// Retrieved 2026-01-13, License - CC BY-SA 4.0

	partial class HttpServer
	{
		private static bool SSEtimeout = true, SSEonce = true;
		private static HttpListenerContext SSEcontext = null;
		private static int foo = 0;

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

		// HandleIncomingConnections() continues after invoking this
		private static async Task KeepAliveAsync()
		{
			await SSEtimer();	// keep-alive
			OKSHmenu.Info("KeepAliveAsync(): SSEtimer() ended.");
		}

		private static async Task SSErve(byte[] data)
		{
			try	// if this takes "too long", call `Response.Close()`
			{
//				https://learn.microsoft.com/en-us/dotnet/api/system.io.stream?view=netframework-4.8
				SSEcontext.Response.ContentType = "text/event-stream";
				SSEcontext.Response.AddHeader("Cache-Control", "no-cache");
				SSEcontext.Response.AddHeader("Access-Control-Allow-Origin", "*");
				SSEcontext.Response.AddHeader("Connection", "keep-alive");
				SSEcontext.Response.ContentEncoding = Encoding.UTF8;
				await SSEcontext.Response.OutputStream.WriteAsync(data, 0, data.Length);	// System.IO.Stream
//				if (3 < foo)
//					await Task.Delay(2000);	// simulate Write() hang
				SSEcontext.Response.OutputStream.Flush();
			}
			catch (HttpListenerException)
			{
				OKSHmenu.Info($"SSErve(): lost connection");
				SSEcontext.Response.Close();
				SSEcontext = null;
			}
		}

		// https://stackoverflow.com/questions/28899954/net-server-sent-events-using-httphandler-not-working
		public static void SSEvent(string name, string data)
		{
			SSErespond("event: " + name + "\ndata:{" + data + "}");
		}

		public static void SSErespond(string responseText)
		{
			SSEtimeout = false;
			if (null == SSEcontext)
			{
				if (SSEonce)
				{
					OKSHmenu.Info("SSErespond():  null SSEcontext");
					SSEonce = false;
				}
				return;
			}

			Task delay = SSErve(Encoding.UTF8.GetBytes(string.Format("data: "
								+ js.Serialize(responseText) + "\n\n")));
			if (null != SSEcontext && !delay.Wait(1000))
			{
				SSEcontext.Response.Close();
				SSEcontext = null;
				OKSHmenu.Info($"SSErespond(): foo = {foo} hung");
			}	
		}

		public async static Task SSEtimer()		// hopefully long-running
		{
			if (null != SSEcontext)
				OKSHmenu.Info("SSEtimer(): launching");
			while (null != SSEcontext)
			{
				if (SSEtimeout)
				{
					SSErespond($"keep-alive {++foo} async");
					if (null == SSEcontext)
						return;
				}
				SSEtimeout = true;
				await Task.Delay(5000);
			}
			OKSHmenu.Info("SSEtimer():  SSEcontext.Response.OutputStream.Write() timeout");
		}
	}		// class
}			// namespace
