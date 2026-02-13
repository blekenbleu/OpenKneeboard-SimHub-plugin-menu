#### [Web Server using C# `TcpListener`](https://github.com/blekenbleu/HttpServer), *NOT* [`HttpListener`](https://aksakalli.github.io/2014/02/24/simple-http-server-with-csparp.html)
- `HttpListener` on Windows 11 Home - worked for me only for localhost (`127.0.0.1`)
- [other doomed alternatives and experiments](Doomed.md)
- [**`TcpListener` works for Internet address**]()
	- requires [hand-coding HTTP header content](https://developer.mozilla.org/en-US/docs/Web/HTTP/Guides/Session)
	- GitHub inspiration: [TcpMultiClient](http://github.com/blekenbleu/TcpMultiClient)
	- serve `<table>` for display by HTTP clients.
		- SSE responses using the same `TcpClient` and `NetworkStream` as `<table>` response
		- After the initial 'Content-Type: text/event-stream\n\n' header response,
			send each SSE *with no header*.
	- [server-sent-events-are-just-http-requests](https://jvns.ca/blog/2021/01/12/day-36--server-sent-events-are-cool--and-a-fun-bug/#server-sent-events-are-just-http-requests)
	- [Each event is sent as a block of text terminated by a pair of newlines.](https://medium.com/@truongtronghai/content-type-text-event-stream-b81fee085ee1)
	- [9.2 Server-sent events spec](https://html.spec.whatwg.org/multipage/server-sent-events.html)
	- [SSE Server Response](https://dev.to/serifcolakel/real-time-data-streaming-with-server-sent-events-sse-1gb2)
```
HTTP/1.1 200 OK
Content-Type: text/event-stream
Cache-Control: no-cache
Connection: keep-alive
```

#### Because *only* a Server-Sent Events application
- seemingly need to *maintain* per-connection Tasks;&nbsp; all SSE client connections have the same state
	- send page with *current* `<table>` and `script` in response to initial `GET /`
	- accumulate a `ConcurrentDictionary<>` of `GET /SSE` connections
		- immediately send current `<table>` row and slider settings to each new `GET /` connection 
- send update Events to each connection
