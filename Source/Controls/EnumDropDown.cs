using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLib.Controls
{
    [DefaultEvent(nameof(ValueChanged))]
    public partial class EnumDropDown : ComboBox
    {
        private Dictionary<string, int> _cbItems = new Dictionary<string, int>();
        private int _value;
        protected bool _ignoreEvents;
        
        public event EventHandler ValueChanged;
        protected virtual void OnValueChanged()
        {
            if (this.ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

        public bool Sort
        {
            set; get;
        }

        public int Value
        {
            get { return _value; }
        }

        public void SetEnumValue(Enum value)
        {
            _ignoreEvents = true;

            if (_cbItems.Values.Contains(Convert.ToInt32(value)))
            {
                string n = _cbItems.First(c => c.Value == Convert.ToInt32(value)).Key;
                SelectedIndex = Items.IndexOf(n);
            }
            else SelectedIndex = -1;

            _ignoreEvents = false;
        }

        public void SetData(int value)
        {
            _ignoreEvents = true;

            if (value < 0)
            {
                SelectedIndex = -1;
            }
            else
            {
                string n = _cbItems.First(c => c.Value == value).Key;
                SelectedIndex = Items.IndexOf(n);
            }

            _ignoreEvents = false;
        }

        public EnumDropDown()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
        public void Fill<T>()
        {
            Fill(typeof(T), null);
        }

        public void Fill(Type t)
        {
            Fill(t, null);
        }

        public void Clear()
        {
            Items.Clear();
            _cbItems.Clear();
        }

        public void AddItem(Enum item, string name)
        {
            _cbItems.Add(name, Convert.ToInt32(item));
            Items.Add(name);
        }

        public void FillSubset<Enum>(params Enum[] list)
        {
            _ignoreEvents = true;

            List<string> items = new List<string>();

            _cbItems.Clear();
            foreach (Enum e in list)
            {
                string name = System.Enum.GetName(typeof(Enum), e).Replace('_', ' ');
                //name = KString.InsertSpacesAtCaseChanges(name);
                _cbItems.Add(name, Convert.ToInt32(e));

                items.Add(name);
            }

            if (Sort)
            {
                items.Sort();
            }

            Items.Clear();
            Items.AddRange(items.ToArray());

            //SelectedIndex = 0;
            _ignoreEvents = false;
        }

        public void Fill(Type t, int[] v)
        {
            _ignoreEvents = true;

            List<string> items = new List<string>();

            _cbItems.Clear();
            foreach (int value in Enum.GetValues(t))
            {
                string name = Enum.GetName(t, value).Replace('_', ' ');
                if (!_cbItems.ContainsKey(name))
                {
                    _cbItems.Add(name, value);

                    if (v == null || v.Contains(value) || value == 0)
                    {
                        items.Add(name);
                    }
                }
            }

            if (Sort)
            {
                items.Sort();
            }

            Items.Clear();
            Items.AddRange(items.ToArray());

            SelectedIndex = 0;
            _ignoreEvents = false;
        }
        private void EnumDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            _value = SelectedIndex >= 0 ? _cbItems[SelectedItem as string] : 0;
            if (!_ignoreEvents)
            {
                OnValueChanged();
            }
        }
    }
}
