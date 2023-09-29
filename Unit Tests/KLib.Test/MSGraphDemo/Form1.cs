using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using KLib.MSGraph;
using KLib.MSGraph.Data;

namespace MSGraphDemo
{
    public partial class Form1 : Form
    {
        GraphClient _graphClient = null;

        public Form1()
        {
            InitializeComponent();
            opsPanel.Enabled = false;
            _graphClient = new GraphClient();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            bool connected = await _graphClient.AcquireAuthorizationTokenAsync();

            if (connected)
            {
                signInButton.Visible = false;
                opsPanel.Enabled = true;
                tokenBox.Text = _graphClient.GetToken();
                signInLabel.Text = "";

                try
                {
                    var ui = await _graphClient.GetUserInfo();
                    if (ui != null)
                    {
                        signInLabel.Text = $"Signed in as {ui.userPrincipalName}";
                    }
                    else
                    {
                        signInLabel.Text = "Null sign-in user info";
                    }
                }
                catch (Exception ex)
                {
                    resultBox.Text = $"Error retrieving user info:{System.Environment.NewLine}{ex.Message}";
                }
            }
        }

        private async void signInButton_Click(object sender, EventArgs e)
        {
            UserInfo ui = null;

            bool connected = await _graphClient.SignInUserAsync();

            if (connected)
            {
                signInButton.Visible = false;
                opsPanel.Enabled = true;
                ui = await _graphClient.GetUserInfo();
                if (ui != null)
                {
                    signInLabel.Text = $"Signed in as {ui.userPrincipalName}";
                }
                else
                {
                    signInLabel.Text = "Error retrieving sign-in user info";
                }
            }
        }

        private async void signOutButton_Click(object sender, EventArgs e)
        {
            bool signedOut = await _graphClient.SignOutUserAsync();

            if (signedOut)
            {
                opsPanel.Enabled = false;
                signInButton.Visible = true;
                signInLabel.Text = "";
            }
            else
            {
                resultBox.Text = "Error signing out user";
            }
        }

        private async void rootButton_Click(object sender, EventArgs e)
        {
            //            var foundBaseFolder = await _graphClient.SetBaseFolder("KEH_shared");
            try
            {
                var foundBaseFolder = await _graphClient.SetBaseFolder("NewFolder");
                if (foundBaseFolder)
                {
                    var names = await _graphClient.GetItemNames("");
                    foreach (var n in names)
                    {
                        resultBox.Text += n + System.Environment.NewLine;
                    }
                }
                else
                {
                    resultBox.Text = "Base folder not found";
                }
            }
            catch (Exception ex)
            {
                resultBox.AppendText($"Error setting base folder:{System.Environment.NewLine}{ex.Message}{System.Environment.NewLine}");
            }
        }

        private async void testButton_Click(object sender, EventArgs e)
        {
            var folderName = "Test";
            try
            {
                var folderExists = await _graphClient.FolderExists(folderName);
                if (folderExists)
                {
                    resultBox.AppendText($"Folder exists{System.Environment.NewLine}");
                }
                else
                {
                    var folderCreated = await _graphClient.CreateFolder(folderName);
                    if (folderCreated)
                    {
                        resultBox.AppendText($"Folder created{System.Environment.NewLine}");
                    }
                    else
                    {
                        resultBox.AppendText($"Folder not created{System.Environment.NewLine}");
                    }
                }
            }
            catch (Exception ex)
            {
                resultBox.AppendText($"Error creating folder:{System.Environment.NewLine}{ex.Message}{System.Environment.NewLine}");
            }
        }

        private async void uploadButton_Click(object sender, EventArgs e)
        {
            try
            {
                var uploaded = await _graphClient.UploadFile("Test", @"C:\Users\Ken\Desktop\useful commands.txt");
                resultBox.AppendText($"File uploaded{System.Environment.NewLine}");
            }
            catch (Exception ex)
            {
                resultBox.AppendText($"Error uploading file:{System.Environment.NewLine}{ex.Message}{System.Environment.NewLine}");
            }
        }

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            try
            {
                var item = await _graphClient.GetItem("Test/Ken-Pitch2AFC-Run1014-Block1-Track1-Trial1.json");
                _graphClient.DownloadFile(item.url, @"C:\Users\hancock\Desktop\shit.txt");
                resultBox.AppendText($"File downloaded{System.Environment.NewLine}");
            }
            catch (Exception ex)
            {
                resultBox.AppendText($"Error downloading file:{System.Environment.NewLine}{ex.Message}{System.Environment.NewLine}");
            }
        }
    }
}
