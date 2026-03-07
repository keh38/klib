# KLib.Net Developer Guide

`KLib.Net` is a .NET library providing TCP messaging and UDP network discovery for local network communication between applications. All classes are in the `KLib.Net` namespace.

**Dependencies:** Newtonsoft.Json

---

## Table of Contents

1. [Quick Start](#quick-start)
2. [Network Discovery](#network-discovery)
3. [UDP Discovery Beacon](#udp-discovery-beacon)
4. [UDP Discovery Listener](#udp-discovery-listener)
5. [TCP Messaging — The TcpMessage Protocol](#tcp-messaging--the-tcpmessage-protocol)
6. [TCP Server — KTcpListener](#tcp-server--ktcplistener)
7. [TCP Client — KTcpClient](#tcp-client--ktcpclient)
8. [Large File Transfer](#large-file-transfer)
9. [Complete End-to-End Example](#complete-end-to-end-example)
10. [Data Transfer Objects (DTOs)](#data-transfer-objects-dtos)
11. [Status Code Reference](#status-code-reference)

---

## Quick Start

The following gets a server and client exchanging a `Ping` message over localhost.

**Server:**
```csharp
var endpoint = Discovery.FindNextAvailableEndPoint();
var beacon = new DiscoveryBeacon("MY_SERVER", endpoint.Address.ToString(), endpoint.Port);
var listener = new KTcpListener();

listener.StartListener(endpoint);
beacon.Start();

// Process one message
listener.AcceptTcpClient();
var request = listener.ReadRequest();

if (request.Command == "Ping")
    listener.WriteResponse(TcpMessage.Ok());
```

**Client:**
```csharp
var discovery = new DiscoveryListener();
discovery.HostDiscovered += (s, beacon) =>
{
    var endpoint = new IPEndPoint(IPAddress.Parse(beacon.Address), beacon.TcpPort);
    var response = KTcpClient.SendRequest(endpoint, TcpMessage.Request("Ping"));
    Console.WriteLine(response.IsOk ? "Pong!" : $"Error: {response.Command}");
};
discovery.Start();
```

---

## Network Discovery

`Discovery` is a static utility class for finding suitable network addresses and endpoints.

### FindServerAddress

```csharp
IPAddress address = Discovery.FindServerAddress(bool canUseLocalhost = true);
```

Returns the best available IP address for hosting a TCP server. Priority order:
1. LAN address (`192.168.x.x`)
2. Direct Ethernet connection (`169.254.x.x`)
3. Localhost (`127.0.0.1`) — only if `canUseLocalhost` is true

Organisation network addresses (`10.10.x.x`) are excluded. Returns `null` if no suitable address is found and localhost is not permitted.

### FindNextAvailableEndPoint

```csharp
IPEndPoint endpoint = Discovery.FindNextAvailableEndPoint();
```

Combines `FindServerAddress` with automatic port selection. The OS assigns a free port. Returns `null` if no suitable address exists. This is the recommended way to initialise a server endpoint.

### GetDiscoveryAddress

```csharp
IPAddress discoveryAddr = Discovery.GetDiscoveryAddress(bool multicast, IPAddress address);
```

Returns the appropriate broadcast or multicast address for UDP discovery given the server's IP address. Used internally by `DiscoveryBeacon`.

---

## UDP Discovery Beacon

`DiscoveryBeacon` periodically broadcasts a UDP packet advertising a server's name, address, and TCP port. Clients with a `DiscoveryListener` running on the same network will receive these broadcasts automatically.

### Constructor

```csharp
var beacon = new DiscoveryBeacon(
    name: "MY_SERVER",           // unique server name
    tcpAddress: "192.168.1.50",  // IP address of the TCP service
    tcpPort: 9000,               // port of the TCP service
    discoveryPort: 10001,        // UDP port (must match DiscoveryListener)
    intervalSeconds: 2           // how often to broadcast
);
```

### Usage

```csharp
beacon.Start();   // begin broadcasting
// ...
beacon.Stop();    // stop broadcasting
beacon.Dispose(); // or use in a using block
```

The beacon broadcasts immediately on `Start()`, then repeats at every interval. `Stop()` can be called multiple times safely.

### ServerBeacon payload

Each broadcast carries a `ServerBeacon` object:

| Property | Description |
|----------|-------------|
| `Name` | Unique server identifier |
| `Address` | IP address of the TCP service |
| `TcpPort` | Port of the TCP service |
| `Version` | Protocol version (increment if beacon format changes) |

---

## UDP Discovery Listener

`DiscoveryListener` receives UDP beacons and maintains a registry of known hosts. A watchdog timer monitors for hosts that have stopped beaconing and raises an event when they disappear.

### Constructor

```csharp
var listener = new DiscoveryListener(
    discoveryPort: 10001,              // must match DiscoveryBeacon
    disappearThresholdSeconds: 10,     // how long before a host is considered gone
    watchdogIntervalSeconds: 2         // how often the watchdog checks
);
```

The disappear threshold should be several multiples of the beacon interval to avoid false positives.

### Events

```csharp
// Raised on the listener thread when a new host is first seen
listener.HostDiscovered += (sender, beacon) =>
{
    Console.WriteLine($"Found: {beacon.Name} at {beacon.Address}:{beacon.TcpPort}");
};

// Raised on the watchdog thread when a host stops beaconing
listener.HostDisappeared += (sender, beacon) =>
{
    Console.WriteLine($"Lost: {beacon.Name}");
};
```

**Threading note:** `HostDiscovered` is raised on the listener thread; `HostDisappeared` is raised on the watchdog timer thread. Marshal to the UI thread if updating controls.

### KnownHosts

```csharp
IReadOnlyDictionary<string, DiscoveredHost> hosts = listener.KnownHosts;
```

Returns a thread-safe snapshot of currently known hosts, keyed by server name. Each `DiscoveredHost` exposes the `ServerBeacon` and a `LastSeen` timestamp.

### Usage

```csharp
listener.Start();
// ...
listener.Stop();    // clears KnownHosts registry
listener.Dispose(); // or use in a using block
```

---

## TCP Messaging — The TcpMessage Protocol

All TCP communication uses a symmetric envelope: every message in both directions — client requests and server responses — is a `TcpMessage`. The wire protocol uses length-prefixed UTF-8 JSON.

### Structure

```csharp
public class TcpMessage
{
    public int Code { get; set; }       // status code (meaningful on responses)
    public string Command { get; set; } // command name on requests; status text on responses
    public string Payload { get; set; } // always a JSON string; never null
}
```

`Payload` is always present. Use `"{}"` when there is no meaningful data.

### Factory Methods

Prefer factory methods over constructing `TcpMessage` directly.

**On the client — building requests:**

```csharp
// Command with no data
TcpMessage.Request("Ping")

// Command with a payload object
TcpMessage.Request("Connect", new ConnectPayload { EndPoint = "192.168.1.50:9000" })

// Command with a raw JSON payload string
TcpMessage.Request("Connect", "{\"EndPoint\":\"192.168.1.50:9000\"}")
```

**On the server — building responses:**

```csharp
TcpMessage.Ok()                          // 200, empty payload
TcpMessage.Ok(new { HostName = name })   // 200, object serialised as payload
TcpMessage.Ok("{\"HostName\":\"X\"}")    // 200, raw JSON payload
TcpMessage.BadRequest("Invalid data")    // 400
TcpMessage.NotFound(request.Command)     // 404
TcpMessage.Error("Unexpected failure")   // 500
```

### Reading a response on the client

```csharp
TcpMessage response = KTcpClient.SendRequest(endpoint, TcpMessage.Request("GetHostName"));

if (response.IsOk)
{
    var data = response.GetPayload<HostNamePayload>();
    Console.WriteLine(data.HostName);
}
else
{
    Console.WriteLine($"Error {response.Code}: {response.Command}");
}
```

`GetPayload<T>()` deserialises the JSON payload string into a typed object.

---

## TCP Server — KTcpListener

`KTcpListener` accepts incoming client connections and exchanges `TcpMessage` objects.

### Lifecycle

```csharp
var listener = new KTcpListener();
listener.StartListener(endpoint);   // begin listening

// ... in your message loop:
if (listener.Pending())
{
    listener.AcceptTcpClient();     // accept one connection
    var request = listener.ReadRequest();
    // handle request...
    listener.WriteResponse(TcpMessage.Ok());
    listener.CloseTcpClient();      // close this connection
}

listener.CloseListener();           // shut down
```

### Message handling pattern

```csharp
listener.AcceptTcpClient();
var request = listener.ReadRequest();

switch (request.Command)
{
    case "Ping":
        listener.WriteResponse(TcpMessage.Ok());
        break;

    case "GetHostName":
        listener.WriteResponse(TcpMessage.Ok(new { HostName = _hostName }));
        break;

    case "FileTransfer":
        // see Large File Transfer below
        break;

    default:
        listener.WriteResponse(TcpMessage.NotFound(request.Command));
        break;
}

listener.CloseTcpClient();
```

### Low-level methods

These are available for advanced use, including file transfer:

| Method | Description |
|--------|-------------|
| `ReadString()` | Reads a length-prefixed UTF-8 string |
| `WriteStringAsByteArray(string)` | Writes a length-prefixed UTF-8 string |
| `ReadBytes()` | Reads a length-prefixed byte array; sends int acknowledgement |

---

## TCP Client — KTcpClient

`KTcpClient` sends requests to a `KTcpListener` server.

### Simple request (recommended)

For normal command/response exchanges, use the static method. It handles connection, exchange, and teardown automatically:

```csharp
TcpMessage response = KTcpClient.SendRequest(endpoint, TcpMessage.Request("Ping"));
```

### Buffered send session

For large file transfers, open a persistent session:

```csharp
var client = new KTcpClient();
client.StartBufferedSend(endpoint);

// Write chunks
client.WriteBuffer(chunkBytes);
// ... repeat for all chunks

// Read final acknowledgement
TcpMessage result = client.ReadBufferedSendResponse();
client.EndBufferedSend();
```

| Method | Description |
|--------|-------------|
| `StartBufferedSend(IPEndPoint)` | Opens connection and initialises streams |
| `WriteBuffer(byte[])` | Sends a length-prefixed byte array |
| `ReadBufferedSendResponse()` | Reads a TcpMessage response |
| `EndBufferedSend()` | Closes streams and connection |

---

## Large File Transfer

For large files, `TcpMessage` handles negotiation while raw bytes flow on a separate connection. This avoids Base64 overhead and enables streaming.

### Protocol

```
Client → Server:  TcpMessage.Request("FileTransfer", new FileTransferPayload { ... })
Server:           Opens a secondary listener on a new port
Server → Client:  TcpMessage.Ok(new { Port = secondaryPort })
Client:           Opens buffered send session to secondary port
Client → Server:  [raw byte chunks via WriteBuffer()]
Server → Client:  TcpMessage.Ok()   ← all bytes received
Client:           EndBufferedSend()
```

### FileTransfer DTO

```csharp
public class FileTransferPayload
{
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public int ChunkSize { get; set; }
}
```

### Server handler sketch

```csharp
case "FileTransfer":
    var transfer = request.GetPayload<FileTransferPayload>();

    // Get a free port and start secondary listener
    var secondaryEndpoint = new IPEndPoint(localAddress, 0);
    var secondaryListener = new KTcpListener();
    secondaryListener.StartListener(secondaryEndpoint);
    int assignedPort = ((IPEndPoint)secondaryListener.LocalEndpoint).Port;

    // Tell the client which port to connect to
    listener.WriteResponse(TcpMessage.Ok(new { Port = assignedPort }));

    // Accept the binary connection and receive chunks
    secondaryListener.AcceptTcpClient();
    long bytesReceived = 0;
    while (bytesReceived < transfer.FileSize)
    {
        byte[] chunk = secondaryListener.ReadBytes();
        // write chunk to file...
        bytesReceived += chunk.Length;
    }

    secondaryListener.WriteResponse(TcpMessage.Ok());
    secondaryListener.CloseTcpClient();
    secondaryListener.CloseListener();
    break;
```

### Client call site sketch

```csharp
var payload = new FileTransferPayload
{
    FileName = Path.GetFileName(filePath),
    FileSize = new FileInfo(filePath).Length,
    ChunkSize = 65536
};

var response = KTcpClient.SendRequest(endpoint, TcpMessage.Request("FileTransfer", payload));
if (!response.IsOk) return;

int transferPort = response.GetPayload<TransferPortPayload>().Port;
var transferEndpoint = new IPEndPoint(endpoint.Address, transferPort);

var client = new KTcpClient();
client.StartBufferedSend(transferEndpoint);

byte[] fileBytes = File.ReadAllBytes(filePath);
int chunkSize = payload.ChunkSize;
for (int offset = 0; offset < fileBytes.Length; offset += chunkSize)
{
    int count = Math.Min(chunkSize, fileBytes.Length - offset);
    byte[] chunk = new byte[count];
    Array.Copy(fileBytes, offset, chunk, 0, count);
    client.WriteBuffer(chunk);
}

TcpMessage result = client.ReadBufferedSendResponse();
client.EndBufferedSend();
```

---

## Complete End-to-End Example

```csharp
// --- SERVER ---
var endpoint = Discovery.FindNextAvailableEndPoint();
var beacon   = new DiscoveryBeacon("MY_SERVER", endpoint.Address.ToString(), endpoint.Port);
var listener = new KTcpListener();

listener.StartListener(endpoint);
beacon.Start();

while (running)
{
    if (!listener.Pending()) { Thread.Sleep(10); continue; }

    listener.AcceptTcpClient();
    var request = listener.ReadRequest();

    switch (request.Command)
    {
        case "Ping":
            listener.WriteResponse(TcpMessage.Ok());
            break;
        case "GetHostName":
            listener.WriteResponse(TcpMessage.Ok(new { HostName = Environment.MachineName }));
            break;
        default:
            listener.WriteResponse(TcpMessage.NotFound(request.Command));
            break;
    }

    listener.CloseTcpClient();
}

beacon.Stop();
listener.CloseListener();


// --- CLIENT ---
IPEndPoint serverEndpoint = null;

var discovery = new DiscoveryListener();
discovery.HostDiscovered += (s, b) =>
{
    serverEndpoint = new IPEndPoint(IPAddress.Parse(b.Address), b.TcpPort);
};
discovery.Start();

// Wait until server is found, then send a Ping
while (serverEndpoint == null) Thread.Sleep(100);

var response = KTcpClient.SendRequest(serverEndpoint, TcpMessage.Request("Ping"));
Console.WriteLine(response.IsOk ? "Server is alive" : $"Error: {response.Command}");

// Get the hostname
response = KTcpClient.SendRequest(serverEndpoint, TcpMessage.Request("GetHostName"));
if (response.IsOk)
{
    var data = response.GetPayload<HostNamePayload>();
    Console.WriteLine($"Server hostname: {data.HostName}");
}

discovery.Stop();
```

---

## Data Transfer Objects (DTOs)

Define one DTO class per command that carries data. Place all DTOs in a single file (`TcpPayloads.cs`) under your application namespace.

```csharp
namespace MyApp.Tcp
{
    public class ConnectPayload
    {
        public string EndPoint { get; set; }
    }

    public class HostNamePayload
    {
        public string HostName { get; set; }
    }

    public class FileTransferPayload
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public int ChunkSize { get; set; }
    }

    public class TransferPortPayload
    {
        public int Port { get; set; }
    }
}
```

Define one DTO per command even if two commands share the same shape — this documents intent, isolates future changes, and lets the compiler catch mismatches.

---

## Status Code Reference

| Code | Meaning | Typical use |
|------|---------|-------------|
| 200 | OK | Command succeeded |
| 400 | Bad Request | Command recognised but payload was malformed or missing |
| 401/403 | Unauthorised / Forbidden | Command valid but not permitted in current state |
| 404 | Not Found | Command name not recognised |
| 500 | Server Error | Unexpected exception on the server side |

On outgoing client requests, `Code` is left 0. It is only meaningful on responses. Use `response.IsOk` to check for success rather than comparing the code directly.
