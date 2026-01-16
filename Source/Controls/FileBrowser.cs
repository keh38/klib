using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLib.Controls
{
    [DefaultEvent(nameof(ValueChanged))]
    public partial class FileBrowser : KUserControl
    {
        public string DefaultFolder { get; set; }
        public bool UseEllipsis { get; set; }
        public string Filter { get; set; }
        public bool FoldersOnly { get; set; }
        public bool HideFolder { get; set; }
        public bool FileMustExist { get; set; }
        public bool ReadOnly
        {
            get { return textBox.ReadOnly; }
            set { textBox.ReadOnly = value; }
        }
        public bool ShowSaveButton
        {
            get { return SaveButton.Visible; }
            set { SaveButton.Visible = value && !FoldersOnly; }
        }


        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ShowValue();
            }
        }

        private string _value = "";

        public FileBrowser()
        {
            InitializeComponent();
        }

        private void FileBrowser_Resize(object sender, EventArgs e)
        {
            Rectangle bb = BrowseButton.Bounds;
            Rectangle sb = SaveButton.Bounds;
            Rectangle tb = textBox.Bounds;

            int x = this.Bounds.Width;

            if (SaveButton.Visible)
            {
                x -= sb.Width;
                SaveButton.SetBounds(x, sb.Y, sb.Width, sb.Height);
            }
            x -= bb.Width;

            BrowseButton.SetBounds(x, bb.Y, bb.Width, bb.Height);
            textBox.SetBounds(tb.X, tb.Y, x - tb.X, tb.Height);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();

            // Set filter options and filter index.
            dlg.Filter = Filter;
            dlg.FilterIndex = 1;
            dlg.OverwritePrompt = true;

            string folder = string.IsNullOrEmpty(_value) ? "" : Path.GetDirectoryName(_value);
            dlg.InitialDirectory = Directory.Exists(folder) ? folder : DefaultFolder;
            dlg.FileName = string.IsNullOrEmpty(_value) ? "" : Path.GetFileName(_value);

            // Call the ShowDialog method to show the dialog box.
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _value = dlg.FileName;
                ShowValue();
                OnSaveFile();
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (FoldersOnly)
            {
                BrowseFolders();
            }
            else
            {
                BrowseFiles();
            }
        }

        private void BrowseFiles()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filter options and filter index.
            dlg.Filter = Filter;
            dlg.FilterIndex = 1;
            dlg.Multiselect = false;
            dlg.CheckFileExists = true;

            string folder = string.IsNullOrEmpty(_value) ? "" : Path.GetDirectoryName(_value);
            dlg.InitialDirectory = Directory.Exists(folder) ? folder : DefaultFolder;
            dlg.FileName = string.IsNullOrEmpty(_value) ? "" : Path.GetFileName(_value);

            // Call the ShowDialog method to show the dialog box.
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _value = dlg.FileName;
                ShowValue();
                OnValueChanged();
            }
        }

        private void BrowseFolders()
        {
            var dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            dlg.SelectedPath = _value;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _value = dlg.SelectedPath;
                ShowValue();
                OnValueChanged();
            }
        }

        private void ShowValue()
        {
            if (string.IsNullOrEmpty(_value))
            {
                textBox.Text = "";
            }
            else if (!FoldersOnly && HideFolder)
            {
                textBox.Text = Path.GetFileName(_value);
            }
            else
            {
                textBox.Text = UseEllipsis ? GetCompactedString(_value, textBox.Font, textBox.Width) : _value;
            }
        }

        public static string GetCompactedString(string stringToCompact, Font font, int maxWidth)
        {
            if (string.IsNullOrEmpty(stringToCompact)) return stringToCompact;

            // Copy the string passed in since this string will be
            // modified in the TextRenderer's MeasureText method
            string compactedString = string.Copy(stringToCompact);
            var maxSize = new Size(maxWidth, 0);
            var formattingOptions = TextFormatFlags.PathEllipsis
                                  | TextFormatFlags.ModifyString;
            TextRenderer.MeasureText(compactedString, font, maxSize, formattingOptions);
            return compactedString;
        }

        private void ValidateStateName()
        {
            if (FoldersOnly)
            {
                if (Directory.Exists(textBox.Text))
                {
                    _value = textBox.Text;
                    OnValueChanged();
                }
            }
            else if (HideFolder)
            {
                string tempfn = textBox.Text;
                if (!string.IsNullOrEmpty(_value))
                {
                    tempfn = Path.Combine(Path.GetDirectoryName(_value), textBox.Text);
                }
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    _value = "";
                    OnValueChanged();
                }
                else if (!FileMustExist || File.Exists(tempfn))
                {
                    _value = tempfn;
                    OnValueChanged();
                }
            }
            else if (!FileMustExist || File.Exists(textBox.Text))
            {
                _value = textBox.Text;
                OnValueChanged();
            }
            else if (string.IsNullOrEmpty(textBox.Text))
            {
                _value = "";
                OnValueChanged();
            }

            ShowValue();
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            ValidateStateName();
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                textBox.Text = _value;
                e.Handled = true;
                ValidateStateName();
            }
            else if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                ValidateStateName();
            }
            else
            {
                base.OnKeyPress(e);
            }

        }

        public event EventHandler SaveFile;
        protected virtual void OnSaveFile()
        {
            if (this.SaveFile != null)
            {
                SaveFile(this, null);
            }
        }

    }
}
