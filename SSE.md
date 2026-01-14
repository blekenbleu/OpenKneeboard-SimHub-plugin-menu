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
