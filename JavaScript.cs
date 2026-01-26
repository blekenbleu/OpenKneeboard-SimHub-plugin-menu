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
const msg = document.getElementById('msg');
const label = document.getElementById('active');
const slider = document.getElementById('myRange');
const table = document.getElementById('tok');
let rows = table.getElementsByTagName('tr');

const blurt = (string) => {
  console.log(string);
  msg.innerHTML = string'
};

const tableUpdate = (data) => {
  let obj = JSON.parse(data);
  let r = obj.row;
  let c = obj.col;
  table.rows[r].cells[c].innerHTML = obj.val;
};

// Table Row Background Colors
const tableScroll = (data) => {
  let foo = JSON.parse(data).foo;
  for(i = 0; i < rows.length; i++)
	rows[i].style.backgroundColor = (foo == i) ? '#ffffff' : '#888888';
};

const slide = (data) => {
	let obj = JSON.parse(data);
	label.innerHTML = obj.id;
	slider.value = obj.val;
};

// Listen for the specific event types defined in C#
source.addEventListener('table', (event) => {
  tableUpdate(event.data);
});

source.addEventListener('scroll', (event) => {
  tableScroll(event.data);
});

source.addEventListener('slider', (event) => {
  slide(event.data);
});

source.onopen = () => {
  blurt('Connection opened');
};

source.onmessage = (event) => {
  msg.innerHTML = event.data;
  console.log('Received message:', event);
};

source.onerror = (e) => {
  let oops = 'Error: ' + JSON.parse(e);
  console.error(oops);
  msg.innerHTML = oops;
  if (source.readyState === EventSource.CONNECTING)
	blurt('Reconnecting...');
};
</script>";			// string JavaScript

	}       // class
}           // namespace
