## [OpenKneeboard](https://openkneeboard.com/)-[SimHub](https://www.simhubdash.com)-[plugin-menu](https://github.com/blekenbleu/OpenKneeboard-SimHub-plugin-menu/wiki)
*display a SimHub plugin menu in OpenKneeboard*

### background
Currently, SimHub (and its plugin menus) access from SteamVR is by e.g.
- SteamVR's Desktop utility
- [Desktop+](https://steamcommunity.com/app/1494460)

While SimHub lacks VR overlay support, [its overlays can be displayed using OpenKneeboard](https://www.madmikeplays.com/free-downloads#block-yui_3_17_2_1_1742822224076_6340) via HTTP.

### wanted
An always visible dedicated menu display controlled e.g. by steering wheel buttons or rotary encoders  
for tweaking e.g. harness tensioner or haptics settings (properties).  
- instead of involving an overlay GUI,
	- directly manipulate HTML table of numeric values
	- scroll the table and increment/decrement table values by e.g. rotary encoders or MIDI control surface

HTML table cell updates should have lower processing overhead than graphical overlay..

### resources
- from [JSONio](https://github.com/blekenbleu/JSONio):
	- in `OpenKneeboard-SimHub-plugin-menu.csproj`:  
	 could not get [ReferencePath](https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/project-build/troubleshooting-broken-references)
	 working;&nbsp; copied JSONio `HintPath`s  
	- SimHub confused `OKSHmenu` with `JSONio` plugin until renaming `class JSONio`.  
	`KSHmenu.ChangeProperties` needs its own `ExternalScript.CarChange` event trigger setting  
	in **SimHub Controls and events**.
- [MIDIio](https://github.com/blekenbleu/MIDIio)
- [**A Simple HTTP SSE server in C#**](SSE.md) - proof of concept -
  [Gist](https://gist.github.com/define-private-public/d05bc52dd0bed1c4699d49e2737e80e7)
	- [Server-sent event (SSE) with ASHX (.Net framework 4.6)](https://laucsharp.blogspot.com/2020/06/server-sent-event-sse-with-ashx-net.html)
	- [WebForms VS MVC](WebForms.md)

### Server-Sent Event (SSE) HTML table cell updates
- Update &lt;table&gt; cells using JavaScript
	- https://www.htmlgoodies.com/html5/updating-html-table-content-using-javascript/
	- https://stackoverflow.com/questions/27859976/how-to-dynamically-change-html-table-content-using-javascript
	- https://www.geeksforgeeks.org/html/how-to-add-edit-and-delete-data-in-html-table-using-javascript/
	- [2015 JavaScript live table update](https://datatables.net/forums/discussion/26983/how-to-do-live-table-updates-with-html5-sse)
- [Set up table with id](https://www.w3schools.com/htmL/html_id.asp)
	- [then access table by id in javascript](https://www.w3schools.com/jsref/dom_obj_table.asp)

Invoke that table cell change javascript in registered event listener  
```
var source = new EventSource('updates.cgi');
source.onmessage = function (event) {
    alert(event.data);
};
```


[w3schools HTML SSE API:  receive and server side examples](https://w3schools.w3schoolsapp.com/html/html5_serversentevents.html)  

[HTML Living Standard SSE](https://html.spec.whatwg.org/multipage/server-sent-events.html#server-sent-events)

- [SSE data as JSON sent and received](https://codelucky.com/javascript-onmessage-event-sse/)  

## SimHub plugins are .NET Framework 4.8 WPF User Control libraries
### [.NET Framework 4.8 `System.Net.ServerSentEvents SseFormatter Class`](https://learn.microsoft.com/en-us/dotnet/api/system.net.serversentevents.sseformatter?view=netframework-4.8-pp)
Writes source of server-sent events to destination stream.  
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

- [EventSource.cs *2014* (569 lines)](https://gist.github.com/igolaizola/3ad45ea8135ad8e5fd06)


### [Server Side Event implementation *Python* source using `FastAPI`](https://malnossi.github.io/blog/server-side-events/)

## [access table cell value in JavaScript...not jQuery](https://stackoverflow.com/questions/3072233/getting-value-from-table-cell-in-javascript-not-jquery)
```
<html>
<head>
  <meta http-equiv="content-type" content="text/html; charset=windows-1250">
  <meta name="generator" content="PSPad editor, www.pspad.com">
  <title></title>
</head>
<body>
  <table id='tableID'>
    <tr>
      <td>dfsdf</td> 
      <td>sdfs</td>
      <td>frtyr</td>
      <td>hjhj</td>
    </tr>
  </table>

<script>
var refTab = document.getElementById("tableID")
// Loop through all rows and columns of the table
for ( var i = 0; i<refTab.rows.length; i++ ) {
   row = refTab.rows[i];
   for ( var j = 0; col = row.cells[j]; j++ ) {
      alert(col.firstChild.value);
   }
}

// change it? r: row; c: column
document.querySelector("#tableID").children[0].children[r].children[c].innerText = value;
</script>
</body>
</html>
```

### [Scroll a Table in JavaScript](https://stackoverflow.com/questions/7852986/javascript-scroll-to-nth-row-in-a-table)
```
// Source - http://jsfiddle.net/r753v2ky/
// Posted by Gabriele Petrioli, modified by community. See post 'Timeline' for change history
// Retrieved 2026-01-10, License - CC BY-SA 4.0

var rows = document.querySelectorAll('#tableid tr');

// line is zero-based
// line is the row number that you want to see into view after scroll    
rows[line].scrollIntoView({
    behavior: 'smooth',
    block: 'center'
});
```

### [`scrollIntoView` is now a "Baseline Widely available*" feature](https://developer.mozilla.org/en-US/docs/Web/API/Element/scrollIntoView)
` var elem = document.querySelector("#tableID").children[0].children[row];`  
` elem.scrollIntoView(true);`

### [highlight selected table row](https://jsfiddle.net/armaandhir/Lgt1j68s/)
from: [stack**overflow**](https://stackoverflow.com/questions/14443533/highlighting-and-un-highlight-a-table-row-on-click-from-row-to-row)

### [Writing a Web Server using C# TcpListener](https://iamsimi.medium.com/writing-a-web-server-in-c-12b93134082bA)
### [Writing a Web Server using C# HttpListener](https://aksakalli.github.io/2014/02/24/simple-http-server-with-csparp.html)
### [SimpleHttpServer using C# `System.Net.Sockets`](https://www.codeproject.com/articles/Simple-HTTP-Server-in-C) - [GitHub](https://github.com/jeske/SimpleHttpServer)

## SimHub plugin build process
### .NET Framework v4.8
Windows-only version of .NET for building client and server applications;  
newest version supported by M$ is 4.8.1 (*August 9th, 2022*), but SimHub uses 4.8  
In Visual Studio, `new project > WPF User Control Library (.NET Framework)`  
- `WPF User Control Library` has xaml

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
