using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace novelReader.Utils
{
    public class HeaderListBoxKeyHandler : KeyHandler
    {
        public ListBox Paragraphs { get; set; }

        public HeaderListBoxKeyHandler(ListBox headers, ListBox paragraphs) : base(headers)
        {
            this.Paragraphs = paragraphs;
        }

        public override bool Handle(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.Paragraphs.SelectedItem = this.Element.SelectedItem;
                    this.Paragraphs.ScrollIntoView(this.Paragraphs.SelectedItem);

                    return true;
                case Key.P:
                    if (this.Element.SelectedIndex > 0)
                    {
                        this.Element.SelectedIndex -= 1;
                    }

                    this.Paragraphs.SelectedItem = this.Element.SelectedItem;
                    this.Paragraphs.ScrollIntoView(this.Paragraphs.SelectedItem);

                    return true;
                case Key.N:
                    if (this.Element.SelectedIndex < this.Element.Items.Count)
                    {
                        this.Element.SelectedIndex += 1;
                    }

                    this.Paragraphs.SelectedItem = this.Element.SelectedItem;
                    this.Paragraphs.ScrollIntoView(this.Paragraphs.SelectedItem);

                    return true;
                case Key.L:
                    this.Paragraphs.Focus();

                    return true;
                default: break;
            }

            return base.Handle(e);
        }
    }
}
