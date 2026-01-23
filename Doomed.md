## Doomed experiments and alternatives
### experiments for Razor
- [IPASideLoader](https://github.com/blekenbleu/IPASideLoader)
- [ASPNETSingle](https://github.com/blekenbleu/ASPNETSingle)
- [MVC](https://github.com/blekenbleu/MVC)
- [WebForm](https://github.com/blekenbleu/WebForm)
- [MvcMovie5](https://github.com/blekenbleu/MvcMovie5)

### alternatives for SSE server
- [**Web Forms vs MVC in VS 2022**](WebForms.md)
- [Using HTTPListener to build a HTTP Server in C#](https://thoughtbot.com/blog/using-httplistener-to-build-a-http-server-in-csharp)  
- [EventSource.cs *2014* (569 lines)](https://gist.github.com/igolaizola/3ad45ea8135ad8e5fd06)
- [IAsyncResult Interface](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncresult?view=netframework-4.8), from
        [Asynchronous Programming Model (APM)](https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/asynchronous-programming-model-apm)
- [Server-sent event (SSE) with ASHX (.Net framework 4.6)](https://laucsharp.blogspot.com/2020/06/server-sent-event-sse-with-ashx-net.html)
- [**Practical ASP.NET Web API.pdf**](https://accorsi.net/docs/Practical%20ASP.NET%20Web%20API.pdf) - 12.2 Pushing Real-time Updates to the Client  
- [.Net Server-Sent Events using IHttpHandler](https://stackoverflow.com/questions/28899954/net-server-sent-events-using-httphandler-not-working)
- [C# ASP Generic Handler - problems with response inside of a callback](https://stackoverflow.com/questions/52243962/c-sharp-asp-generic-handler-problems-with-response-inside-of-a-callback)
- [Server Sent Events not sent ASP.Net 4](https://stackoverflow.com/questions/25713016/server-sent-events-not-sent-asp-net-4)
- [Server Sent Events (SSE) not working with ASHX Handler](https://stackoverflow.com/questions/29276147/asp-net-javascript-jquery-server-sent-events-sse-not-working-with-ashx-hand)
- [Displaying Real Time Data using HTML5 and ASP.NET *Feb 7, 2013*](https://www.codeguru.com/csharp/displaying-real-time-data-using-html5-and-asp-net/)
- [SseFormatter.WriteAsync Method](https://learn.microsoft.com/en-us/dotnet/api/system.net.serversentevents.sseformatter.writeasync?view=netframework-4.8-pp)
	- [nuget](https://www.nuget.org/packages/System.Net.ServerSentEvents)
- [Katana - Microsoft Owin components](https://devblogs.microsoft.com/dotnet/katana-asp-net-5-and-bridging-the-gap/)
- [Microsoft.Owin.Host.HttpListener](https://www.nuget.org/packages/Microsoft.Owin.Host.HttpListener) - nuget
	- OWIN server built on the .NET Framework 4.5's HttpListener class.  
		 Currently the default server used for self-hosting.
	- [Use OWIN to Self-Host ASP.NET Web API](https://learn.microsoft.com/en-us/aspnet/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api) - learn.microsoft.com
	- [Use OWIN to Self-Host ASP.NET Web API](https://www.learnonlineasp.net/2017/10/use-owin-to-self-host-aspnet-web-api.html) - learnonlineasp.net
	- [Use OWIN to Self-Host ASP.NET Web API](https://github.com/dotnet/AspNetDocs/blob/main/aspnet/web-api/overview/hosting-aspnet-web-api/use-owin-to-self-host-web-api.md) - github.com/dotnet/AspNetDocs
- [Microsoft.Owin.SelfHost](https://www.nuget.org/packages/Microsoft.Owin.SelfHost/) - nuget
- [Microsoft.AspNet.SignalR](https://github.com/SignalR/SignalR) - nuget for use in .NET Framework applications using System.Web or Katana
	- [Simple SignalR Server apps](https://www.codeproject.com/articles/Simple-SignalR-Server-and-Client-Applications-Demo) - [GitHub](https://github.com/nthdeveloper/SignalRSamples)
	- [ASP.NET Core SignalR & SignalR Debugging Tool](https://medium.com/@dwivedi.gourav/asp-net-core-signalr-signalr-debugging-tool-a82dc5230035) - [**SignalR Debugging Tool**](https://gourav-d.github.io/SignalR-Web-Client/dist/)
	- [Building a Real-Time Event Viewer in C# with SignalR and Server-Sent Events](https://en.ittrip.xyz/c-sharp/real-time-viewer-csharp)
- [.NET 9-10 `System.Net.ServerSentEvents` example solutions **using `WriteAsync` may** port](https://dev.to/mashrulhaque/server-sent-events-in-net-10-finally-a-native-solution-22kg)  
	- [**Minimal API example**](https://fullstackcity.com/server-sent-events-in-net-10) sends web page
	- [Real-Time Updates with SSE in C# ASP.NET: A Complete Guide (2025)](https://dev.to/mayank_agarwal/implementing-real-time-updates-with-server-sent-events-sse-in-c-net-a-complete-guide-248l)  
	- [.NET 9 WebApplication SSE tutorial and source (2024)](https://www.strathweb.com/2024/07/built-in-support-for-server-sent-events-in-net-9/)  
	- [Consuming Server-Sent Events in JavaScript](https://www.milanjovanovic.tech/blog/server-sent-events-in-aspnetcore-and-dotnet-10#consuming-server-sent-events-in-javascript)
	- [Real-Time `ServerSentEvents` example](https://www.codemag.com/Article/2309051/Developing-Real-Time-Web-Applications-with-Server-Sent-Events-in-ASP.NET-7-Core)
	- [Milan Jovanovic YouTube](https://www.youtube.com/watch?v=JKCyvlh0_mE)
	- [.NET 9 example with `WriteAsync`](https://medium.com/@tomas.madajevas/server-sent-events-and-rx-for-requests-interception-3e6b2356a9b6)
	- [.NET 8 WebApplication C# SSE server 2023 using WebApplication.CreateBuilder](https://medium.com/@kova98/server-sent-events-in-net-7f700b21cdb7) - [**GitHub**](https://github.com/kova98/BackendCommunicationPatterns.NET)

### [Writing a Web Server using C# TcpListener](https://iamsimi.medium.com/writing-a-web-server-in-c-12b93134082bA)
### [SimpleHttpServer using C# `System.Net.Sockets`](https://www.codeproject.com/articles/Simple-HTTP-Server-in-C) - [GitHub](https://github.com/jeske/SimpleHttpServer)
### [.NET Framework 4.8 `System.Net.ServerSentEvents SseFormatter Class`](https://learn.microsoft.com/en-us/dotnet/api/system.net.serversentevents.sseformatter?view=netframework-4.8-pp)
Writes source of server-sent events to destination stream.  

<details><summary><b>.NET 9.0 WPF Class Library - doomed for SimHub</b></summary>
Unrecognized as a plugin by SimHubWPF.exe;&nbsp;  <a href=https://dotnet.microsoft.com/en-us/download/visual-studio-sdks>.NET 8 and newer are cross-platform, open-source</a><ul>
<li> created a new Visual Studio .NET 9.0 WPF Class Library named <code>OpenKneeboard-SimHub-plugin-menu</code><ul>
	<li> copied those <code>OpenKneeboard-SimHub-plugin-menu.csproj OpenKneeboard-SimHub-plugin-menu.sln</code> into this repository
	<li>from <code>SimHub/PluginSdk/JSONio</code>, copied <code>Control.xaml  Control.xaml.cs  JSONio.cs Properties Settings.cs  Slim.cs  Values.cs  ViewModel.cs  bin  js.cs sdkmenuicon.png</code>
	<li>renamed <code>namespace</code> to </code>blekenbleu.OpenKneeboard_SimHub_plugin_menu</coe>
	<li>deleted <code>GlobalSection(ExtensibilityGlobals)</code> in <code>OpenKneeboard-SimHub-plugin-menu.sln</code></ul></ul>
To debug executible from Visual Studio UI, hacked into <code>.csproj</code>:<br>
<code>  &lt;PropertyGroup Condition="'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'"&gt;
    &lt;OutputPath&gt;$(SIMHUB_INSTALL_PATH)&lt;/OutputPath&gt;
    &lt;StartAction&gt;Program&lt;/StartAction&gt;
    &lt;StartProgram&gt;$(SIMHUB_INSTALL_PATH)SimHubWPF.exe&lt;/StartProgram&gt;
  &lt;/PropertyGroup&gt; </code>
</details>
