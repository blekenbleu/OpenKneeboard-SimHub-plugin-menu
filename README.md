## [OpenKneeboard](https://openkneeboard.com/)-[SimHub](https://www.simhubdash.com)-plugin-menu
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
- [JSONio](https://github.com/blekenbleu/JSONio)
- [MIDIio](https://github.com/blekenbleu/MIDIio)

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

## [Real-Time Updates with SSE in C# .NET: A Complete Guide (2025)](https://dev.to/mayank_agarwal/implementing-real-time-updates-with-server-sent-events-sse-in-c-net-a-complete-guide-248l)

[w3schools HTML SSE API:  receive and server side examples](https://w3schools.w3schoolsapp.com/html/html5_serversentevents.html)  

[HTML Living Standard SSE](https://html.spec.whatwg.org/multipage/server-sent-events.html#server-sent-events)

- [SSE data as JSON sent and received](https://codelucky.com/javascript-onmessage-event-sse/)  

#### [.NET 9 WebApplication SSE tutorial and source (2024)](https://www.strathweb.com/2024/07/built-in-support-for-server-sent-events-in-net-9/)

- [**.NET 8 WebApplication C# SSE server** 2023](https://medium.com/@kova98/server-sent-events-in-net-7f700b21cdb7)  

- stackoverflow [Implement sending Server Sent Events in C# (no ASP.NET / MVC / ...)](https://stackoverflow.com/questions/44851970/implement-sending-server-sent-events-in-c-sharp-no-asp-net-mvc)  

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
```
var elem = document.querySelector("#tableID").children[0].children[row];  
elem.scrollIntoView(true);
```

### [highlight selected table row](https://jsfiddle.net/armaandhir/Lgt1j68s/)
from: [stack**overflow**](https://stackoverflow.com/questions/14443533/highlighting-and-un-highlight-a-table-row-on-click-from-row-to-row)
