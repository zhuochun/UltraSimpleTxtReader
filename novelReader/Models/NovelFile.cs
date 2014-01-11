using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace novelReader.Models
{
    public class NovelFile
    {
        public static void Open(String path, ObservableCollection<Content> novel)
        {
            if (String.IsNullOrEmpty(path))
            {
                return ;
            }

            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs, Encoding.Default))
            {
                String line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        novel.Add(new Content(line));
                    }
                }
            }
        }
    }
}
