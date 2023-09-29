using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KLib.Controls
{
    public partial class KColorDialog : Form
    {
        private Color _value = Color.White;
        private bool _ignoreEvents = false;

        public KColorDialog()
        {
            InitializeComponent();
        }

        public Color Value
        {
            get { return _value; }
            set
            {
                _ignoreEvents = true;

                _value = value;
                redSlider.Value = value.R;
                greenSlider.Value = value.G;
                blueSlider.Value = value.B;
                alphaSlider.Value = value.A;

                colorBox.BackColor = value;

                _ignoreEvents = false;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void redSlider_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _value = Color.FromArgb(_value.A, (byte)redSlider.Value, _value.G, _value.B);
                colorBox.BackColor = _value;
            }
        }

        private void greenSlider_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _value = Color.FromArgb(_value.A, _value.R, (byte)greenSlider.Value, _value.B);
                colorBox.BackColor = _value;
            }
        }

        private void blueSlider_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _value = Color.FromArgb(_value.A, _value.R, _value.G, (byte)blueSlider.Value);
                colorBox.BackColor = _value;
            }
        }

        private void alphaSlider_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _value = Color.FromArgb((byte)alphaSlider.Value, _value);
                colorBox.BackColor = _value;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void colorBox_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = _value;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _value = Color.FromArgb(_value.A, dlg.Color);
                redSlider.Value = _value.R;
                greenSlider.Value = _value.G;
                blueSlider.Value = _value.B;
                colorBox.BackColor = _value;
            }
        }
    }
}
