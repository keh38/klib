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
    public partial class ValueSlider : KUserControl
    {
        private double _minVal = 0;
        private double _maxVal = 1;
        private double _stepVal = 0.1;
        private double _resol = 0.01;

        private bool _isInteger = false;

        private double _value = 0;

        private int _offset = 5;
        private int _lastSliderStep = 0;

        public double MinValue
        {
            get { return _minVal; }
            set
            {
                _minVal = value;
                valueNumeric.MinValue = value;
                ResetScale();
            }
        }

        public double MaxValue
        {
            get { return _maxVal; }
            set
            {
                _maxVal = value;
                valueNumeric.MaxValue = value;
                ResetScale();
            }
        }

        public double Resolution
        {
            get { return _resol; }
            set { _resol = value; ResetScale(); }
        }

        public double TickSpacing
        {
            get { return _stepVal; }
            set { _stepVal = value; ResetScale(); }
        }

        public string Title
        {
            get { return titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        public string MinLabel
        {
            get { return minLabel.Text; }
            set { minLabel.Text = value; }
        }

        public string MaxLabel
        {
            get { return maxLabel.Text; }
            set { maxLabel.Text = value; RepositionMaxLabel(); }
        }

        public bool IsInteger
        {
            get { return _isInteger; }
            set
            {
                _isInteger = value;
                valueNumeric.IsInteger = value;
                if (_isInteger)
                {
                    Resolution = 1;
                }
            }
        }

        public ValueSlider()
        {
            InitializeComponent();
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; UpdateValue(); }
        }

        public float ValueAsFloat
        {
            get { return (float)_value; }
            set { _value = value; }
        }

        private void UpdateValue()
        {
            _ignoreEvents = true;

            _value = CoerceValue(_value);
            int step = (int)((_value - _minVal) / _resol);
            valueTrackBar.Value = step;
            _lastSliderStep = step;

            valueNumeric.Value = _value;

            _ignoreEvents = false;
        }

        private void ResetScale()
        {
            valueTrackBar.Maximum = (int)((_maxVal - _minVal) / _resol);
            valueTrackBar.TickFrequency = (int)(_stepVal / _resol);

            if (_isInteger)
            {
                valueNumeric.TextFormat = "";
            }
            else
            {
                int numPlacesAfterDecimalPoint = -(int)(Math.Round(Math.Log10(_resol)));
                string fmt = (numPlacesAfterDecimalPoint > 0) ? "F" + numPlacesAfterDecimalPoint.ToString() : "F0";
                valueNumeric.TextFormat = fmt;
            }
        }

        private void RepositionTextBox()
        {
            Rectangle b = valueNumeric.Bounds;
            valueNumeric.SetBounds(valueTrackBar.Bounds.Right - b.Width - _offset - 3, b.Y, b.Width, b.Height);
        }

        private void RepositionMaxLabel()
        {
            Rectangle b = maxLabel.Bounds;
            maxLabel.SetBounds(valueTrackBar.Bounds.Right - b.Width, b.Y, b.Width, b.Height);
        }

        private void RepositionMinLabel()
        {
            Rectangle b = minLabel.Bounds;
            minLabel.SetBounds(_offset, b.Y, b.Width, b.Height);
        }

        private void RepositionTitle()
        {
            Rectangle b = titleLabel.Bounds;
            titleLabel.SetBounds(_offset, b.Y, b.Width, b.Height);
        }

        private void KValueSlider_Resize(object sender, EventArgs e)
        {
            int right = this.Bounds.Right;
            Rectangle b = valueTrackBar.Bounds;
            valueTrackBar.SetBounds(_offset, b.Y, this.Bounds.Width - 2* _offset, b.Height);

            RepositionMinLabel();
            RepositionMaxLabel();
            RepositionTitle();
            RepositionTextBox();
        }

        private void valueTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                if (valueTrackBar.Value != _lastSliderStep)
                {
                    _lastSliderStep = valueTrackBar.Value;
                    _value = _minVal + _resol * (float)valueTrackBar.Value;

                    _ignoreEvents = true;
                    valueNumeric.Value = _value;
                    _ignoreEvents = false;
                    OnValueChanged();
                }
            }
        }

        private double CoerceValue(double testVal)
        {
            double coercedVal = testVal;
            if (coercedVal < _minVal || testVal > _maxVal)
            {
                coercedVal = Math.Min(coercedVal, _maxVal);
                coercedVal = Math.Max(coercedVal, _minVal);
            }
            return coercedVal;
        }

        private void valueNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                double v = valueNumeric.Value; 
                _ignoreEvents = true;
                v = CoerceValue(v);
                _value = v;
                valueNumeric.Value = v;
                valueTrackBar.Value = (int)((_value - _minVal) / _resol);

                _ignoreEvents = false;

                if (_lastSliderStep != valueTrackBar.Value)
                {
                    _lastSliderStep = valueTrackBar.Value;
                    OnValueChanged();
                }
            }
        }
    }
}
