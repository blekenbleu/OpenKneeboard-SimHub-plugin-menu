### Server-Sent Event (SSE) HTML `<table>` cell updates
- Update &lt;table&gt; cells [using JavaScript](JavaScript.cs)
	- [Learn Server-Sent Events in C# .NET - c-sharpcorner.com](https://www.c-sharpcorner.com/article/learn-server-sent-events-in-c-sharp-net/) minimal C# + JavaScript
	- send C# `Dictionary<>` to client as a `<table>`, along with JavaScript, as a normal web page
		- [Set up `<table>` with id](https://www.w3schools.com/htmL/html_id.asp)
			- [then access `<table>` by id in JavaScript](https://www.w3schools.com/jsref/dom_obj_table.asp)
		- JavaScript  `EventSource()` initiates SSE
		- [*generated HTML received by browser*](HTML.txt)
	- [Consuming Server-Sent Events in JavaScript](https://www.milanjovanovic.tech/blog/server-sent-events-in-aspnetcore-and-dotnet-10#consuming-server-sent-events-in-javascript) using `EventSource` with reconnect on error
	- [JavaScript dynamically update `<table>` cells - stackoverflow](https://stackoverflow.com/questions/27859976/how-to-dynamically-change-html-table-content-using-javascript)
	- https://www.geeksforgeeks.org/html/how-to-add-edit-and-delete-data-in-html-table-using-javascript/
	- [2015 JavaScript live `<table>` update](https://datatables.net/forums/discussion/26983/how-to-do-live-table-updates-with-html5-sse)

Invoke that `<table>` cell change JavaScript in custom `table` event listener  
```
const source = new EventSource('SSE');
source.addEventListener('table', (event) => {tableUpdate(event.data);});
```


[w3schools HTML SSE API:  receive and server side examples](https://w3schools.w3schoolsapp.com/html/html5_serversentevents.html)  

[HTML Living Standard SSE](https://html.spec.whatwg.org/multipage/server-sent-events.html#server-sent-events)

- [SSE data as JSON sent and received](https://codelucky.com/javascript-onmessage-event-sse/)  
- [SSE JavaScript onmessage Event](https://codelucky.com/javascript-onmessage-event-sse/)


## [access `<table>` cell value in JavaScript...not jQuery](https://stackoverflow.com/questions/3072233/getting-value-from-table-cell-in-javascript-not-jquery)
```
<html>
<head>
  <title>simplistic example</title>
</head>
<body>
  <table id='tableID'>
    <tr>
      <td>one</td> 
      <td>two</td>
      <td>three</td>
      <td>four</td>
    </tr>
  </table>

<script>
let refTab = document.getElementById("tableID")
// Loop through all rows and columns of the table
for ( var i = 0; i<refTab.rows.length; i++ ) {
   let row = refTab.rows[i];
   for ( var j = 0; col = row.cells[j]; j++ )
      alert(col.firstChild.value);
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

### [highlight selected `<table>` row](https://jsfiddle.net/armaandhir/Lgt1j68s/)
from: [stack**overflow**](https://stackoverflow.com/questions/14443533/highlighting-and-un-highlight-a-table-row-on-click-from-row-to-row)

## JavaSript EventSource
- [**EventSource is only text** - javascript.info](https://javascript.info/server-sent-events)
	- EventSource event types:&nbsp; `onopen`, `onmessage`, `onerror`
		- and 3 readyState property values: `CONNECTING`, `OPEN`, `CLOSED`
	- JSON-encoded messages:
		- `data: {"user":"John","message":"First line\n Second line"}`
		- [JSON.parse(event.data) - mozilla.org](https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events/Using_server-sent_events#listening_for_custom_events):&nbsp;
		`const user = JSON.parse(event.data).user;`
- [HTML Server-Sent Events API - w3schools](https://www.w3schools.com/html/html5_serversentevents.asp)
- [mozilla](https://developer.mozilla.org/en-US/docs/Web/API/EventSource)
