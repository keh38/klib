using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KLib.Crypto;
using KLib.Crypto.PublicKey;

namespace EncryptionTool
{
    public partial class MainWindow : Form
    {
        string xmlPublicKey;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Encrypt_Click(object sender, EventArgs e)
        {
            //Crypto c = new Crypto();
             //string encryptedString = c.Encrypt(inputBox.Text);

            PublicKeyCrypto crypto = new PublicKeyCrypto(xmlPublicKey, false);
            string encryptedString = crypto.Encrypt(inputBox.Text);
            resultBox.Text = encryptedString;
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            //Crypto c = new Crypto();
            //string decryptedString = c.Decrypt(resultBox.Text);

            try
            {

                //PublicKeyCrypto crypto = new PublicKeyCrypto(xmlPublicKey, false);
                PublicKeyCrypto crypto = new PublicKeyCrypto(containerNameBox.Text);
                string decryptedString = crypto.Decrypt(resultBox.Text);
                decryptBox.Text = decryptedString;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Decryption failed: " + ex.Message);
            }
        }

        private void CreateKeysButton_Click(object sender, EventArgs e)
        {
            PublicKeyCrypto.CreateKeys(containerNameBox.Text);
        }

        private void ExportPublicButton_Click(object sender, EventArgs e)
        {
            PublicKeyCrypto crypto = new PublicKeyCrypto(containerNameBox.Text);
            string xml = crypto.GetPublicKey();

            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(xml);
                    myStream.Write(byteArray, 0, byteArray.Length);
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }

        }

        private void ImportKeyButton_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\" ;
            openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*" ;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true ;

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            int n = (int) myStream.Length;
                            byte[] byteArray = new byte[n];
                            myStream.Read(byteArray, 0, n);
                            myStream.Close();

                            xmlPublicKey = Encoding.UTF8.GetString(byteArray);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }


    }
}
