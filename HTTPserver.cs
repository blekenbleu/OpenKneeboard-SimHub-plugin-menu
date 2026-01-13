// https://gist.github.com/define-private-public/d05bc52dd0bed1c4699d49e2737e80e7
// Author:    Benjamin N. Summerton <define-private-public>        
// License:   Unlicense (http://unlicense.org/)

using System;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
    class HttpServer
    {
        public static HttpListener listener = null;
        public static string url = "http://localhost:8765/";
        public static int pageViews = 0;
        public static int requestCount = 0;
        public static bool runServer = true;
        public static string pageData = 
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>HttpListener Example</title>" +
            "  </head>" +
            "  <body>" +
            "    <p>Page Views: {0}</p>" +
            "    <form method=\"post\" action=\"shutdown\">" +
            "      <input type=\"submit\" value=\"Shutdown\" {1}>" +
            "    </form>" +
            "  </body>" +
            "</html>";


        public static async Task HandleIncomingConnections()
        {
            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                OKSHmenu.Info($"Request #: {++requestCount} "
                    + req.Url.ToString() + "\n\t" 
                    + req.HttpMethod + "\n\t"
                    + req.UserHostName + "\n\t"
                    + req.UserAgent);

                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    OKSHmenu.Info($"{url} Shutdown requested");
                    runServer = false;
                }

                // Make sure we don't increment the page views counter if `favicon.ico` is requested
                if (req.Url.AbsolutePath != "/favicon.ico")
                    pageViews += 1;

                // Write the response info
                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }


        public static void Serve()
        {
            // Create a Http server and start listening for incoming connections
			if (null == listener)
			{
            	listener = new HttpListener();
            	listener.Prefixes.Add(url);
            	listener.Start();
			}
            OKSHmenu.Info("Listening for connections on " + url);

            // Handle requests
			runServer = true;
            Task listenTask = HandleIncomingConnections();
/*          listenTask.GetAwaiter().GetResult();

            // Close the listener
			OKSHmenu.Info("Closing {url} listener");
            listener.Close();
 */
        }
    }
}
