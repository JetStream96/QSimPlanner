﻿using System.Collections.Generic;
using System.Windows.Forms;

namespace QSP.UI.Util
{
    public class ListViewSortEnabler
    {
        private ListView view;
        private int lastColumn = -1;

        public ListViewSortEnabler(ListView view)
        {
            this.view = view;
        }

        public void EnableSort()
        {
            view.ColumnClick += Sort;
        }

        private void Sort(object sender, ColumnClickEventArgs e)
        {
            int col = e.Column;

            if (col == lastColumn)
            {
                view.Sorting = InvertOrder(view.Sorting);
            }
            else
            {
                view.Sorting = SortOrder.Ascending;
            }

            view.ListViewItemSorter = Comparer<ListViewItem>.Create((x, y) =>
            {
                var txtX = x.SubItems[col].Text;
                var txtY = y.SubItems[col].Text;
                int invert = view.Sorting == SortOrder.Ascending ? 1 : -1;

                return txtX.CompareTo(txtY) * invert;
            });

            lastColumn = col;
            view.Sort();
        }

        private static SortOrder InvertOrder(SortOrder order)
        {
            if (order == SortOrder.None || order == SortOrder.Descending)
            {
                return SortOrder.Ascending;
            }

            return SortOrder.Descending;
        }
    }
}
