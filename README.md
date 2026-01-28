## [OpenKneeboard](https://openkneeboard.com/)-[SimHub](https://www.simhubdash.com)-[plugin-menu](https://github.com/blekenbleu/OpenKneeboard-SimHub-plugin-menu/wiki)
*[SimHub plugin properties](https://github.com/blekenbleu/JSONio) HTTP table for e.g. [OpenKneeboard](https://github.com/OpenKneeboard/OpenKneeboard)*
![](example.png)  
### background
Currently, access from SteamVR to SimHub (and its plugin menus) is by e.g.
- [SteamVR's Desktop](https://store.steampowered.com/news/app/250820/view/2898585530113863169)
- [Desktop+](https://steamcommunity.com/app/1494460)

SimHub dash overlays lack VR support, but [OpenKneeboard can display SimHub overlays](https://www.madmikeplays.com/free-downloads#block-yui_3_17_2_1_1742822224076_6340) via HTTP.

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

### plan
- generate an [HTML `<table>`](HTML.md) from `NCalcScripts/OKSHpm.ini` JSON properties during `Init()`
- hand-code [JavaScript](JavaScript.md) for browser to update `<table>` from Server-Sent Events
- send [HTML](HTML.cs) + [JavaScript](JavaScript.cs) page to client browsers
- send Server-Sent Events for `<table>` cell property values and e.g. scroll actions
	- *working, but disconnected client causes SimHub game disconnects and reconnects*
	- `HttpListener` without admin privelege can only serve to LocalHost on Windows 11
		- *to do*: experiment with [`TcpListener`](TcpServer.cs) - not handling multiple connections
			= as yet, no HTTP served...

### [Writing a Web Server using C# HttpListener](https://aksakalli.github.io/2014/02/24/simple-http-server-with-csparp.html)
- [ doomed alternatives and experiments](Doomed.md)

## SimHub plugins are .NET Framework 4.8 WPF User Control libraries
## SimHub plugin build process
### .NET Framework v4.8
Windows-only version of .NET for building client and server applications;  
newest version supported by M$ is 4.8.1 (*August 9th, 2022*), but SimHub uses 4.8  
In Visual Studio, `new project > WPF User Control Library (.NET Framework)`  
- `WPF User Control Library` has xaml
