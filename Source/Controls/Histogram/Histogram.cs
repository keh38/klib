using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace KLib.Controls
{
    /// <summary>
    /// Summary description for Histogram.
    /// </summary>
    public class Histogram : KUserControl
	{
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        private float[] _values;
        private float _maxVal;

        private float _yUnit; //this gives the vertical unit used to scale our values
        private float _xUnit; //this gives the horizontal unit used to scale our values
        private int _offset = 20; //the offset, in pixels, from the control margins.

        private AxisScale _xScale = new AxisScale();
        private ThresholdBar _thresholdBar = new ThresholdBar();

        private float _yMax = 1;

        private Color _histogramColor = Color.Black;
        private Color _axisColor = Color.Black;
        private Font _font = new Font("Tahoma", 10);

        private bool _showBox = true;

        private enum DragState { Off, X, Y};
        private DragState _dragState = DragState.Off;
        private bool _shiftDown;
        private float _ySelect;
        private float _original_yMax;

        private float _xbar;

        public Histogram()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            _dragState = DragState.Off;
            _shiftDown = false;

			this.Paint += new PaintEventHandler(Histogram_Paint);
			this.Resize += new EventHandler(Histogram_Resize);
            this.MouseMove += new MouseEventHandler(Histogram_MouseMove);
            this.MouseDown += new MouseEventHandler(Histogram_MouseDown);
            this.MouseUp += new MouseEventHandler(Histogram_MouseUp);
            this.MouseLeave += new EventHandler(Histogram_MouseLeave);
            this.KeyDown += new KeyEventHandler(Histogram_KeyDown);
            this.KeyUp += new KeyEventHandler(Histogram_KeyUp);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// HistogramaDesenat
			// 
			this.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "Histogram";
			this.Size = new Size(208, 176);
		}
        #endregion

        [Category("Histogram Options")]
        [Description("The distance from the margins for the histogram")]
        public int Offset
        {
            set
            {
                if (value > 0)
                    _offset = value;
            }
            get { return _offset; }
        }

        [Category("Histogram Options")]
        [Description("The color used within the control")]
        public Color DisplayColor
        {
            set { _histogramColor = value; }
            get { return _histogramColor; }
        }

        [Category("Histogram Options")]
        [Description("X-axis scale")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AxisScale XScale
        {
            set { _xScale = value; }
            get { return _xScale; }
        }

        [Category("Histogram Options")]
        [Description("Histogram maximum value")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public float HistogramMax
        {
            set { _yMax = value; }
            get { return _yMax; }
        }

        [Category("Histogram Options")]
        [Description("Threshold bar")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ThresholdBar ThresholdBar
        {
            set { _thresholdBar = value;  }
            get { return _thresholdBar; }
        }

        [Category("Histogram Options")]
        [Description("Plot box around outside of histogram")]
        public bool ShowBox
        {
            set { _showBox = value; }
            get { return _showBox; }
        }

        [ReadOnly(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Threshold
        {
            set { _thresholdBar.Value = value; }
            get { return _thresholdBar.Value; }
        }

        private void Histogram_Paint(object sender, PaintEventArgs e)
		{
            Graphics g = e.Graphics;
            Pen pen = new Pen(new SolidBrush(_histogramColor), _xUnit);

            ComputeXYUnitValues();

            float y0 = _offset + _font.Height;

            // Draw X-axis
            pen.Color = _axisColor;
            pen.Width = 1;
            g.DrawLine(pen,
                new PointF(_offset + (_xScale.Min * _xUnit), this.Height - y0),
                new PointF(_offset + (_xScale.Max * _xUnit), this.Height - y0));

            // Draw Y-axis
            g.DrawLine(pen,
                new PointF(_offset, this.Height - y0),
                new PointF(_offset, _offset));

            if (_values != null)
            {
                pen.Color = _histogramColor;
                pen.Width = (float)Math.Ceiling(_xUnit);
                //The width of the pen is given by the XUnit for the control.
                for (int i = 0; i < _values.Length; i++)
                {
                    // Draw each line 
                    g.DrawLine(pen,
                        new PointF(_offset + (i * _xUnit), this.Height - y0),
                        new PointF(_offset + (i * _xUnit), this.Height - y0 - _values[i] * _yUnit));
                }
            }

            // Draw threshold bar
            if (_thresholdBar.Visible)
            {
                pen.Color = _thresholdBar.Color;
                pen.Width = _thresholdBar.Width;
                _xbar = _offset + (_thresholdBar.Value * _xUnit);
                g.DrawLine(pen,
                    new PointF(_xbar, this.Height - y0),
                    new PointF(_xbar, _offset));
            }

            //We draw the indexes for 0 and for the length of the array being plotted
            g.DrawString(_xScale.Min.ToString(), _font,
                new SolidBrush(_axisColor),
                new PointF(_offset, this.Height - _font.Height - _offset),
                StringFormat.GenericDefault);
            g.DrawString(_xScale.Max.ToString(), _font,
                new SolidBrush(_axisColor),
                new PointF(_offset + (_xScale.Max * _xUnit) - g.MeasureString(_xScale.Max.ToString(), _font).Width,
                this.Height - _font.Height - _offset),
                StringFormat.GenericDefault);

            if (_showBox)
            {
                //Draw a rectangle surrounding the control.
                g.DrawRectangle(new Pen(new SolidBrush(Color.Black), 1), 0, 0, this.Width - 1, this.Height - 1);
            }

		}

        /// <summary>
        /// We draw the histogram on the control
        /// </summary>
        /// <param name="myValues">The values beeing draw</param>
        public void DrawHistogram(float[] values)
		{
			_values = new float[values.Length];

            float sum = 0;
            for (int k = 0; k < values.Length; k++)
                sum += values[k];

            _maxVal = 0;
            for (int k = 0; k < values.Length; k++)
            {
                _values[k] = values[k] / sum;
                _maxVal = Math.Max(_maxVal, _values[k]);
            }

			this.Refresh();
		}

		private void Histogram_Resize(object sender, EventArgs e)
		{
			this.Refresh();
		}

		private void ComputeXYUnitValues()
		{
			_yUnit = (float) (this.Height - (2 * _offset + _font.Height)) / _yMax;
			_xUnit = (this.Width - (2 * _offset)) / (_xScale.Range());
		}

        private void Histogram_MouseMove(object sender, MouseEventArgs e)
        {
            if (_thresholdBar.Visible && _thresholdBar.Drag)
            {
                if (_dragState == DragState.X)
                {
                    int thr = MouseXToThreshold(e.X);
                    if (thr != _thresholdBar.Value)
                    {
                        _thresholdBar.Value = thr;
                        Refresh();
                        OnValueChanged();
                    }
                }
                else if (_dragState == DragState.Y)
                {
                    float y = MouseYToHistogramValue(e.Y);
                    _yMax = _original_yMax + (y - _ySelect);
                    //_yMax = Math.Max(_yMax, 0.0005f);
                    //_yMax = Math.Min(_yMax, _maxVal);
                    Refresh();
                }
                else
                {
                    Cursor = MouseOverThresholdBar(e.Location) ? Cursors.SizeWE : Cursors.Default;
                }
            }
        }

        private void Histogram_MouseDown(object sender, MouseEventArgs e)
        {
            if (_thresholdBar.Visible && _thresholdBar.Drag && MouseOverThresholdBar(e.Location) && !_shiftDown)
            {
                _dragState = DragState.X;
            }

            if (_shiftDown && _dragState != DragState.X)
            {
                _ySelect = MouseYToHistogramValue(e.Y);
                _original_yMax = _yMax;
                _dragState = DragState.Y;
                Cursor = Cursors.SizeNS;
            }
        }

        private void Histogram_MouseUp(object sender, MouseEventArgs e)
        {
            _dragState = DragState.Off;
        }

        private void Histogram_MouseLeave(object sender, EventArgs e)
        {
            _dragState = DragState.Off;
            Cursor = Cursors.Default;
        }

        private void Histogram_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                _shiftDown = true;
            }
        }

        private void Histogram_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Shift)
            {
                _shiftDown = false;
                if (_dragState == DragState.Y)
                {
                    _dragState = DragState.Off;
                    Cursor = Cursors.Default;
                }
            }
        }

        private bool MouseOverThresholdBar(Point location)
        {
            return Math.Abs(_xbar - location.X) <= _thresholdBar.GrabTolerance;
        }

        private int MouseXToThreshold(int x)
        {
            float v = (x - _offset) / _xUnit;
            int threshold = (int)Math.Round(v);
            threshold = (threshold < _xScale.Min) ? (int)_xScale.Min : threshold;
            threshold = (threshold > _xScale.Max) ? (int)_xScale.Max : threshold;
            return threshold;
        }

        private float MouseYToHistogramValue(int y)
        {
            return (y - _offset - _font.Height) / _yUnit;
        }

    }
}
