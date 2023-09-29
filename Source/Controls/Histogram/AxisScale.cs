using System;
using System.ComponentModel;

namespace KLib.Controls
{
    [TypeConverter(typeof(AxisScaleConverter))]
    public class AxisScale
    {
        public bool Auto { set; get; }
        public int Min { set; get; }
        public int Max { set; get; }
        public AxisScale()
        {
            Auto = false;
            Min = 0;
            Max = 255;
        }
        public float Range()
        {
            return Max - Min;
        }
    }

    public class AxisScaleConverter : TypeConverter
    {
        public AxisScaleConverter()
        {
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(AxisScale));
        }
    }

}