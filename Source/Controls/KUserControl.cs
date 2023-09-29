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
    public partial class KUserControl : UserControl
    {
        protected bool _ignoreEvents;

        [Category("KLib")]
        [Description("Occurs when control value changed")]
        public event EventHandler ValueChanged;
        protected virtual void OnValueChanged()
        {
            if (this.ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

        public KUserControl()
        {
            InitializeComponent();
        }


    }
}
