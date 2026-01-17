// https://gist.github.com/define-private-public/d05bc52dd0bed1c4699d49e2737e80e7
// https://16bpp.net/tutorials/csharp-networking/02/
// Author:	Benjamin N. Summerton <define-private-public>		
// License:   Unlicense (http://unlicense.org/)

using System;
using System.Diagnostics.Tracing;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	class HttpServer	// works in .NET Framework 4.8 WPF User Control library (SimHub plugin)
	{
		public static HttpListener listener = null;
		public static string[] urls = { "http://localhost:8765/", "http://127.0.0.1:8765/" };
		public static int pageViews = 0;
		public static int requestCount = 0;
		public static string head = 
			"<!DOCTYPE>" +
			"<html>" +
			"  <head>" +
			"	<title>HttpListener Example</title>" +
			"   <link rel=\"icon\" href=" +
            "   \"https://media.geeksforgeeks.org/wp-content/cdn-uploads/gfg_200X200.png\"" +
			"	type=\"image/x-icon\">" +
			"  </head>" +
			"  <body>";
		public static string pageData =
			"	<p>Page Views: {0};&nbsp; Request Count: {1}</p>" +
			"	<form method=\"post\" action=\"shutdown\">" +
			"	  <input type=\"submit\" value=\"Shutdown\" {2}>" +
			"	</form>";
		public static string end =
			"  </body>" +
			"</html>";
		private static bool SSEtimeout = true;
		private static HttpListener SSElistener = null;
		private static HttpListenerContext SSEcontext = null;

		// should be a thread, instead of task..?
		// https://stackoverflow.com/questions/4672010/multi-threading-with-net-httplistener
		// Multi-threading with .Net HttpListener
		// http://beonebeauty.net/faq/csharp/multithreading-with-net-httplistener.html
		// multithreaded simple HttpListener webserver
		// https://github.com/arshad115/HttpListenerServer
		// Using HTTPListener to build a HTTP Server in C#
		// https://thoughtbot.com/blog/using-httplistener-to-build-a-http-server-in-csharp
		// Task.Run(async () => await SomeAsyncMethod()).Wait();
		// https://www.w3tutorials.net/blog/understanding-the-use-of-task-run-wait-async-await-used-in-one-line/
		// Handling multiple requests with C# HttpListener
		// https://www.iditect.com/faq/csharp/handling-multiple-requests-with-c-httplistener.html
		// https://stackoverflow.com/questions/9034721/handling-multiple-requests-with-c-sharp-httplistener
		// multi-threaded c# http server - THREAD-SAFE?
		// https://stackoverflow.com/questions/6371741/production-ready-multi-threaded-c-sharp-http-server
		public static async Task HandleIncomingConnections()
		{
			bool first = true;

			// While a user hasn't visited the `shutdown` url, keep on handling requests
			for (bool runServer = true; runServer;)
			{
				// Will wait here until we hear from a connection
				HttpListenerContext ctx = await listener.GetContextAsync();

				// Peel out the requests and response objects
				HttpListenerRequest req = ctx.Request;
				HttpListenerResponse resp = ctx.Response;

				requestCount++;
				if (true)
				{
					first = false;
					// Print out some info about the request
					OKSHmenu.Info($"HandleIncomingConnections(): Request #: {requestCount} "
						+ req.Url.ToString() + "\n\t"
						+ req.HttpMethod + "\n\t"
						+ req.UserHostName + "\n\t"
						+ req.UserAgent);
				}

				// If `shutdown` url requested w/ POST, then shutdown the server after serving the page
				if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
				{
					OKSHmenu.Info("HandleIncomingConnections(): Shutdown requested");
					runServer = false;
				}
				else if (req.Url.AbsolutePath.StartsWith("/SSE"))
				{
					if (null == SSEcontext)
					{
						SSElistener = listener;
						SSEcontext = ctx;
						Task<int> keepalive = KeepAliveAsync();
					}
					else OKSHmenu.Info($"HandleIncomingConnections(): non-null SSEcontext;  SSElistener is {(SSElistener.IsListening ? "" : "NOT")} listening");
					continue;	// Server-Sent Events:  do not close this context
				}

				// Make sure we don't increment the page views counter if `favicon.ico` is requested
				if (req.Url.AbsolutePath != "/favicon.ico")
					pageViews += 1;

				// Write the response info
				string disableSubmit = !runServer ? "disabled" : "";
				byte[] data = Encoding.UTF8.GetBytes(head + String.Format(pageData, pageViews, requestCount, disableSubmit) + end);
				resp.ContentType = "text/html";
				resp.ContentEncoding = Encoding.UTF8;
				resp.ContentLength64 = data.LongLength;

				// Write out to the response stream (asynchronously), then close it
				await resp.OutputStream.WriteAsync(data, 0, data.Length);
				resp.Close();
			}
		}

		// HandleIncomingConnections() continues after invoking this
		private static async Task<int> KeepAliveAsync()
		{
			await SSEtimer();	// keep-alive
			OKSHmenu.Info("KeepAliveAsync(): SSEtimer() ended.");
			return 0;
		}

		// called in OKSHmenu.Init()
		public static void Serve()
		{
			SSEcontext = null;
			SSElistener = null;
			// Create a Http server and start listening for incoming connections
			listener = new HttpListener();
			foreach (string url in urls)
				listener.Prefixes.Add(url);
//			listener.Prefixes.Add("http://192.168.1.147:8765/");	// needs elevated privileges
			try
			{
            	listener.Start();
			}
			catch (HttpListenerException hlex)
        	{
            	OKSHmenu.Info("Serve(): HttpListenerException transaction " + hlex);
            	return;
        	}
			OKSHmenu.Info($"Serve(): Listening for connections on {urls[0]}");
			// Handle requests
			Task listenTask = HandleIncomingConnections();
			// Close listener in OKSHmenu.End().
		}

		// https://stackoverflow.com/questions/28899954/net-server-sent-events-using-httphandler-not-working
		public static void SSEreponse(string responseText)
		{
			SSEtimeout = false;
			if (null == SSEcontext)
				return;

			SSEcontext.Response.ContentType = "text/event-stream";
			HttpListenerResponse response = SSEcontext.Response;
			response.AddHeader("Cache-Control", "no-cache");
			response.AddHeader("Access-Control-Allow-Origin", "*");
			JavaScriptSerializer js = new JavaScriptSerializer();
			byte[] data = Encoding.UTF8.GetBytes(string.Format("data: " + js.Serialize(responseText) + "\n\n"));
			response.ContentEncoding = Encoding.UTF8;
			response.ContentLength64 = data.LongLength;
			try	// if this takes "too long", call `response.Close()`
			{
//				response.Write(data, 0, data.Length);				// not in .NET 4.8
//				https://learn.microsoft.com/en-us/dotnet/api/system.io.stream?view=netframework-4.8
				response.OutputStream.Write(data, 0, data.Length);	// System.IO.Stream 
				response.OutputStream.Flush();
			}
			catch (Exception e)
			{
				OKSHmenu.Info($"SSEreponse():  {e}");
				SSEcontext.Response.Close();
				SSEcontext = null;
			}
			OKSHmenu.Info($"SSEreponse(): foo = {foo}; IsListening = {(SSElistener.IsListening ? "true" : "false")}");
		}

		private static int foo = 0;
		public async static Task SSEtimer()
        {
            OKSHmenu.Info("SSEtimer(): launched");
			SSEcontext.Response.OutputStream.WriteTimeout = 1000;	// accepted, but seemingly ignored
			while (null != SSElistener && SSElistener.IsListening)
			{
				if (SSEtimeout)
				{
					OKSHmenu.Info($"SSEtimer(foo = {++foo})");
					SSEreponse($"foo {foo} not async");	// this hangs for 2 == foo
				}
				SSEtimeout = true;
				await Task.Delay(2000);
			}
			OKSHmenu.Info("SSEtimer(): client not listening");
			SSEcontext = null;
        }
		// GetContextAsync() with Cancellation Support
		// https://stackoverflow.com/questions/69715297/getcontextasync-with-cancellation-support
	}
}
