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
    public class HeaderListBoxKeyHandler : KeyHandler
    {
        public ListBoxWrapper Paragraphs { get; set; }

        public HeaderListBoxKeyHandler(ListBoxWrapper headers, ListBoxWrapper paragraphs) : base(headers)
        {
            this.Paragraphs = paragraphs;
        }

        public override bool Handle(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.Paragraphs.FocusOnLineItem(this.Element.CurrentLineItem());

                    return true;
                case Key.P:
                    this.Element.FocusOnPrevLine();
                    this.Paragraphs.FocusOnLineItem(this.Element.CurrentLineItem());

                    return true;
                case Key.N:
                    this.Element.FocusOnNextLine();
                    this.Paragraphs.FocusOnLineItem(this.Element.CurrentLineItem());

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
