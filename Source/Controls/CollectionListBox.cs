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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static KLib.Controls.KUserListBox;

namespace KLib.Controls
{
#if !NET48
    [SupportedOSPlatform("windows")]
#endif
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

        public bool ShowUpDownButtons
        {
            get { return upButton.Visible; }
            set { upButton.Visible = value; downButton.Visible = value; }
        }

        public bool ShowAddDropDown
        {
            get { return addDropDown.Visible; }
            set { addDropDown.Visible = value; addButton.Visible = !value; }
        }

        public bool Alphabetize
        {
            get { return listBox.Sorted; }
            set { listBox.Sorted = value; }
        }

        public BindingList<string> AddDropDownItems { get; set; } = new BindingList<string>();

        public delegate string GetDisplayTextDelegate(object value);
        [Browsable(false)]
        public GetDisplayTextDelegate GetDisplayText { set; get; }

        public delegate object CreateNewItemDelegate(object item);
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
                var removed = _collection[_selectedIndex];
                _collection.Remove(removed);
                UpdateCollectionList();
                OnItemRemoved(removed);
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
                    OnItemAdded(item);
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

                OnItemAdded(item);
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
                OnSelectedItemChanged(_collection[_selectedIndex]);
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

            if (e.ChangedItem.Label == "Name")
            {
                OnItemRenamed(e.OldValue as string, e.ChangedItem.Value as string);
            }
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

        public event EventHandler<RenamedItem> ItemRenamed;
        private void OnItemRenamed(string oldName, string newName)
        {
            ItemRenamed?.Invoke(this, new RenamedItem(oldName, newName));
        }

        public class RenamedItem : EventArgs
        {
            public string oldName;
            public string newName;
            public RenamedItem(string oldName, string newName)
            {
                this.oldName = oldName;
                this.newName = newName;
            }
        }

        public event EventHandler<ChangedItem> ItemAdded;
        private void OnItemAdded(object item)
        {
            ItemAdded?.Invoke(this, new ChangedItem(item));
        }

        public event EventHandler<ChangedItem> ItemRemoved;
        private void OnItemRemoved(object item)
        {
            ItemRemoved?.Invoke(this, new ChangedItem(item));
        }

        public event EventHandler<ChangedItem> SelectedItemChanged;
        private void OnSelectedItemChanged(object item)
        {
            SelectedItemChanged?.Invoke(this, new ChangedItem(item));
        }

        public class ChangedItem : EventArgs
        {
            public object item;
            public ChangedItem(object item) { this.item = item; }
        }

        private void CollectionListBox_Resize(object sender, EventArgs e)
        {
            int nominalButtonWidth = 100;

            int x = this.Bounds.Width;

            int xButton = this.Bounds.Width;
            if (upButton.Visible)
            {
                xButton = xButton
                - upButton.Width
                - upButton.Margin.Right;

                upButton.Left = xButton;
                downButton.Left = xButton;
            }

            listBox.Width = xButton
                - listBox.Margin.Left
                - listBox.Margin.Right;

            if (upButton.Visible)
            {
                listBox.Width -= upButton.Margin.Left;
            }

            int xRemove = listBox.Right - nominalButtonWidth;

            bool twoRows = false;
            if (xRemove - removeButton.Margin.Left < addButton.Left + nominalButtonWidth + addButton.Margin.Right)
            {
                twoRows = true;
                removeButton.Left = listBox.Left;
                removeButton.Width = listBox.Width;

                addButton.Left = listBox.Left;
                addButton.Width = listBox.Width;
                addDropDown.Left = listBox.Left;
                addDropDown.Width = listBox.Width;
            }
            else
            {
                removeButton.Left = xRemove;
                removeButton.Width = nominalButtonWidth;

                addButton.Left = listBox.Left;
                addButton.Width = nominalButtonWidth;
                addDropDown.Left = listBox.Left;
                addDropDown.Width = nominalButtonWidth;
            }

            int nominalListBoxBottom = this.Bounds.Height
                - removeButton.Margin.Bottom
                - removeButton.Height
                - removeButton.Margin.Top
                - listBox.Margin.Bottom;

            if (twoRows)
            {
                nominalListBoxBottom = nominalListBoxBottom
                    - addButton.Margin.Bottom
                    - addButton.Height
                    - addButton.Margin.Top;
            }

            listBox.Height = nominalListBoxBottom - listBox.Top;

            int yButton = listBox.Bottom
                + listBox.Margin.Bottom
                + removeButton.Margin.Top;

            addButton.Top = yButton;
            addDropDown.Top = yButton;

            if (twoRows)
            {
                yButton += addButton.Margin.Bottom + addButton.Height + removeButton.Margin.Top;
            }
            removeButton.Top = yButton;

            this.Height = yButton + removeButton.Height + removeButton.Margin.Bottom;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.D))
            {
                //true if key was processed by control, false otherwise
                return DuplicateItem();
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private bool DuplicateItem()
        {
            if (CreateNewItem == null) return false;

            _selectedIndex = listBox.SelectedIndex;
            if (_selectedIndex >= 0 && _selectedIndex < _collection.Count)
            {
                var item = _collection[_selectedIndex];
                var duplicate = CreateNewItem(item);


                _collection.Insert(_selectedIndex + 1, duplicate);
                UpdateCollectionList();
                listBox.SelectedIndex = _selectedIndex + 1;

                OnItemAdded(item);
                return true;
            }
            return false;
        }
    }
}
