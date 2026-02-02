using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	// Source - https://stackoverflow.com/questions/28899954/net-server-sent-events-using-httphandler-not-working
	//			https://learn.microsoft.com/en-us/dotnet/api/system.io.stream?view=netframework-4.8

	partial class HttpServer
	{
		private static bool SSEtimeout = true, SSEonce = true;
		private static int foo = 0;

		private static bool SSErve(StreamWriter sw, byte[] data)
		{
			try	// if this takes "too long", call `Response.Close()`
			{
				sw.Write(data.ToString()+"\n\n");	// System.IO.Stream
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
					if (!SSErve(client.Sw, Encoding.UTF8.GetBytes(responseText + "\n\n")))
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
