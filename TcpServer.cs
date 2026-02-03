// https://stackoverflow.com/a/26562695

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
	class SSES								// SSE connections
	{
		NetworkStream ns;
		private StreamWriter sw;
		public NetworkStream Ns { get => ns; set => ns = value; }
		public StreamWriter Sw { get => sw; set => sw = value; }
	}

	partial class HttpServer				// works in .NET Framework 4.8 WPF User Control library (SimHub plugin)
	{
		internal static string SliderProperty;
		internal static double SliderValue;
		static TcpListener listener;		// works for any IP addresses
		static readonly Int32 port = 8765;
		static List<SSES> clients;
		static bool ServerLoop;
		static Task keepalive;

		// TcpListener server with List<SSES> clients:
		// https://medium.com/@jm.keuleyan/c-tcp-communications-building-a-client-server-chat-a2155d585191

		// https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient?view=netframework-4.8
		// TcpClient creates a Socket to send and receive data, accessible as TcpClient.Client
		// https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient.client?view=netframework-4.8

		private static readonly string get = "";
		private static readonly string end = "</body></html>";
		private static readonly string head = 
			"\n<html>" +
			  "<head><style>th, td {padding-right: 30px;}</style>" +
				"\n<title>HttpListener Example</title>" +
				"\n<link rel='icon' href=" +
					"'https://media.geeksforgeeks.org/wp-content/cdn-uploads/gfg_200X200.png'" +
					" type='image/x-icon'>" +
			  "\n</head>\n" +
			  "\n<body style='font-size: 30px; margin-left: 50px;'>";

		static bool W(StreamWriter sw, string s)
		{
			try {
	  			OKSHmenu.Info("W:\t"+s);
				sw.WriteLine(s);
			} catch(Exception exp) {
				OKSHmenu.Info("W():  "+exp.Message);
				return false; 
			}
			return true;
		}

		static bool W(StreamWriter sw)
		{
			return W(sw, "");
		} 

		static void Table (SSES ss)
		{
			StreamWriter sw = ss.Sw;
			if (W(sw, "HTTP/1.1 200 OK")
			&& W(sw, $"Content-Type:text/html; charset=UTF-8"))
			{
				// Write the response info
				string data = head + get + HTMLtable(OKSHmenu.simValues) + end;
				if (W(sw, $"Content-Length: {data.Length}"))
					W(sw, data+"\n\n");
				W(sw);
				sw.Flush();
//				sw.Close();
			}	
//			ns.Close();
		}

		// add SSES() to clients;  leave ns open
		static void Sse(SSES ss)
		{
			string SSEhead = "HTTP/1.1 200 OK\nContentType: text/event-stream"
                        + "\nCache-Control: no-cache"
						+ "\nX-Accel-Buffering: no"
						+ "\nAccess-Control-Allow-Origin: *"
//						+ "\nConnection: keep-alive"
						+ "\n";
			try {
				StreamWriter sw = ss.Sw;
				W(sw, SSEhead);
				// update set <table> scroll, slider
				if (SSErve(sw, "event: scroll\ndata:{"
						+ "\"row\": \"1\"}")
				 && SSErve(sw, "event: slider\ndata:{"
						+ $"\"prop\": \"{SliderProperty}\", \"val\": \"{SliderValue}\"" + "}"))
				{
					clients.Add(ss);
					OKSHmenu.Info($"TcpServe():  client List count {clients.Count}");
					if (1 == clients.Count)
						keepalive = SSEtimer();
				}
			} catch (Exception exp) {
				OKSHmenu.Info($"SSE():  "+exp.Message);
			}
		}

		// Served page is passive; only SSE from JavaScript is supported.
		// Any other request gets table<>

		// https://dev.to/nickproud/net-tcplistener-accepting-multiple-client-connections-108d

		static void End(SSES ss)
		{
			ss.Ns.Close();
			clients.Remove(ss);
			OKSHmenu.Info($"TcpServer.End():  client List count {clients.Count}");
		}

		static JavaScriptSerializer js;
		static string localIP;
		// called in OKSHmenu.End();
		internal static void Close()
		{
			ServerLoop = false;
			clients = null;
		}

		public static async Task TcpServe()
		{
			OKSHmenu.Info($"TcpServe():  TcpListener.Start {localIP} port {port}");
			for (ServerLoop = true; ServerLoop;)
			{
				TcpClient server = await listener.AcceptTcpClientAsync();
				// sort out /SSE clients here, then only await those
				NetworkStream ns = server.GetStream();
					// https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader?view=netframework-4.8
					StreamReader sr = new StreamReader(ns);
					StreamWriter sw = new StreamWriter(ns);
					SSES ss = new SSES() { Ns = ns, Sw = sw };
					
					while (server.Connected && ServerLoop && !sr.EndOfStream)
					{
						string msg = await sr.ReadLineAsync();
						// https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader.readlineasync?view=netframework-4.8
						if (null == msg)
							continue;
						if (msg.StartsWith("GET /SSE"))
						{
							OKSHmenu.Info($"HttpServe(): new message = '"+msg+"'");
							Sse(ss);
//							break;
						}
						else if (msg.StartsWith("GET"))
						{
							OKSHmenu.Info($"HttpServe(): new message = '"+msg+"'");
							Table(ss);
						}
					}
			}
			OKSHmenu.Info($"Closing {localIP} listener");
		}

		// called in OKSHmenu.Init()
		internal static void Serve()
		{
			js = new JavaScriptSerializer();		// reuse for each SSE
			clients = new List<SSES> { };	// list management should all occur while this task runs
			using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
			{
				socket.Connect("8.8.8.8", 65530);
				IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
				localIP = endPoint.Address.ToString();
			}
			listener = new TcpListener(IPAddress.Any, Convert.ToInt32(port));
			listener.Start();
			Task.Run(() => TcpServe());
		}	// Serve()
	}	   // class	HttpServer
} 			// namespace
