// https://stackoverflow.com/a/26562695

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
    partial class HttpServer    // works in .NET Framework 4.8 WPF User Control library (SimHub plugin)
    {
        private static TcpListener listener;    // also works for other IP addresses
        private static bool listen = true; // <--- boolean flag to exit loop
        static readonly Int32 port = 5678;
        static List<NetworkStream> clients;

        // TcpListener server with List<TcpClient> clients:
        // https://medium.com/@jm.keuleyan/c-tcp-communications-building-a-client-server-chat-a2155d585191
        // 

        // https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient?view=netframework-4.8
        // TcpClient creates a Socket to send and receive data, accessible as TcpClient.Client
        // https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient.client?view=netframework-4.8
        private static async Task HttpServe(TcpClient server)
        {
            if (server.Connected)
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.networkstream?view=netframework-4.8
                using (NetworkStream ns = server.GetStream())
                {
                    // https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader?view=netframework-4.8
                    using StreamReader sr = new StreamReader(ns);
                    OKSHmenu.Info($"HttpServe(): new message");
                    while (server.Connected && listen && !sr.EndOfStream)
                    {
                        string msg;
                        // https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader.readlineasync?view=netframework-4.8
                        do
                        {
                            msg = await sr.ReadLineAsync();
                            if (null != msg && 0 < msg.Length && msg.StartsWith("GET /"))
                            {
                                if (msg.StartsWith("GET /SSE"))
                                    clients.Add(ns);
                                OKSHmenu.Info("HttpServe():  " + msg);
                            }
                        } while (null != msg && 0 < msg.Length);
                        await Task.Delay(1000);
                    }
                	OKSHmenu.Info($"HttpServe():  end clients List count {clients.Count}");
                	clients.Remove(ns);
                }
            }
        }

		// SSE place-holder tasks; if a Write fails, set Readable = false
        private static async Task NetServe(TcpClient server, NetworkStream ns)
        {
			while (server.Connected && listen && ns.Writeable && ns.Readable)
				await Task.Delay(1000);
			OKSHmenu.Info("NetServe(): ending");
        }

        // https://dev.to/nickproud/net-tcplistener-accepting-multiple-client-connections-108d
        public static async Task TcpServe()
        {
            clients = new List<NetworkStream> { };	// list management should all occur in this task
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            OKSHmenu.Info($"TcpServe():  TcpListener.Start port {port}");
            for (listen = true; listen;)
            {
                TcpClient server = await listener.AcceptTcpClientAsync();
				// sort out /SSE clients here, then only await those
				using (NetworkStream ns = server.GetStream())
                {
					// https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader?view=netframework-4.8
                    using StreamReader sr = new StreamReader(ns);
                    if (server.Connected && listen && !sr.EndOfStream)
                    {
                        string msg = await sr.ReadLineAsync();
                        // https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader.readlineasync?view=netframework-4.8
                        if (null != msg && 0 < msg.Length)
                        {
                    		OKSHmenu.Info($"HttpServe(): new message");
                            if (msg.StartsWith("GET /"))
                            {
                                if (msg.StartsWith("GET /SSE"))
								{
                                    clients.Add(ns);
									OKSHmenu.Info($"TcpServe():  start clients List count {clients.Count}");
									await Task.Run(() => NetServe(server, ns));   // is await ok here?
                    				OKSHmenu.Info($"TcpServe():  end clients List count {clients.Count}");
                    				clients.Remove(ns);
								}
								else do {
                                	OKSHmenu.Info("HttpServe():  " + msg);
									msg = await sr.ReadLineAsync();
								} while (null != msg && 0 < msg.Length && listen && server.Connected && !sr.EndOfStream);
                            }
                        }
                    }
				}
            }
            OKSHmenu.Info($"TcpServe(): ending");
        }
    }	   // class
} 			// namespace
