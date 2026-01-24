namespace blekenbleu.OpenKneeboard_SimHub_plugin_menu
{
    partial class HttpServer    // works in .NET Framework 4.8 WPF User Control library (SimHub plugin)
    {
		public static string JavaScript =	// https://www.milanjovanovic.tech/blog/server-sent-events-in-aspnetcore-and-dotnet-10#consuming-server-sent-events-in-javascript
"
const eventSource = new EventSource('/orders/realtime/with-replays');

// Listen for the specific 'orders' event type we defined in C#
eventSource.addEventListener('orders', (event) => {
  const payload = JSON.parse(event.data);
  console.log(`New Order ${event.lastEventId}:`, payload.data);
});

// Do something when the connection opens
eventSource.onopen = () => {
  console.log('Connection opened');
};

// Handle generic messages (if any)
eventSource.onmessage = (event) => {
  console.log('Received message:', event);
};

// Handle errors and reconnections
eventSource.onerror = () => {
  if (eventSource.readyState === EventSource.CONNECTING) {
    console.log('Reconnecting...');
  }
};
";
	}       // class
}           // namespace
