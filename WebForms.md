## [Web Forms vs MVC in VS 2022](https://visualstudiomagazine.com/articles/2022/05/16/vs2022-web-forms-tip.aspx)
 &emsp; ["*If you have experience in WPF*"](https://learn.microsoft.com/en-us/aspnet/overview) - (also alternatives)  
- alternative [MVC](https://learn.microsoft.com/en-us/aspnet/mvc/overview/) can use [`<TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>`](https://github.com/robbiemcarthur/ASP.net-MVCtutorial)
	- [Razor Syntax](https://www.learnrazorpages.com/razor-syntax) uses e.g.
	[`<TargetFramework>net48</TargetFramework>`](https://github.com/tompazourek/AspNetMvcOnSdk/tree/feature/advanced)  
		- [M$ ASP.NET Web Pages (**Razor**) Guidance](https://learn.microsoft.com/en-us/aspnet/web-pages/overview/)
		- other examples:&nbsp; [KeepFormStateUsingSession](https://github.com/darrenji/KeepFormStateUsingSession), [MVCtutorial](https://github.com/robbiemcarthur/ASP.net-MVCtutorial)
			- [Razor **Pages** uses .NET 6](https://stackoverflow.com/a/56193541) - requiring [migration from .NET Framework 4.8](https://steven-giesel.com/blogPost/f531dded-44af-4341-871c-d54875877900)
- [official M$ Web Forms documentation](https://learn.microsoft.com/en-us/aspnet/web-forms/)

<hr>

### [.NET Framework and ASP.NET Core *11 Oct 2016*](https://odetocode.com/blogs/scott/archive/2016/10/11/asp-net-core-and-the-enterprise-part-1ndashframeworks.aspx)  
![](https://odetocode.com/images2/Open-Live-Writer/ASP.NET-and-the-Enterprise-Part-1Framewo_B5C4/frameworks_3.png)

#### [.NET Framework for server apps](https://learn.microsoft.com/en-us/dotnet/standard/choosing-core-framework-server) - alternative implementations
- [ASP.NET Web Forms (.NET Framework) in Visual Studio 2022](https://stackoverflow.com/questions/77712319/how-to-create-asp-net-web-forms-project-in-visual-studio-2022-using-empty-templa)
	- [ASP.NET Core 6.0](https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/release-notes/aspnetcore-6.0.md) broke ASP.NET Core support for `.Net Framework`
- [Developing Web apps with ASP.NET and .NET Framework - M$](https://learn.microsoft.com/en-us/dotnet/framework/develop-web-apps-with-aspnet)
- [Learn ASP.NET MVC 5 on .NET Framework](https://www.tutorialsteacher.com/mvc) - TutorialsTeacher
- [Getting started with ASP.NET MVC 5 - M$](https://learn.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/getting-started) - [GitHub sample app](https://github.com/kexuelou/MvcMovie5)

### [ASP.NET Core webserver interface](https://aspnetcore.readthedocs.io/en/stable/fundamentals/servers.html)

<hr>

- [Kestrel web server running under .net framework](https://stackoverflow.com/questions/40013139/running-kestrel-webserver-on-net-framework-4-5)
	- copy `libuv.dll` to `bin\Debug`
	- "Kestrel is not a fully-featured web server
	 	It was designed to make ASP.NET as fast as possible  
		but is limited in its ability to manage security and serve static files."
	- [What is Kestrel Web Server?](https://stackify.com/what-is-kestrel-web-server/)  
		All ASP.NET Core apps utilize a new MVC framework and the Kestrel web server.  
		These new apps can run on full .NET Framework or .NET Core.
	- [Kestrel from NuGet](https://github.com/dotnet/aspnetcore/discussions/62810)
	- [Using WebListener in ASP.NET Core](https://anuraj.dev/blog/using-weblistener-in-aspnet-core/) - [GitHub](https://github.com/Azure-Samples/service-fabric-dotnet-web-reference-app)
	- [Kestrel Server-Sent Events](https://github.com/aspnet/Tooling/issues/359) using SignalR 2.2 through Owin  
		Kestrel is Microsoft's cross-platform development OWIN compatible web server for ASP.NET 5.  
		ASP.NET 5 has a "bridge" enabling OWIN components to run on Kestrel.
		- [example code](https://fast-endpoints.com/docs/server-sent-events)
		- [Open Web Interface for .NET (OWIN) with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/owin?view=aspnetcore-1.0)
	- [Setup Kestrel to use http2](https://stackoverflow.com/questions/58682933/how-do-i-enable-http2-in-c-sharp-kestrel-web-server-over-plain-http)
		- [Deep Dive with Real-World Examples](https://www.c-sharpcorner.com/article/understanding-kestrel-web-server-in-net-core-deep-dive-with-real-world-exa/)
