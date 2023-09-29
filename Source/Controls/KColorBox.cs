using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KLib.Controls
{
    public partial class KColorBox : KUserControl
    {
        private Color _value = Color.White;

        public KColorBox()
        {
            InitializeComponent();
        }

        public Color Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ShowColor();
            }
        }

        public uint ValueAsUInt
        {
            get { return (uint)_value.ToArgb(); }
            set
            {
                _value = Color.FromArgb((int)value);
                ShowColor();
            }
        }

        private void ShowColor()
        {
            colorBox.BackColor = _value;
        }

        private void colorBox_Click(object sender, EventArgs e)
        {
            KColorDialog dlg = new KColorDialog();
            dlg.Value = _value;

            Point pt = this.PointToScreen(Location);

            dlg.Location = new Point(pt.X - dlg.Width / 2, pt.Y - dlg.Height / 2);
            dlg.StartPosition = FormStartPosition.Manual;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _value = dlg.Value;
                colorBox.BackColor = _value;
                OnValueChanged();
            }
        }

    }
}
