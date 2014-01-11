using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace novelReader.Utils
{
    public class KeyHandler
    {
        public ListBox Element { get; set; }

        public KeyHandler(ListBox e)
        {
            this.Element = e;
        }

        public virtual bool Handle(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.J:
                    if (this.Element.SelectedIndex < this.Element.Items.Count)
                    {
                        this.Element.SelectedIndex += 1;
                    }

                    this.Element.ScrollIntoView(this.Element.SelectedItem);

                    return true;
                case Key.K:
                    if (this.Element.SelectedIndex > 0)
                    {
                        this.Element.SelectedIndex -= 1;
                    }

                    this.Element.ScrollIntoView(this.Element.SelectedItem);

                    return true;
                case Key.T:
                    this.Element.SelectedItem = this.Element.Items[0];
                    this.Element.ScrollIntoView(this.Element.SelectedItem);

                    return true;
                case Key.G:
                    this.Element.SelectedItem = this.Element.Items[this.Element.Items.Count - 1];
                    this.Element.ScrollIntoView(this.Element.SelectedItem);

                    return true;
                default: break;
            }

            return false;
        }
    }
}
