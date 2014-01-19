using novelReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace novelReader.Utils
{
    public abstract class KeyHandler
    {
        public ListBoxWrapper Element { get; set; }

        public KeyHandler(ListBoxWrapper e)
        {
            this.Element = e;
        }

        public virtual bool Handle(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.J:
                    this.Element.FocusOnNextLine();

                    return true;
                case Key.K:
                    this.Element.FocusOnPrevLine();

                    return true;
                case Key.T:
                    this.Element.FocusOnFirstLine();

                    return true;
                case Key.G:
                    this.Element.FocusOnLastLine();

                    return true;
                default: break;
            }

            return false;
        }
    }
}
