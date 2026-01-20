## .NET Framework 4.8 `HttpListener` Server-Sent Event handler
 &emsp; *SimHub plugins use* `.NET Framework 4.8`  

For `req.Url.AbsolutePath.StartsWith("/SSE")` in `HandleIncomingConnections()`
- make `SSEcontext` non-null:&nbsp; (`SSEcontext = ctx = await OKSHlistener.GetContextAsync();`)
- kick off a SSEkeep-alive Timer task (`Task keepalive = KeepAliveAsync();`)
```
    {
        await SSEtimer();   // keep-alive
        OKSHmenu.Info("KeepAliveAsync(): SSEtimer() ended.");
    }
```

`SSEtimer()` method periodically checks for SSEtimeout flag to be `true`  
- set `false` by `SSErespond()` table update data events  
```
	while (null != SSEcontext)
    {
        if (SSEtimeout)
        {
            SSErespond($"foo {++foo} async");
            if (null == SSEcontext)
                return;
        }
        SSEtimeout = true;
        await Task.Delay(5000);
    }
```
* `SSErespond()` sets `SSEcontext = null` for `HttpListenerException`s.

<hr>

### [.NET Framework and ASP.NET Core *11 Oct 2016*](https://odetocode.com/blogs/scott/archive/2016/10/11/asp-net-core-and-the-enterprise-part-1ndashframeworks.aspx)  
![](https://odetocode.com/images2/Open-Live-Writer/ASP.NET-and-the-Enterprise-Part-1Framewo_B5C4/frameworks_3.png)

#### [.NET Framework for server apps](https://learn.microsoft.com/en-us/dotnet/standard/choosing-core-framework-server) - alternative implementations
- [ASP.NET Core 7.0](https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/7.0/libuv-transport-dll-removed) seemingly broke ASP.NET Core support for `.Net Framework`
- [Kestrel web server running under .net framework](https://stackoverflow.com/questions/40013139/running-kestrel-webserver-on-net-framework-4-5)
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
<hr>

### (SSE) [`response.OutputStream.WriteAsync(data, 0, data.Length);` exceptions](https://github.com/jeremybytes/channel-exceptions)  
 &emsp; *detecting SSE client disconnects otherwise not possible?*  

- [`WriteAsync(Byte[], Int32, Int32, CancellationToken)`](https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.writeasync?view=netframework-4.8#system-io-stream-writeasync(system-byte()-system-int32-system-int32-system-threading-cancellationtoken))
- [`CancellationTokenSource.CancelAfter()`](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/cancel-async-tasks-after-a-period-of-time#complete-example)
- [`await Task.WhenAny(DoSomethingAsync(), Task.Delay(TimeSpan.FromSeconds(1)));`](https://devblogs.microsoft.com/oldnewthing/20220505-00/?p=106585)
- [Using CancellationTokenSource](https://www.webdevtutor.net/blog/c-sharp-kill-running-task)
- [How to: Cancel a Task and Its Children](https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-cancel-a-task-and-its-children)
- [Cancel async tasks after a period of time](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/cancel-async-tasks-after-a-period-of-time)
- [`Task.Delay()` is non-blocking](https://www.w3tutorials.net/blog/understanding-the-use-of-task-run-wait-async-await-used-in-one-line/)
- [`Task.Run()` vs `await`](https://dev.to/stevsharp/taskrun-vs-await-what-every-c-developer-should-know-1mmi)
	- use `Task.Run()` only when absolutely needed.
