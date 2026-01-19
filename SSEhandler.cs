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
		private static bool SSEtimeout = true;
		private static HttpListenerContext SSEcontext = null;

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
		private static async Task<int> KeepAliveAsync()
		{
			await SSEtimer();	// keep-alive
			OKSHmenu.Info("KeepAliveAsync(): SSEtimer() ended.");
			return 0;
		}

		static bool SSErunning = false;
		// https://stackoverflow.com/questions/28899954/net-server-sent-events-using-httphandler-not-working
		public static void SSEreponse(string responseText)
		{
			SSEtimeout = false;
			if (null == SSEcontext)
			{
				OKSHmenu.Info("SSEreponse():  null SSEcontext");
				return;
			}
			OKSHmenu.Info($"SSEreponse({responseText}): SSErunning = {(SSErunning ? "true" : "false")}"); 

			SSEcontext.Response.ContentType = "text/event-stream";
			HttpListenerResponse response = SSEcontext.Response;
			response.AddHeader("Cache-Control", "no-cache");
			response.AddHeader("Access-Control-Allow-Origin", "*");
			JavaScriptSerializer js = new JavaScriptSerializer();
			byte[] data = Encoding.UTF8.GetBytes(string.Format("data: " + js.Serialize(responseText) + "\n\n"));
			response.ContentEncoding = Encoding.UTF8;
			response.ContentLength64 = data.LongLength;
			Task delay;

			if (SSErunning)
				delay = Task.Delay(1000).ContinueWith(_ =>
				{
					OKSHmenu.Info("SSEreponse()Task.Delay(1000): response.OutputStream.WriteAsync() incomplete");
//					SSEcontext.Response.Close();
//					SSErunning = false;
				});

			try	// if this takes "too long", call `response.Close()`
			{
//				https://learn.microsoft.com/en-us/dotnet/api/system.io.stream?view=netframework-4.8
				SSErunning = true;
				response.OutputStream.WriteAsync(data, 0, data.Length);	// System.IO.Stream 
				SSErunning = false;
				response.OutputStream.Flush();
			}
			catch (Exception e)
			{
				OKSHmenu.Info($"SSEreponse():  {e}");
				SSEcontext.Response.Close();
				SSEcontext = null;
			}
			OKSHmenu.Info($"SSEreponse(): foo = {foo}; SSErunning = {(SSErunning ? "true" : "false")}");
			SSErunning = false;
		}

		private static int foo = 0;
		public async static Task SSEtimer()
		{
			if (null == SSEcontext)
			{
				OKSHmenu.Info("SSEtimer():  null SSEcontext");
				return;
			}

			OKSHmenu.Info("SSEtimer(): launching");
			while (OKSHlistener.IsListening)
			{
				if (SSEtimeout)
				{
					OKSHmenu.Info($"SSEtimer(foo = {++foo})");
					SSEreponse($"foo {foo} async");	// this hangs for 2 == foo
				}
				SSEtimeout = true;
				await Task.Delay(5000);
			}
			OKSHmenu.Info("SSEtimer(): client not listening");
			SSEcontext = null;
			return;
		}
		// GetContextAsync() with Cancellation Support
		// https://stackoverflow.com/questions/69715297/getcontextasync-with-cancellation-support
	
	}	// class
}		// namespace
