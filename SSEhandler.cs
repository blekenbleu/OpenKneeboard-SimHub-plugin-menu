using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	// Source - https://stackoverflow.com/questions/28899954/net-server-sent-events-using-httphandler-not-working
	//			https://learn.microsoft.com/en-us/dotnet/api/system.io.stream?view=netframework-4.8

	partial class HttpServer
	{
		private static bool SSEtimeout = true, SSEonce = true;
		private static int foo = 0;

		private static bool SSErve(StreamWriter sw, string data)
		{
			try
			{
				if (!data.EndsWith("\n"))
					data += "\n";
				string sse = Encoding.UTF8.GetString(Encoding.Default.GetBytes(data));
				W(sw, sse);	// System.IO.Stream
				sw.Flush();
				return true;
			}
			catch (Exception oops)
			{
				OKSHmenu.Info($"SSErve(): " + oops.Message);
				sw.Close();
				return false;
			}
		}

		// https://stackoverflow.com/questions/28899954/net-server-sent-events-using-httphandler-not-working
		public static void SSEvent(string name, string data)
		{
			SSErespond("event: " + name + "\ndata:{" + data + "}");
		}

		// send event to each client
		public static void SSErespond(string responseText)
		{
			SSEtimeout = false;
			if (null != clients && 0 < clients.Count)
			{
				SSEonce = true;
				foreach(var client in clients)
					if (!SSErve(client.Sw, responseText))
						End(client);
			} else {
				if (SSEonce)
					OKSHmenu.Info("SSErespond():  no clients");
				SSEonce = false;
			}
		}

		public async static Task SSEtimer()		// hopefully long-running
		{
			OKSHmenu.Info("SSEtimer(): launching");
			while (ServerLoop)
			{
				if (SSEtimeout)
					SSErespond($"data: keep-alive {++foo} async");
				SSEtimeout = true;
				await Task.Delay(15000);
			}
			OKSHmenu.Info("SSEtimer(): exiting");
		}
	}		// class
}			// namespace
