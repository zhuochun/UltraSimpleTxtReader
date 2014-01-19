using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace novelReader.Models
{
    public class Reader : ListBoxWrapper
    {
        public Reader(ListBox list) : base(list)
        {
            this.KeepLineInTop = true;
        }
    }
}
