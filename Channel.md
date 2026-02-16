[*back*](README.md)  
## WebMenu MIDI input support

If/when `MIDI learn` button is pressed, the plugin must
- set `learn = true`, disable UI event handling, then enable [`Channel`](Channel.cs), if not already
	- [Start() MidiIn](https://github.com/blekenbleu/MIDIio/blob/UI/docs/map.md)
	- make 'MIDI remove` button visible

- prompt for Delete or MIDI event, then associated UI
	- if Delete, then remove from event list
    - if event, then associate with UI button or slider as appropriate, replacing any previous association

When `MIDI learn` is pressed while `true == learn`
- set `learn = false`
- make 'MIDI remove` button invisible
- clear MIDI prompts
- if event list is empty, kill `Channel` and Midi inputs
	- else kill unused MidiIn devices
- update WebMenu UI event handling, *then* enable

---

### MIDI event list processing
- Midi input events occur in `reader.TryRead()`
- check if device and MIDI channel message are in event list
  - invoke matching UI event handler
- check if event device is in MidiOut list
  - map device to MidiOut channel number and forward it
- similarly for VJoy and property/event list handling


### [Channel programming](https://github.com/blekenbleu/MIDIio/blob/UI/docs/map.md#queue-multiple-midi-device-inputs-by-systemthreadingchannels) reference links
- [An Introduction](https://devblogs.microsoft.com/dotnet/an-introduction-to-system-threading-channels/)  
`while (await channelReader.WaitToReadAsync())	//` [false when channel is closed](https://learn.microsoft.com/en-us/dotnet/api/system.threading.channels.channelreader-1.waittoreadasync?view=net-10.0&viewFallbackFrom=netframework-4.8)  
`    while (channelReader.TryRead(out T item))	//` empty the queue  
`        Use(item);`

- [How to Build](https://oneuptime.com/blog/post/2026-01-30-dotnet-custom-channels/view)
```
// Bounded with drop: drops items when full (useful for telemetry)
var boundedDrop = Channel.CreateBounded(new BoundedChannelOptions(100)
{
    FullMode = BoundedChannelFullMode.DropOldest
});
```

- [Explained - Medium](https://medium.com/@abhirajgawai/c-channels-explained-from-producer-consumer-basics-to-high-performance-net-systems-f8ab610c0639) - [`Channel.CreateBounded()`](https://learn.microsoft.com/en-us/dotnet/core/extensions/channels#bounded-creation-patterns)  
```
var channel = Channel.CreateBounded<Event>(new BoundedChannelOptions(100)
{
    FullMode = BoundedChannelFullMode.Wait,
    SingleReader = true,
    SingleWriter = false
});
```
