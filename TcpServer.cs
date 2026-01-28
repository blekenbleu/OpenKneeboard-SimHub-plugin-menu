// https://stackoverflow.com/a/26562695

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
    partial class HttpServer    // works in .NET Framework 4.8 WPF User Control library (SimHub plugin)
    {
		private static TcpListener listener;	// also works for other IP addresses
		private static bool listen = true; // <--- boolean flag to exit loop
		static Int32 port = 5678;

		private static async Task HandleClient(TcpClient clt)
		{
    		using NetworkStream ns = clt.GetStream();
    		using StreamReader sr = new StreamReader(ns);
			OKSHmenu.Info($"HandleClient(): new message");
			while (listen)
			{
    			string msg = await sr.ReadLineAsync();
                OKSHmenu.Info("HandleClient():  " + msg);
            }
			OKSHmenu.Info($"HandleClient(): ending");
        }

		public static async Task TcpServe()
		{
			listener = new TcpListener(IPAddress.Any, port);
			listener.Start();
			OKSHmenu.Info($"TcpServe():  TcpListener.Start port {port}");
    		for (listen = true; listen;)
        		if (listener.Pending())
				{
//					var v = (IPEndPoint)listener.LocalEndpoint;	// always 0.0.0.0
//					OKSHmenu.Info($"TcpServe():  TcpListener {v} port {port}");
            		await HandleClient(await listener.AcceptTcpClientAsync());
				}
        		else await Task.Delay(1000); //<--- timeout
			OKSHmenu.Info($"TcpServe(): ending");
        }
	}       // class
} 			// namespace
