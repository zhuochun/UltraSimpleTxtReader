using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace novelReader.Utils
{
    public class ParagraphListBoxKeyHandler : KeyHandler
    {
        public ListBox Headers { get; set; }

        public ParagraphListBoxKeyHandler(ListBox paragraphs, ListBox headers) : base(paragraphs)
        {
            this.Headers = headers;
        }

        public override bool Handle(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.P:
                    if (this.Headers.SelectedIndex > 0)
                    {
                        this.Headers.SelectedIndex -= 1;
                    }

                    this.Element.SelectedItem = this.Headers.SelectedItem;
                    this.Element.ScrollIntoView(this.Element.SelectedItem);

                    return true;
                case Key.N:
                    if (this.Headers.SelectedIndex < this.Headers.Items.Count)
                    {
                        this.Headers.SelectedIndex += 1;
                    }

                    this.Element.SelectedItem = this.Headers.SelectedItem;
                    this.Element.ScrollIntoView(this.Element.SelectedItem);

                    return true;
                case Key.T:
                    Object firstItem = this.Headers.Items[0];

                    this.Element.SelectedItem = firstItem;
                    this.Element.ScrollIntoView(this.Element.SelectedItem);

                    return true;
                case Key.G:
                    Object lastItem = this.Headers.Items[this.Headers.Items.Count - 1];

                    this.Element.SelectedItem = lastItem;
                    this.Element.ScrollIntoView(this.Element.SelectedItem);

                    return true;
                case Key.H:
                    this.Headers.Focus();

                    return true;
                default: break;
            }

            return base.Handle(e);
        }
    }
}
