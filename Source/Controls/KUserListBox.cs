using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLib.Controls
{
    public partial class KUserListBox : KUserControl
    {
        public KUserListBox()
        {
            InitializeComponent();
        }

        public void SetItems(List<string> items)
        {
            listBox.Items.Clear();
            listBox.Items.AddRange(items.ToArray());
        }

        public void Clear()
        {
            listBox.Items.Clear();
        }

        public string DefaultName { set; get; } = "Event";

        public int SelectedIndex
        {
            get { return listBox.SelectedIndex; }
            set
            {
                _ignoreEvents = true;
                listBox.SelectedIndex = value;
                _ignoreEvents = false;
            }
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectionChanged(new ChangedItem(listBox.SelectedItem as string, listBox.SelectedIndices.Count == 1 ? listBox.SelectedIndex : -1));
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string newName = FindNewName();
            if (listBox.SelectedIndex >= 0)
            {
                listBox.Items.Insert(listBox.SelectedIndex, newName);
            }
            else
            {
                listBox.Items.Add(newName);
            }

            int index = listBox.Items.IndexOf(newName);
            OnItemAdded(new ChangedItem(newName, index));
            EditEntry(index);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            List<string> deletedItems = listBox.SelectedItems.Cast<string>().ToList();

            foreach (string item in deletedItems)
            {
                listBox.Items.Remove(item);
            }
            OnItemsDeleted(new ChangedItems(deletedItems));
        }

        private void sortButton_Click(object sender, EventArgs e)
        {
            List<string> items = listBox.Items.Cast<string>().ToList();
            items.Sort();

            listBox.Items.Clear();
            foreach (string i in items) listBox.Items.Add(i);

            OnItemsMoved(new ChangedItems(items));
        }

        private string FindNewName()
        {
            List<string> items = listBox.Items.Cast<string>().ToList();
            int num = items.Count + 1;
            string name = DefaultName + (num > 1 ? " " + num : "");
            while (items.Find(i => i == name) != null) name = DefaultName + " " + (++num);

            return name;
        }

        private void editBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                editBox.Hide();
                e.Handled = true;
            }
            else if (e.KeyChar == (char)13)
            {
                listBox.Items[(int) editBox.Tag] = editBox.Text;
                editBox.Hide();
                e.Handled = true;
                listBox.SelectedIndex = (int)editBox.Tag;
                OnItemRenamed(new ChangedItem(editBox.Text, (int)editBox.Tag));
            }
            else base.OnKeyPress(e);
        }

        private void editBox_Leave(object sender, EventArgs e)
        {
            editBox.Hide();
        }

        private void listBox_KeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.Alt && listBox.SelectedIndices.Count == 1)
            {
                if (e.KeyCode == Keys.Up)
                    SwapItems(listBox.SelectedIndex, 1);
                else if (e.KeyCode == Keys.Down)
                    SwapItems(listBox.SelectedIndex, -1);
            }
        }

        private void SwapItems(int index, int delta)
        {
            object item = listBox.Items[index];
            if (delta > 0 && index > 0)
            {
                listBox.Items.RemoveAt(index);
                listBox.Items.Insert(--index, item);
                listBox.SelectedIndex = index;
                OnItemMoved(new ChangedItem((item as string), index));
            }
            if (delta < 0 && index < listBox.Items.Count-1)
            {
                listBox.Items.RemoveAt(index);
                listBox.Items.Insert(++index, item);
                listBox.SelectedIndex = index;
                OnItemMoved(new ChangedItem((item as string), index));
            }
        }

        private void listBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox.SelectedIndex >= 0) EditEntry(listBox.SelectedIndex);
        }

        private void EditEntry(int index)
        {
            int delta = 1;

            listBox.ClearSelected();

            Rectangle r = listBox.GetItemRectangle(index);
            string itemText = (string)listBox.Items[index];
            editBox.Location = new Point(r.X + delta, r.Y + delta + listBox.Location.Y);
            editBox.Size = new Size(r.Width - 5, r.Height - delta);
            //listBox.Controls.AddRange(new System.Windows.Forms.Control[] { this.editBox });
            editBox.Show();

            editBox.Tag = index;
            editBox.Text = itemText;
            editBox.SelectAll();
            editBox.Focus();
        }

        #region Events
        public event EventHandler<ChangedItem> SelectionChanged;
        private void OnSelectionChanged(ChangedItem itemChanged)
        {
            if (SelectionChanged != null) SelectionChanged(this, itemChanged);
        }

        public event EventHandler<ChangedItem> ItemAdded;
        private void OnItemAdded(ChangedItem itemChanged)
        {
            if (ItemAdded != null) ItemAdded(this, itemChanged);
        }

        public event EventHandler<ChangedItem> ItemRenamed;
        private void OnItemRenamed(ChangedItem itemChanged)
        {
            if (ItemRenamed != null) ItemRenamed(this, itemChanged);
        }

        public event EventHandler<ChangedItem> ItemMoved;
        private void OnItemMoved(ChangedItem itemChanged)
        {
            if (ItemMoved != null) ItemMoved(this, itemChanged);
        }

        public event EventHandler<ChangedItems> ItemsDeleted;
        private void OnItemsDeleted(ChangedItems items)
        {
            if (ItemsDeleted != null) ItemsDeleted(this, items);
        }

        public event EventHandler<ChangedItems> ItemsMoved;
        private void OnItemsMoved(ChangedItems items)
        {
            if (ItemsMoved != null) ItemsMoved(this, items);
        }

        public class ChangedItem : EventArgs
        {
            public string name;
            public int index;
            public ChangedItem(string name, int index)
            {
                this.name = name;
                this.index = index;
            }
        }

        public class ChangedItems : EventArgs
        {
            public List<string> names;
            public ChangedItems(List<string> names)
            {
                this.names = names;
            }
        }

        #endregion

    }
}
