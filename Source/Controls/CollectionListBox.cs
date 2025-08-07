using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLib.Controls
{
    [SupportedOSPlatform("windows")]
    public partial class CollectionListBox : UserControl
    {
        private IList _collection;
        public IList Collection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                UpdateCollectionList();
                if (_collection != null && _collection.Count > 0)
                {
                    listBox.SelectedIndex = 0;
                }
            }
        }

        public string ListTitle
        {
            get { return listBoxLabel.Text; }
            set { listBoxLabel.Text = value; }
        }

        public bool ShowAddDropDown
        {
            get { return addDropDown.Visible; }
            set { addDropDown.Visible = value; addButton.Visible = !value; }
        }

        public BindingList<string> AddDropDownItems { get; set; } = new BindingList<string>();

        public delegate string GetDisplayTextDelegate(object value);
        [Browsable(false)]
        public GetDisplayTextDelegate GetDisplayText { set; get; }

        public delegate object CreateNewItemDelegate(string itemType);
        [Browsable(false)]
        public CreateNewItemDelegate CreateNewItem { set; get; }

        private PropertyGrid _propertyGrid = null;
        private int _selectedIndex = -1;

        public CollectionListBox()
        {
            InitializeComponent();

            Utilities.SetCueBanner(addDropDown.Handle, "Add");

            if (AddDropDownItems != null)
            {
                addDropDown.Items.Clear();
                foreach (var item in AddDropDownItems)
                {
                    addDropDown.Items.Add(item);
                }
            }
        }

        public void AttachPropertyGrid(PropertyGrid propertyGrid)
        {
            _propertyGrid = propertyGrid;
            _propertyGrid.PropertyValueChanged += propertyGrid_PropertyValueChanged;
        }

        private void UpdateCollectionList()
        {
            if (_collection == null)
            {
                listBox.Items.Clear();
            }
            else
            {
                for (int k = 0; k < _collection.Count; k++)
                {
                    var item = _collection[k];
                    string displayText = item.ToString();
                    if (GetDisplayText != null)
                    {
                        displayText = GetDisplayText(item);
                    }

                    if (k < listBox.Items.Count)
                    {
                        listBox.Items[k] = displayText;
                    }
                    else
                    {
                        listBox.Items.Add(displayText);
                    }
                }

                for (int k = _collection.Count; k < listBox.Items.Count; k++)
                {
                    listBox.Items.RemoveAt(k);
                }
            }
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Ensure the item index is valid
            if (e.Index < 0) return;

            // Draw the background of the item.
            e.DrawBackground();

            var textRect = new Rectangle(e.Bounds.Left, e.Bounds.Top + 2, e.Bounds.Height - 4, e.Bounds.Height - 4);
            e.Graphics.FillRectangle(Brushes.LightGray, textRect);
            e.Graphics.DrawRectangle(Pens.DarkGray, textRect);

            // Get the item text
            string itemText = this.listBox.Items[e.Index].ToString();

            // Draw the item text
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(e.Index.ToString(), e.Font, Brushes.Black, textRect, format);


            textRect = new Rectangle(textRect.Left + textRect.Width, e.Bounds.Top, e.Bounds.Width - textRect.Width, e.Bounds.Height);
            e.Graphics.DrawString(itemText, e.Font, Brushes.Black, textRect, StringFormat.GenericDefault);

            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            _selectedIndex = listBox.SelectedIndex;
            if (_selectedIndex >= 0 && _selectedIndex < _collection.Count)
            {
                _collection.RemoveAt(_selectedIndex);
                UpdateCollectionList();
                //if (_selectedIndex >=0 && _selectedIndex < _collection.Count)
                //{
                //    ShowItemInPropertyGrid(_collection[_selectedIndex]);
                //}
                //else
                //{
                //    listBox.SelectedIndex = _selectedIndex - 1;
                //}
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            _selectedIndex = listBox.SelectedIndex;
            if (_selectedIndex > 0 && _selectedIndex < _collection.Count)
            {
                var item = _collection[_selectedIndex];
                _collection.RemoveAt(_selectedIndex);
                _collection.Insert(_selectedIndex - 1, item);
                UpdateCollectionList();
                listBox.SelectedIndex = _selectedIndex - 1;
            }
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            _selectedIndex = listBox.SelectedIndex;
            if (_selectedIndex >= 0 && _selectedIndex < _collection.Count - 1)
            {
                var item = _collection[_selectedIndex];
                _collection.RemoveAt(_selectedIndex);
                _collection.Insert(_selectedIndex + 1, item);
                UpdateCollectionList();
                listBox.SelectedIndex = _selectedIndex + 1;
            }
        }

        private void addDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (addDropDown.SelectedIndex >= 0)
            {
                if (CreateNewItem != null)
                {
                    var item = CreateNewItem(addDropDown.SelectedItem.ToString());
                    _collection.Add(item);
                    UpdateCollectionList();
                    listBox.SelectedIndex = _collection.Count - 1;
                }
                addDropDown.SelectedIndex = -1;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (CreateNewItem != null)
            {
                var item = CreateNewItem("");
                _collection.Add(item);
                UpdateCollectionList();
                listBox.SelectedIndex = _collection.Count - 1;
            }
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedIndex = listBox.SelectedIndex;

            if (_selectedIndex < 0 || _selectedIndex >= _collection.Count)
            {
                ShowItemInPropertyGrid(null);
            }
            else
            {
                ShowItemInPropertyGrid(_collection[_selectedIndex]);
            }
        }

        private void ShowItemInPropertyGrid(object newObject)
        {
            if (_propertyGrid == null) return;

            if (newObject == null)
            {
                _propertyGrid.SelectedObject = null;
                return;
            }

            _propertyGrid.SelectedObject = newObject;
            _propertyGrid.ExpandAllGridItems();
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _propertyGrid.Refresh();
            UpdateCollectionList();
        }

        private void addDropDown_VisibleChanged(object sender, EventArgs e)
        {
            if (AddDropDownItems != null)
            {
                addDropDown.Items.Clear();
                foreach (var item in AddDropDownItems)
                {
                    addDropDown.Items.Add(item);
                }
            }

        }
    }
}
