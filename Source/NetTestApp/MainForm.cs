using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KLib.Net;

namespace NetTestApp
{
    public partial class MainForm : Form
    {
        // ── Protocol constants ────────────────────────────────────────────────
        private const int DiscoveryPort   = 10001;
        private const string ServerName   = "KLib Test Server";

        // ── Server state ──────────────────────────────────────────────────────
        private KTcpListener    _listener;
        private DiscoveryBeacon _beacon;
        private IPEndPoint      _serverEndPoint;
        private Thread          _serverThread;
        private volatile bool   _serverRunning;

        // ── Client state ──────────────────────────────────────────────────────
        private DiscoveryListener _discoveryListener;
        private IPEndPoint        _discoveredEndPoint;   // set when beacon arrives

        // ─────────────────────────────────────────────────────────────────────

        public MainForm()
        {
            InitializeComponent();

            btnStartTcp.Click    += BtnStartTcp_Click;
            btnStopTcp.Click     += BtnStopTcp_Click;
            btnStartBeacon.Click += BtnStartBeacon_Click;
            btnStopBeacon.Click  += BtnStopBeacon_Click;
            btnPing.Click        += BtnPing_Click;
            btnSendFile.Click    += BtnSendFile_Click;
            btnClearLog.Click    += (s, e) => txtLog.Clear();

            Load    += MainForm_Load;
            FormClosing += MainForm_FormClosing;
        }

        // ── Form lifecycle ────────────────────────────────────────────────────

        private void MainForm_Load(object sender, EventArgs e)
        {
            _discoveryListener = new DiscoveryListener(
                discoveryPort:            DiscoveryPort,
                disappearThresholdSeconds: 10,
                watchdogIntervalSeconds:   2);

            _discoveryListener.HostDiscovered  += OnHostDiscovered;
            _discoveryListener.HostDisappeared += OnHostDisappeared;
            _discoveryListener.Start();

            Log("[client] Discovery listener started (port {0})", DiscoveryPort);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
            StopBeacon();
            _discoveryListener?.Stop();
        }

        // ── Server — TCP listener ─────────────────────────────────────────────

        private void BtnStartTcp_Click(object sender, EventArgs e)
        {
            try
            {
                _serverEndPoint = Discovery.FindNextAvailableEndPoint();
                if (_serverEndPoint == null)
                {
                    MessageBox.Show("Could not find a suitable network address.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _listener = new KTcpListener();
                _listener.StartListener(_serverEndPoint);

                _serverRunning = true;
                _serverThread  = new Thread(ServerPollLoop)
                {
                    IsBackground = true,
                    Name         = "ServerPoll"
                };
                _serverThread.Start();

                SetTcpStatus(running: true);
                lblTcpInfo.Text = $"{_serverEndPoint.Address}:{_serverEndPoint.Port}";
                btnStartBeacon.Enabled = true;

                Log("[server] TCP listener started on {0}:{1}",
                    _serverEndPoint.Address, _serverEndPoint.Port);
            }
            catch (Exception ex)
            {
                Log("[server] Failed to start TCP listener: {0}", ex.Message);
            }
        }

        private void BtnStopTcp_Click(object sender, EventArgs e)
        {
            StopBeacon();
            StopServer();
        }

        private void StopServer()
        {
            if (!_serverRunning) return;

            _serverRunning = false;
            try { _listener?.CloseListener(); } catch { }
            _serverThread?.Join(500);
            _serverThread = null;
            _listener = null;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SetTcpStatus(running: false)));
            }
            else
            {
                SetTcpStatus(running: false);
            }

            Log("[server] TCP listener stopped");
        }

        private void SetTcpStatus(bool running)
        {
            lblTcpStatus.ForeColor  = running ? System.Drawing.Color.Green : System.Drawing.Color.Gray;
            btnStartTcp.Enabled     = !running;
            btnStopTcp.Enabled      = running;
            if (!running)
            {
                btnStartBeacon.Enabled = false;
                lblTcpInfo.Text        = "Idle";
            }
        }

        // ── Server — UDP beacon ───────────────────────────────────────────────

        private void BtnStartBeacon_Click(object sender, EventArgs e)
        {
            if (_serverEndPoint == null) return;
            try
            {
                _beacon = new DiscoveryBeacon(
                    name:            ServerName,
                    tcpAddress:      _serverEndPoint.Address.ToString(),
                    tcpPort:         _serverEndPoint.Port,
                    discoveryPort:   DiscoveryPort,
                    intervalSeconds: 2);
                _beacon.Start();

                SetBeaconStatus(running: true);
                lblBeaconInfo.Text = $"Broadcasting → :{DiscoveryPort}";
                Log("[server] Beacon started: name={0} tcp={1}:{2}",
                    ServerName, _serverEndPoint.Address, _serverEndPoint.Port);
            }
            catch (Exception ex)
            {
                Log("[server] Failed to start beacon: {0}", ex.Message);
            }
        }

