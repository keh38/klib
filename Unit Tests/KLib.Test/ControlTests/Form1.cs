using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlTests
{
    public partial class MainForm : Form
    {
        private List<SampleClass> _sampleClasses;

        public MainForm()
        {
            InitializeComponent();

            _sampleClasses = new List<SampleClass>();

            collectionListBox1.AttachPropertyGrid(propertyGrid1);
            collectionListBox1.CreateNewItem += CreateNewItem;
            collectionListBox1.Collection = _sampleClasses;
        }

        private object CreateNewItem(object item)
        {
            if (item != null)
            {
                if (item is string)
                {
                    return new SampleClass { Name = (string)item };
                }
                else if (item is SampleClass)
                {
                    var original = (SampleClass)item;
                    return new SampleClass
                    {
                        Name = original.Name + "_2",
                        Value = original.Value,
                        Other = original.Other
                    };
                }
                return null;
            }
            return null;
        }
    }
}
