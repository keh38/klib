# KTcp Protocol Design

## Background

The original TCP system had an inconsistent response structure. Simple commands returned a 4-byte integer acknowledgement; data-returning commands returned a string; some commands returned arrays. The client and server had to implicitly agree on what each response would look like depending on the command, making the system error-prone, hard to maintain, and difficult to extend.

This document describes the replacement: a symmetric, envelope-based protocol where every exchange — in both directions — has the same shape.

---

## Core Principle

Every message, whether a request from the client or a response from the server, is a `TcpMessage`. Both sides always send and receive the same structure. The implicit per-command agreement about response type is replaced by a single explicit contract that applies universally.

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

| Code | Meaning | When to use |
|------|---------|-------------|
| 200 | OK | Command succeeded |
| 400 | Bad Request | Command recognised but payload was malformed or invalid |
| 401/403 | Unauthorised / Forbidden | Command valid but not permitted in current state (e.g. command arrived before Connect) |
| 404 | Not Found | Command not recognised — maps to the `default` case in the server switch |
| 500 | Server Error | Unexpected failure on the server side |

**Distinguishing 400 from 404:** A 404 almost always indicates a programming error — a typo in a command name or a version mismatch between client and server. A 400 means the command routing was fine but the accompanying data was wrong. The code tells you where to look for the bug.

On outgoing client requests, `Code` is left 0. It is meaningful only on responses.

---

## Factory Methods

`TcpMessage` provides static factory methods to keep call sites concise and consistent. Prefer these over constructing `TcpMessage` directly.

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

When a command carries data — either in a request or a response — that data is represented as a typed Data Transfer Object (DTO) serialised to JSON and stored in the `Payload` field.

Define one DTO class per command, even if two commands happen to have the same shape. This documents intent, isolates future changes, and lets the compiler catch mismatches.

```csharp
// TcpPayloads.cs — application-specific, in your application namespace
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

**Note on namespaces:** `TcpMessage` itself lives in the reusable `KTcp` library and has no application-specific knowledge. DTOs are application-specific and live in the application project under its own namespace. A single `using MyApp.Tcp` gives you all the DTOs for a given application.

---

## Wire Protocol

The underlying wire protocol is unchanged: all messages are transmitted as length-prefixed byte arrays using `BinaryWriter`/`BinaryReader`. `TcpMessage` serialises to JSON before hitting the wire and deserialises from JSON on receipt. Application code never deals with framing directly.

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

For large files (tens of MB and above), forcing the data through the `TcpMessage` envelope is inappropriate — Base64 encoding adds ~33% overhead and prevents streaming. Instead, `TcpMessage` is used to *coordinate* the transfer while the raw bytes flow outside the envelope.

**Pattern: negotiate with TcpMessage, transfer raw**

```
Client → Server:  TcpMessage.Request("FileTransfer")
                  payload: { FileName, FileSize, ChunkSize }

Server → Client:  TcpMessage.Ok()        ← ready, begin transfer

Client → Server:  [raw binary chunks]

Server → Client:  TcpMessage.Ok()        ← all bytes received, transfer complete
```

`TcpMessage` is the *control plane*. Raw binary is the *data plane*. Both sides know to switch modes because a `FileTransfer` command arrived — which is exactly what the protocol is designed to make explicit.

For small text files, packaging the filename and content directly in the payload is perfectly appropriate.

---

## Migration Guide

When migrating an existing codebase to this protocol:

1. **Add `TcpMessage.cs`** to the shared library. Nothing breaks — no existing code is touched.
2. **Add `ReadRequest()` and `WriteResponse()`** to the server-side listener, and `SendRequest()` to `KTcpClient`. Again, additive only.
3. **Migrate one command at a time**, updating both the server handler and the client call site together. Old methods can remain until all commands are ported.
4. **Remove old methods** once nothing references them.

Start with the simplest command (no payload either direction) to prove the round-trip works, then move to commands that return data.
