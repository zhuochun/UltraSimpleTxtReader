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
    public class ParagraphListBoxKeyHandler : KeyHandler
    {
        public ListBoxWrapper Headers { get; set; }

        public ParagraphListBoxKeyHandler(ListBoxWrapper paragraphs, ListBoxWrapper headers) : base(paragraphs)
        {
            this.Headers = headers;
        }

        public override bool Handle(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.P:
                    this.Headers.FocusOnPrevLine();
                    this.Element.FocusOnLineItem(this.Headers.CurrentLineItem());

                    return true;
                case Key.N:
                    this.Headers.FocusOnNextLine();
                    this.Element.FocusOnLineItem(this.Headers.CurrentLineItem());

                    return true;
                case Key.T:
                    this.Headers.FocusOnFirstLine();
                    this.Element.FocusOnLineItem(this.Headers.CurrentLineItem());

                    return true;
                case Key.G:
                    this.Headers.FocusOnLastLine();
                    this.Element.FocusOnLineItem(this.Headers.CurrentLineItem());

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