        private void BtnStopBeacon_Click(object sender, EventArgs e) => StopBeacon();

        private void StopBeacon()
        {
            if (_beacon == null) return;
            try { _beacon.Stop(); _beacon.Dispose(); } catch { }
            _beacon = null;

            if (InvokeRequired)
                BeginInvoke(new Action(() => SetBeaconStatus(running: false)));
            else
                SetBeaconStatus(running: false);

            Log("[server] Beacon stopped");
        }

        private void SetBeaconStatus(bool running)
        {
            lblBeaconStatus.ForeColor  = running ? System.Drawing.Color.Green : System.Drawing.Color.Gray;
            btnStartBeacon.Enabled     = !running && _serverRunning;
            btnStopBeacon.Enabled      = running;
            if (!running) lblBeaconInfo.Text = "Idle";
        }

        // ── Server — request dispatch loop ────────────────────────────────────

        private void ServerPollLoop()
        {
            while (_serverRunning)
            {
                try
                {
                    if (_listener != null && _serverRunning && _listener.Pending())
                    {
                        _listener.AcceptTcpClient();
                        try   { HandleRequest(); }
                        catch (Exception ex) { Log("[server] Handler error: {0}", ex.Message); }
                        finally { try { _listener.CloseTcpClient(); } catch { } }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                catch (Exception)
                {
                    if (!_serverRunning) break;
                    Thread.Sleep(50);
                }
            }
        }

        private void HandleRequest()
        {
            var request = _listener.ReadRequest();
            Log("[server] ← {0}", request.Serialize());

            switch (request.Command)
            {
                case "Ping":
                    var pong = TcpMessage.Ok();
                    _listener.WriteResponse(pong);
                    Log("[server] → {0}", pong.Serialize());
                    break;

                case "SendFile":
                    HandleSendFile(request);
                    break;

                default:
                    var notFound = TcpMessage.NotFound(request.Command);
                    _listener.WriteResponse(notFound);
                    Log("[server] → {0}", notFound.Serialize());
                    break;
            }
        }

        // File transfer — server side:
        //   1. Approve with TcpMessage.Ok()
        //   2. ReadBytes() — receives file bytes; internally writes int=1 ack
        //   3. Save file
        //   4. WriteResponse(TcpMessage.Ok()) — final result
        private void HandleSendFile(TcpMessage request)
        {
            var info = request.GetPayload<FileTransferInfo>();
            Log("[server] Incoming file: {0} ({1:N0} bytes)", info.FileName, info.FileSize);

            // Step 1: approve
            var ok = TcpMessage.Ok();
            _listener.WriteResponse(ok);
            Log("[server] → {0}", ok.Serialize());

            // Step 2: receive bytes (KTcpListener.ReadBytes writes int=1 ack after reading)
            byte[] fileBytes = _listener.ReadBytes();
            Log("[server] ← [binary {0:N0} bytes]", fileBytes.Length);

            // Step 3: save
            string dir  = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KLib_Received");
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, info.FileName);
            File.WriteAllBytes(path, fileBytes);
            Log("[server] Saved → {0}", path);

            // Step 4: final response
            var final = TcpMessage.Ok(new { Message = $"Saved: {path}" });
            _listener.WriteResponse(final);
            Log("[server] → {0}", final.Serialize());
        }

        // ── Client — discovery ────────────────────────────────────────────────

        private void OnHostDiscovered(object sender, ServerBeacon beacon)
        {
            _discoveredEndPoint = new IPEndPoint(
                IPAddress.Parse(beacon.Address), beacon.TcpPort);

            BeginInvoke(new Action(() =>
            {
                lblSvrName.Text         = $"Name:   {beacon.Name}";
                lblSvrIP.Text           = $"IP:       {beacon.Address}";
                lblSvrPort.Text         = $"Port:    {beacon.TcpPort}";
                lblWatchdogWarn.Visible = false;
                btnPing.Enabled         = true;
                btnSendFile.Enabled     = true;
            }));

            Log("[client] Server discovered: {0} @ {1}:{2}",
                beacon.Name, beacon.Address, beacon.TcpPort);
        }

        private void OnHostDisappeared(object sender, ServerBeacon beacon)
        {
            BeginInvoke(new Action(() =>
            {
                lblWatchdogWarn.Visible = true;
                btnPing.Enabled         = false;
                btnSendFile.Enabled     = false;
            }));

            Log("[client] ⚠ Beacon lost: {0}", beacon.Name);
        }

        // ── Client — Ping ─────────────────────────────────────────────────────

        private async void BtnPing_Click(object sender, EventArgs e)
        {
            if (_discoveredEndPoint == null) return;

            btnPing.Enabled = false;
            try
            {
                var request = TcpMessage.Request("Ping");
                Log("[client] → {0}", request.Serialize());

                var response = await Task.Run(
                    () => KTcpClient.SendRequest(_discoveredEndPoint, request));

                Log("[client] ← {0}", response.Serialize());
            }
            catch (Exception ex)
            {
                Log("[client] Ping error: {0}", ex.Message);
            }
            finally
            {
                btnPing.Enabled = _discoveredEndPoint != null;
            }
        }

        // ── Client — Send File ────────────────────────────────────────────────

        private async void BtnSendFile_Click(object sender, EventArgs e)
        {
            if (_discoveredEndPoint == null) return;

            using var ofd = new OpenFileDialog { Title = "Choose file to send" };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string filePath = ofd.FileName;
            btnSendFile.Enabled = false;
            try
            {
                await SendFileAsync(filePath);
            }
            catch (Exception ex)
            {
                Log("[client] SendFile error: {0}", ex.Message);
            }
            finally
            {
                btnSendFile.Enabled = _discoveredEndPoint != null;
            }
        }

        // File transfer — client side (raw TcpClient so we can read the ack):
        //
        //   Wire protocol (all on one connection):
        //   C→S  [int32 len][negotiate TcpMessage JSON]      (matches KTcpListener.ReadRequest)
        //   S→C  [int32 len][ok TcpMessage JSON]             (matches KTcpListener.WriteResponse)
        //   C→S  [int32 fileLen][file bytes]                 (matches KTcpListener.ReadBytes prefix)
        //   S→C  [int32=1]                                   (ReadBytes ack — must be consumed)
        //   S→C  [int32 len][final TcpMessage JSON]          (matches KTcpListener.WriteResponse)
        private async Task SendFileAsync(string filePath)
        {
            byte[] fileBytes = await Task.Run(() => File.ReadAllBytes(filePath));
            string fileName  = Path.GetFileName(filePath);

            Log("[client] Sending '{0}' ({1:N0} bytes)", fileName, fileBytes.Length);

            var result = await Task.Run<TcpMessage>(() =>
            {
                using var tcp = new TcpClient();
                tcp.Connect(_discoveredEndPoint.Address, _discoveredEndPoint.Port);
                using var stream = tcp.GetStream();
                using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
                using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

                // 1. Send negotiate TcpMessage
                var negotiate = TcpMessage.Request("SendFile",
                    new FileTransferInfo { FileName = fileName, FileSize = fileBytes.Length });
                byte[] negotiateBytes = Encoding.UTF8.GetBytes(negotiate.Serialize());
                writer.Write(negotiateBytes.Length);
                writer.Write(negotiateBytes);
                writer.Flush();
                Log("[client] → {0}", negotiate.Serialize());

                // 2. Read server approval
                int okLen    = reader.ReadInt32();
                var okBytes  = reader.ReadBytes(okLen);
                var okResp   = TcpMessage.Deserialize(Encoding.UTF8.GetString(okBytes));
                Log("[client] ← {0}", okResp.Serialize());
                if (!okResp.IsOk)
                    return okResp;

                // 3. Send file bytes (wire format matching KTcpListener.ReadBytes)
                writer.Write(fileBytes.Length);
                writer.Write(fileBytes);
                writer.Flush();
                Log("[client] → [binary {0:N0} bytes of '{1}']", fileBytes.Length, fileName);

                // 4. Read the int=1 ack that KTcpListener.ReadBytes sends automatically
                reader.ReadInt32();

                // 5. Read final TcpMessage response
                int finalLen   = reader.ReadInt32();
                var finalBytes = reader.ReadBytes(finalLen);
                var finalResp  = TcpMessage.Deserialize(Encoding.UTF8.GetString(finalBytes));
                Log("[client] ← {0}", finalResp.Serialize());

                return finalResp;
            });

            if (!result.IsOk)
                MessageBox.Show($"Transfer failed: {result.Command}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ── Logging ───────────────────────────────────────────────────────────

        private void Log(string format, params object[] args)
        {
            string msg = $"[{DateTime.Now:HH:mm:ss.fff}]  {string.Format(format, args)}";

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => AppendLog(msg)));
            }
            else
            {
                AppendLog(msg);
            }
        }

        private void AppendLog(string msg)
        {
            txtLog.AppendText(msg + "\n");
            txtLog.ScrollToCaret();
        }

        // ── Private types ─────────────────────────────────────────────────────

        private class FileTransferInfo
        {
            public string FileName { get; set; }
            public int    FileSize { get; set; }
        }
    }
}
