# KTcp Protocol

All TCP communication in this system uses a symmetric, envelope-based protocol. Every message — whether a client request or a server response — has the same structure.

---

## The TcpMessage Envelope

```csharp
public class TcpMessage
{
    public int Code { get; set; }       // Status code (see below)
    public string Command { get; set; } // Command name on requests; human-readable status on responses
    public string Payload { get; set; } // Always a JSON string; "{}" when empty, never null
}
```

### Status Codes

Follows HTTP conventions:

| Code | Meaning |
|------|---------|
| 200 | OK — command succeeded |
| 400 | Bad Request — command recognised but payload was malformed or invalid |
| 401/403 | Unauthorised / Forbidden — command valid but not permitted in current state |
| 404 | Not Found — command not recognised |
| 500 | Server Error — unexpected failure on the server side |

On outgoing client requests, `Code` is left 0. It is meaningful only on responses.

---

## Factory Methods

Prefer these over constructing `TcpMessage` directly.

```csharp
// Client: sending a command with no payload
TcpMessage.Request("Disconnect")

// Server: success with no data to return
TcpMessage.Ok()

// Server: success with data
TcpMessage.Ok(new { hostname = _hostName })
TcpMessage.Ok(myPayloadObject)

// Server: error responses
TcpMessage.BadRequest("Endpoint could not be parsed")
TcpMessage.NotFound(request.Command)
TcpMessage.Error("Unexpected failure")
```

---

## The DTO Pattern

Data accompanying a command — in either direction — is represented as a typed DTO serialised to JSON in the `Payload` field. Define one DTO class per command, even if two commands share the same shape.

```csharp
// TcpPayloads.cs — in your application namespace
namespace MyApp.Tcp
{
    public class ConnectPayload
    {
        public string EndPoint { get; set; }
    }

    public class FileTransferPayload
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public int ChunkSize { get; set; }
    }
}
```

**Deserialising a payload on the client:**

```csharp
TcpMessage response = _client.SendRequest(_remoteEndPoint, TcpMessage.Request("GetHostName"));
if (response.IsOk)
{
    var data = response.GetPayload<HostNamePayload>();
    string hostName = data.HostName;
}
```

**Namespaces:** `TcpMessage` lives in the reusable `KTcp` library. DTOs are application-specific and live in the application project under its own namespace.

---

## Wire Protocol

All messages are transmitted as length-prefixed byte arrays using `BinaryWriter`/`BinaryReader`. `TcpMessage` serialises to JSON before hitting the wire and deserialises from JSON on receipt. Application code never deals with framing directly.

---

## A Complete Exchange

**Simple command (no payload either direction):**

```
Client → Server:  { Command: "Disconnect", Payload: "{}" }
Server → Client:  { Code: 200, Payload: "{}" }
```

**Command returning data:**

```
Client → Server:  { Command: "GetHostName", Payload: "{}" }
Server → Client:  { Code: 200, Payload: "{\"HostName\": \"MyMachine\"}" }
```

**Server switch block:**

```csharp
var request = _listener.ReadRequest();

switch (request.Command)
{
    case "Disconnect":
        _listener.WriteResponse(TcpMessage.Ok());
        _remoteConnected = false;
        break;

    case "GetHostName":
        _listener.WriteResponse(TcpMessage.Ok(new { HostName = _hostName }));
        break;

    case "Ping":
        _listener.WriteResponse(_remoteConnected
            ? TcpMessage.Ok()
            : TcpMessage.BadRequest("Not connected"));
        break;

    default:
        _listener.WriteResponse(TcpMessage.NotFound(request.Command));
        break;
}
```

---

## Large File Transfers

For large files, `TcpMessage` coordinates the transfer while raw bytes flow outside the envelope.

```
Client → Server:  TcpMessage.Request("FileTransfer")
                  payload: { FileName, FileSize, ChunkSize }

Server → Client:  TcpMessage.Ok()        ← ready, begin transfer

Client → Server:  [raw binary chunks]

Server → Client:  TcpMessage.Ok()        ← transfer complete
```

`TcpMessage` is the *control plane*. Raw binary is the *data plane*.

For small text files, packaging filename and content directly in the payload is appropriate.
