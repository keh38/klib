using System;
using System.ComponentModel;
using System.Drawing;

namespace KLib.Controls
{
    [TypeConverter(typeof(ThresholdBarConverter))]
    public class ThresholdBar
    {
        public Color Color { set; get; }
        public bool Drag { set; get; }
        public int GrabTolerance { set; get; }
        public int Width { set; get; }
        public int Value { set; get; }
        public bool Visible { set; get; }

        public ThresholdBar()
        {
            Color = Color.Red;
            Drag = true;
            GrabTolerance = 5;
            Width = 3;
            Value = 128;
            Visible = true;
        }
    }

    public class ThresholdBarConverter : TypeConverter
    {
        public ThresholdBarConverter()
        {
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(ThresholdBar));
        }
    }
}