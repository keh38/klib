using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.Versioning;

namespace KLib.Controls
{
    [DefaultEvent(nameof(ValueChanged))]
    [SupportedOSPlatform("windows")]
    public partial class KNumericBox : UserControl
    {
        private double _minVal = 0;
        private double _maxVal = double.MaxValue;
        private string _textFormat = "K4";
        private string _units = "";

        private double _value;

        private int _wheelDelta;

        private ToolTip _toolTip = new ToolTip();

        public KNumericBox()
        {
            InitializeComponent();
            textBox.MouseWheel += new MouseEventHandler(textbox_MouseWheel);

            _wheelDelta = SystemInformation.MouseWheelScrollDelta;
        }

        public event EventHandler ValueChanged;
        protected virtual void OnValueChanged()
        {
            if (this.ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

        public bool ClearOnDisable
        {
            get; set;
        }

        public bool MinCoerce
        {
            get; set;
        }

        public double MinValue
        {
            get { return _minVal; }
            set { _minVal = value; }
        }

        public bool MaxCoerce
        {
            get; set;
        }

        public double MaxValue
        {
            get { return _maxVal; }
            set { _maxVal = value; }
        }

        public string TextFormat
        {
            get { return _textFormat; }
            set { _textFormat = value; }
        }

        public string Units
        {
            get { return _units; }
            set { _units = value; Redisplay(); }
        }

        public bool IsInteger
        {
            get; set;
        }

        public bool AllowInf
        {
            set; get;
        }

        public double Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        public float FloatValue
        {
            get { return (float)_value; }
            set { SetValue(value); }
        }

        public int IntValue
        {
            get { return (int)_value; }
            set { SetValue(value); }
        }

        public string ToolTip
        {
            set { _toolTip.SetToolTip(textBox, value); }
            get { return _toolTip.GetToolTip(textBox); }
        }

        private void Redisplay()
        {
            if (IsInteger)
            {
                textBox.Text = FormatInteger(_value, _textFormat);
            }
            else
            {
                textBox.Text = FormatNumber(_value, _textFormat);
            }

            if (!double.IsPositiveInfinity(_value) && !double.IsNegativeInfinity(_value)) textBox.Text += " " + _units;
        }

        private void KNumericBox_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(10, textBox.Bounds.Height);
            this.MaximumSize = new Size(20000, textBox.Bounds.Height);
        }

        private void KNumericBox_Resize(object sender, EventArgs e)
        {
            Rectangle b = textBox.Bounds;
            textBox.SetBounds(b.X, b.Y, this.Bounds.Width, b.Height);
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpdateValue();
        }

        const int WM_KEYUP = 0x0101;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == WM_KEYUP)
            {
                KeyEventArgs args1 = new KeyEventArgs(((Keys)((int)m.WParam)) | Control.ModifierKeys);

                switch (args1.KeyCode)
                {
                    case Keys.Up:
                        IncrementValue(1);
                        return false;
                    case Keys.Down:
                        IncrementValue(-1);
                        return false;
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            
            System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();

            if (Char.IsDigit(e.KeyChar))
            {
                // Digits are OK
            }
            else if (keyInput.Equals(decimalSeparator) && !IsInteger)
            {
                // Decimal point OK if non-integer
            }
            else if (e.KeyChar == 'i')
            {
                // Infinity OK
            }
            else if (keyInput.Equals(negativeSign) && (_minVal < 0 || textBox.Text.Contains("e") ||!MinCoerce))
            {
                // Negative sign OK if min value is negative
            }
            else if (e.KeyChar == 'e')
            {
                // Scientific notation OK
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            else if (e.KeyChar == (char)13)
            {
                // Enter key: get the value
                e.Handled = true;
                UpdateValue();
            }
            //else if (e.KeyChar
            //{
            //    // Enter key: get the value
            //    e.Handled = true;
            //    UpdateValue();
            //}
            else
            {
                // Consume this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }
        }

        private void SetValue(double val)
        {
            _value = val;
            if (MinCoerce) val = Math.Max(_minVal, val);
            if (MaxCoerce)
            {
                if (AllowInf && val > _maxVal)
                    val = double.PositiveInfinity;
                else
                    val = Math.Min(_maxVal, val);
            }
            _value = val;

            Redisplay();
        }

        private void SetValue(float val)
        {
            if (MinCoerce) val = Math.Max((float)_minVal, val);
            if (MaxCoerce)
            {
                if (AllowInf && val > _maxVal)
                    val = float.PositiveInfinity;
                else
                    val = Math.Min((float)_maxVal, val);
            }
            _value = val;

            Redisplay();
        }

        private string FormatNumber(double val, string format)
        {
            if (double.IsPositiveInfinity(val))
            {
                return "INF";
            }

            if (string.IsNullOrEmpty(format) || format == "F")
            {
                return val.ToString();
            }

            if (format[0] != 'K')
            {
                return val.ToString(format);
            }
            int ndec = int.Parse(format.Substring(1));
            string s = val.ToString("F" + ndec.ToString());

            int dot = s.IndexOf('.');
            if (dot < 0)
            {
                return s;
            }

            int firstKeep = dot-1;
            for (int k=s.Length-1; k>dot; k--)
            {
                if (s[k] != '0')
                {
                    firstKeep = k;
                    break;
                }
            }

            return s.Substring(0, firstKeep + 1);
        }

        private string FormatInteger(double val, string format)
        {
            if (string.IsNullOrEmpty(format) || format.Equals("K4"))
            {
                return ((int)_value).ToString();
            }

            if (double.IsPositiveInfinity(val))
            {
                return "INF";
            }

            return val + format.Replace("{max}", _maxVal.ToString());
        }

        private void UpdateValue()
        {
            double v = _value;
            string text = textBox.Text;
            int index = text.IndexOf(_units);
            if (index > 0)
            {
                text = text.Remove(index, _units.Length);
            }

            if (text.Contains("e"))
            {
                string[] s = text.Split(new char[] { 'e' });
                if (s.Length == 2)
                { 
                    double mult = double.Parse(s[0]);
                    double exp = double.Parse(s[1]);
                    v = mult * Math.Pow(10, exp);
                }
                SetValue(v);
            }
            else if (text.ToLower().Contains("i"))
            {
                if (IsInteger)
                {
                    v = text.ToLower().Contains("-i") ? int.MinValue : int.MaxValue;
                }
                else
                {
                    v = text.ToLower().Contains("-i") ? double.NegativeInfinity : double.PositiveInfinity;
                }
                if (v != _value)
                {
                    SetValue(v);
                    OnValueChanged();
                }
            }
            else if (IsInteger && !string.IsNullOrEmpty(TextFormat))
            {
                var m = Regex.Match(text, @"([\-\d]+)");
                if (m.Success && double.TryParse(m.Groups[1].Value, out v))
                { 
                    if (v != _value)
                    {
                        SetValue(v);
                        OnValueChanged();
                    }
                }
            }
            else
            {
                if (double.TryParse(text, out v))
                {
                    if (v != _value)
                    {
                        SetValue(v);
                        OnValueChanged();
                    }
                }
            }
        }

        private void KNumericBox_EnabledChanged(object sender, EventArgs e)
        {
            if (ClearOnDisable)
            {
                if (Enabled)
                {
                    Redisplay();
                }
                else
                {
                    textBox.Text = "";
                }
            }
        }

        private void textbox_MouseWheel(object sender, MouseEventArgs e)
        {
            IncrementValue((int)(e.Delta / _wheelDelta));
        }

        private void IncrementValue(int delta)
        {
            double oldValue = _value;
            if (delta != 0)
            {
                if (double.IsPositiveInfinity(_value) && delta < 0 && !double.IsPositiveInfinity(_maxVal))
                {
                    SetValue(_maxVal);
                }
                else
                {
                    SetValue(_value + delta);
                }

                if (_value != oldValue)
                {
                    OnValueChanged();
                }
            }
        }
    }
}
