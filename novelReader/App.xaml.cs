using novelReader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace novelReader
{
    public partial class App : Application
    {
        private ObservableCollection<Content> novel = new ObservableCollection<Content>();
        private ObservableCollection<Content> headers = new ObservableCollection<Content>();

        public ObservableCollection<Content> Novel {
            get { return this.novel; }
            set { this.novel = value; }
        }

        public ObservableCollection<Content> Headers {
            get { return this.headers; }
            set { this.headers = value; }
        }
    }
}
