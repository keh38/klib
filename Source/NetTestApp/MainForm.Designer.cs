namespace NetTestApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // ---- Server side ----
        private System.Windows.Forms.GroupBox grpServer;
        private System.Windows.Forms.Label lblTcpSection;
        private System.Windows.Forms.Button btnStartTcp;
        private System.Windows.Forms.Button btnStopTcp;
        private System.Windows.Forms.Label lblTcpStatus;
        private System.Windows.Forms.Label lblTcpInfo;
        private System.Windows.Forms.Label lblBeaconSection;
        private System.Windows.Forms.Button btnStartBeacon;
        private System.Windows.Forms.Button btnStopBeacon;
        private System.Windows.Forms.Label lblBeaconStatus;
        private System.Windows.Forms.Label lblBeaconInfo;

        // ---- Client side ----
        private System.Windows.Forms.GroupBox grpClient;
        private System.Windows.Forms.Label lblDiscoveredTitle;
        private System.Windows.Forms.Label lblSvrName;
        private System.Windows.Forms.Label lblSvrIP;
        private System.Windows.Forms.Label lblSvrPort;
        private System.Windows.Forms.Label lblWatchdogWarn;
        private System.Windows.Forms.Button btnPing;
        private System.Windows.Forms.Button btnSendFile;

        // ---- Log ----
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button btnClearLog;

        // ---- Layout ----
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.TableLayoutPanel tableTop;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── Server group ──────────────────────────────────────────────────
            grpServer = new System.Windows.Forms.GroupBox();
            grpServer.Text = "Server";
            grpServer.Font = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Bold);
            grpServer.Dock = System.Windows.Forms.DockStyle.Fill;
            grpServer.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);

            lblTcpSection = MakeLabel("TCP Listener", 10, 28, bold: true, font: 9f);
            btnStartTcp   = MakeButton("Start", 10, 50, 80);
            btnStopTcp    = MakeButton("Stop",  96, 50, 80);
            lblTcpStatus  = MakeStatusDot(182, 58);
            lblTcpInfo    = MakeLabel("Idle", 200, 58, font: 8.5f, fg: System.Drawing.Color.Gray);

            lblBeaconSection = MakeLabel("UDP Beacon", 10, 95, bold: true, font: 9f);
            btnStartBeacon   = MakeButton("Start", 10, 117, 80);
            btnStopBeacon    = MakeButton("Stop",  96, 117, 80);
            lblBeaconStatus  = MakeStatusDot(182, 125);
            lblBeaconInfo    = MakeLabel("Idle", 200, 125, font: 8.5f, fg: System.Drawing.Color.Gray);

            btnStopTcp.Enabled    = false;
            btnStopBeacon.Enabled = false;
            btnStartBeacon.Enabled = false; // need TCP started first

            grpServer.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTcpSection, btnStartTcp, btnStopTcp, lblTcpStatus, lblTcpInfo,
                lblBeaconSection, btnStartBeacon, btnStopBeacon, lblBeaconStatus, lblBeaconInfo
            });

            // ── Client group ──────────────────────────────────────────────────
            grpClient = new System.Windows.Forms.GroupBox();
            grpClient.Text = "Client";
            grpClient.Font = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Bold);
            grpClient.Dock = System.Windows.Forms.DockStyle.Fill;
            grpClient.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);

            lblDiscoveredTitle = MakeLabel("Discovered Server", 10, 28, bold: true, font: 9f);

            lblSvrName = MakeLabel("Name:   (scanning…)", 10, 52, font: 8.5f);
            lblSvrName.AutoSize = false;
            lblSvrName.Size = new System.Drawing.Size(280, 18);

            lblSvrIP = MakeLabel("IP:       —", 10, 72, font: 8.5f);
            lblSvrIP.AutoSize = false;
            lblSvrIP.Size = new System.Drawing.Size(280, 18);

            lblSvrPort = MakeLabel("Port:    —", 10, 92, font: 8.5f);
            lblSvrPort.AutoSize = false;
            lblSvrPort.Size = new System.Drawing.Size(280, 18);

            lblWatchdogWarn = new System.Windows.Forms.Label();
            lblWatchdogWarn.Text = "⚠  Beacon lost — server may be offline";
            lblWatchdogWarn.Location = new System.Drawing.Point(10, 116);
            lblWatchdogWarn.Size = new System.Drawing.Size(300, 18);
            lblWatchdogWarn.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold);
            lblWatchdogWarn.ForeColor = System.Drawing.Color.OrangeRed;
            lblWatchdogWarn.Visible = false;

            btnPing     = MakeButton("Ping",      10, 145, 90);
            btnSendFile = MakeButton("Send File", 106, 145, 100);
            btnPing.Enabled     = false;
            btnSendFile.Enabled = false;
            btnPing.Font     = new System.Drawing.Font("Segoe UI", 8.5f);
            btnSendFile.Font = new System.Drawing.Font("Segoe UI", 8.5f);

            grpClient.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblDiscoveredTitle,
                lblSvrName, lblSvrIP, lblSvrPort,
                lblWatchdogWarn,
                btnPing, btnSendFile
            });

            // ── Log group ─────────────────────────────────────────────────────
            grpLog = new System.Windows.Forms.GroupBox();
            grpLog.Text = "Message Log";
            grpLog.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            grpLog.Dock = System.Windows.Forms.DockStyle.Fill;
            grpLog.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);

            txtLog = new System.Windows.Forms.RichTextBox();
            txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            txtLog.ReadOnly = true;
            txtLog.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            txtLog.ForeColor = System.Drawing.Color.FromArgb(200, 220, 200);
            txtLog.Font = new System.Drawing.Font("Consolas", 8.5f);
            txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            txtLog.WordWrap = false;

            btnClearLog = new System.Windows.Forms.Button();
            btnClearLog.Text = "Clear";
            btnClearLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnClearLog.Height = 26;
            btnClearLog.Font = new System.Drawing.Font("Segoe UI", 8f);
            btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.System;

            grpLog.Controls.Add(txtLog);
            grpLog.Controls.Add(btnClearLog);

            // ── Top TableLayoutPanel (server | client) ────────────────────────
            tableTop = new System.Windows.Forms.TableLayoutPanel();
            tableTop.Dock = System.Windows.Forms.DockStyle.Fill;
            tableTop.ColumnCount = 2;
            tableTop.RowCount = 1;
            tableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
            tableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
            tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
            tableTop.Controls.Add(grpServer, 0, 0);
            tableTop.Controls.Add(grpClient, 1, 0);

            // ── Main TableLayoutPanel (top | log) ─────────────────────────────
            tableMain = new System.Windows.Forms.TableLayoutPanel();
            tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableMain.ColumnCount = 1;
            tableMain.RowCount = 2;
            tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200f));
            tableMain.Controls.Add(tableTop, 0, 0);
            tableMain.Controls.Add(grpLog,   0, 1);

            // ── Form ──────────────────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(860, 560);
            MinimumSize = new System.Drawing.Size(700, 480);
            Text = "KLib.Net Test App";
            Font = new System.Drawing.Font("Segoe UI", 9f);
            Controls.Add(tableMain);
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static System.Windows.Forms.Button MakeButton(string text, int x, int y, int w)
        {
            return new System.Windows.Forms.Button
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(w, 28),
                FlatStyle = System.Windows.Forms.FlatStyle.System
            };
        }

        private static System.Windows.Forms.Label MakeLabel(
            string text, int x, int y,
            bool bold = false,
            float font = 9f,
            System.Drawing.Color? fg = null)
        {
            var lbl = new System.Windows.Forms.Label
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", font,
                    bold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular)
            };
            if (fg.HasValue) lbl.ForeColor = fg.Value;
            return lbl;
        }

        private static System.Windows.Forms.Label MakeStatusDot(int x, int y)
        {
            return new System.Windows.Forms.Label
            {
                Text = "●",
                Location = new System.Drawing.Point(x, y),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10f),
                ForeColor = System.Drawing.Color.Gray
            };
        }
    }
}
