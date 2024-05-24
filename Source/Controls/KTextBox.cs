using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLib.Controls
{
    [DefaultEvent(nameof(ValueChanged))]
    public partial class KTextBox : UserControl
    {
        private string _value;

        public KTextBox()
        {
            InitializeComponent();
        }

        public event EventHandler ValueChanged;
        protected virtual void OnValueChanged()
        {
            if (this.ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

        public Color FontColor
        {
            set { textBox.ForeColor = value; }
            get { return textBox.ForeColor; }
        }

        public bool IsIPAddress { set; get; }

        public string Text
        {
            get { return _value; }
            set
            {
                _value = value;
                textBox.Text = value;
            }
        }

        public HorizontalAlignment TextAlign
        {
            get { return textBox.TextAlign; }
            set { textBox.TextAlign = value; }
        }

        public void SetContextMenu(ContextMenuStrip cms)
        {
            textBox.ContextMenuStrip = cms;
        }

        private void KTextBox_Resize(object sender, EventArgs e)
        {
            Rectangle b = textBox.Bounds;
            textBox.SetBounds(b.X, b.Y, this.Bounds.Width, b.Height);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                textBox.Text = _value;
                e.Handled = true;
                OnValueChanged();
            }
            else if (e.KeyChar == (char)13)
            {
                _value = textBox.Text;
                e.Handled = true;
                OnValueChanged();
            }
            else
            {
                base.OnKeyPress(e);
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            _value = textBox.Text;
            OnValueChanged();
        }

        private bool ValidateIPAddress()
        {
            return true;
        }

    }
}
