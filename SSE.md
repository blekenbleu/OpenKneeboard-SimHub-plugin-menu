## primitive .NET Framework 4.8 `HttpListener` Server-Sent Event handler
For "SSE" == `req.Url.AbsolutePath`, `HandleIncomingConnections()`
- makes `SSEcontext` non-null  
- kicks off a SSEkeep-alive Timer task

`SSEtimer()` method checks every minute or so for SSEtimeout flag to be `true`  
- `if (null == SSEcontext)` then `return`
- `if (SSEtimeout)`, then `SSEreponse(SSEkeep-alive)`
- set `SSEtimeout = true`;  

`SSEreponse()` method resets `SSEtimeout` flag and sends data  
- will also be invoked for table value changes...

<hr>

### [.NET Framework and ASP.NET Core *11 Oct 2016*](https://odetocode.com/blogs/scott/archive/2016/10/11/asp-net-core-and-the-enterprise-part-1ndashframeworks.aspx)  
![](https://odetocode.com/images2/Open-Live-Writer/ASP.NET-and-the-Enterprise-Part-1Framewo_B5C4/frameworks_3.png)

- [.NET Framework for server apps](https://learn.microsoft.com/en-us/dotnet/standard/choosing-core-framework-server)
- [ASP.NET Core 7.0](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/7.0/libuv-transport-dll-removed) seemingly broke ASP.NET Core support for .Net Framework
- [Kestrel web server in a application running under .net framework](https://stackoverflow.com/questions/40013139/running-kestrel-webserver-on-net-framework-4-5)
	- copy `libuv.dll` to `bin\Debug`
	- "Kestrel is not a fully-featured web server
	 	It was designed to make ASP.NET as fast as possible  
		but is limited in its ability to manage security and serve static files."
	- [What is Kestrel Web Server?](https://stackify.com/what-is-kestrel-web-server/)  
		All ASP.NET Core apps utilize a new MVC framework and the Kestrel web server.  
		These new apps can run on full .NET Framework or .NET Core.
	- [Kestrel Server-Sent Events](https://github.com/aspnet/Tooling/issues/359) using SignalR 2.2 through Owin  
		Kestrel is Microsoft's cross-platform development OWIN compatible web server for ASP.NET 5.  
		ASP.NET 5 has a "bridge" enabling OWIN components to run on Kestrel.
		- [example code](https://fast-endpoints.com/docs/server-sent-events)
		- [Open Web Interface for .NET (OWIN) with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/owin?view=aspnetcore-1.0)
	- [Setup Kestrel to use http2](https://stackoverflow.com/questions/58682933/how-do-i-enable-http2-in-c-sharp-kestrel-web-server-over-plain-http)
		- [Deep Dive with Real-World Examples](https://www.c-sharpcorner.com/article/understanding-kestrel-web-server-in-net-core-deep-dive-with-real-world-exa/)
