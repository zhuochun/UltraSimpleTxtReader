using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace novelReader.Models
{
    public abstract class ListBoxWrapper
    {
        protected ListBox list;
        public bool KeepLineInTop { get; set; }

        public ListBoxWrapper(ListBox list)
        {
            this.list = list;
            this.KeepLineInTop = true;
        }

        private void gotoLine(int num)
        {
            this.list.SelectedIndex = num;
            this.list.ScrollIntoView(this.list.SelectedItem);
        }

        private void gotoLineItem(object item)
        {
            this.list.SelectedItem = item;
            this.list.ScrollIntoView(this.list.SelectedItem);
        }

        public void FocusOnFirstLine()
        {
            this.gotoLine(0);
        }

        public void FocusOnLastLine()
        {
            this.gotoLine(this.LineCount() - 1);
        }

        public void FocusOnPrevLine()
        {
            if (this.IsAtFirstLine()) return;

            this.gotoLine(this.CurrentLine() - 1);
        }

        public void FocusOnNextLine()
        {
            if (this.IsAtLastLine()) return;

            int nextLine = this.CurrentLine() + 1;

            if (this.KeepLineInTop)
            {
                this.FocusOnLastLine();
                this.Refresh();
            }

            this.gotoLine(nextLine);
        }

        public void FocusOnLine(int num) {
            if (this.KeepLineInTop)
            {
                this.FocusOnLastLine();
                this.Refresh();
            }

            this.gotoLine(num);
        }

        public void FocusOnLineItem(object item)
        {
            if (this.KeepLineInTop)
            {
                this.FocusOnLastLine();
                this.Refresh();
            }

            this.gotoLineItem(item);
        }

        public void Focus() {
            this.list.Focus();
        }

        public Content CurrentLineItem()
        {
            return (Content)this.list.SelectedItem;
        }

        public int CurrentLine()
        {
            return this.list.SelectedIndex;
        }

        public int LineCount()
        {
            return this.list.Items.Count;
        }

        public bool IsAtFirstLine()
        {
            return this.CurrentLine() == 0;
        }

        public bool IsAtLastLine()
        {
            return this.CurrentLine() + 1 == this.LineCount();
        }

        public void Refresh()
        {
            this.list.UpdateLayout();
        }
    }
}
