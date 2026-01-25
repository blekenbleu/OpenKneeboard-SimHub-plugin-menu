namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
    partial class HttpServer    // works in .NET Framework 4.8 WPF User Control library (SimHub plugin)
    {

/*		Custom SSE events
			https://medium.com/pon-tech-talk/a-simple-guide-to-server-sent-events-sse-and-eventsource-9de19c23645b
			https://github.com/omer-pon/sse-eventsource

		https://www.w3schools.com/Js/js_json_parse.asp
		JavaScript minification:  https://github.com/trullock/NUglify
		JavaScript Debugging:  https://www.w3schools.com/js/js_debugging.asp
*/
		public static string JavaScript =	// https://www.milanjovanovic.tech/blog/server-sent-events-in-aspnetcore-and-dotnet-10#consuming-server-sent-events-in-javascript
"<script>
const source = new EventSource('http://localhost:8765/SSE');
const table = document.getElementById("tok");
let rows = table.getElementsByTagName("tr");

const tableUpdate = (data) => {
  let obj = JSON.parse(data);
  let r = obj.row;
  let c = obj.col;
  table.rows[r].cells[c].innerHTML = obj.value;
};

// Table Row Background Colors
const tableScroll = (data) => {
  let foo = JSON.parse(data).foo;
  for(i = 0; i < rows.length; i++)
	rows[i].style.backgroundColor = (foo == i) ? '#ffffff' : '#888888';
};

// Listen for the specific event types defined in C#
source.addEventListener('table', (event) => {
  tableUpdate(event.data);
});

source.addEventListener('scroll', (event) => {
  tableScroll(event.data);
});

source.onopen = () => {
  console.log('Connection opened');
};

source.onmessage = (event) => {
  console.log('Received message:', event);
};

source.onerror = (e) => {
  console.error("Error: " + JSON.parse(e));
  if (source.readyState === EventSource.CONNECTING)
	console.log('Reconnecting...');
};
</script>";			// string JavaScript

	}       // class
}           // namespace
