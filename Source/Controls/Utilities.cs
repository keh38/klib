﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KLib.Controls
{
    public static class Utilities
    {
        public static void SetEnumItems(CheckedListBox box, Type t)
        {
            box.Items.Clear();
            foreach (var value in Enum.GetValues(t))
            {
                box.Items.Add(Enum.GetName(t, value));
            }
        }

        public static void SetCheckedEnumItems<T>(CheckedListBox box, List<T> items)
        {
            for (int k = 0; k < box.Items.Count; k++) box.SetItemChecked(k, false);
            foreach (var e in items)
            {
                box.SetItemChecked(Convert.ToInt32(e), true);
            }
        }

        public static List<T> GetCheckedEnumItems<T>(CheckedListBox box)
        {
            List<T> checkedItems = new List<T>();
            foreach (var i in box.CheckedIndices)
            {
                checkedItems.Add((T)i);
            }
            return checkedItems;
        }

    }
}