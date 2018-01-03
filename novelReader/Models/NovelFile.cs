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
                return;
            }

            FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(fs);
            if (sr.CurrentEncoding == Encoding.UTF8)
            {
                var chArr = new char[1024];
                sr.Read(chArr, 0, chArr.Length);
                var buffer1 = Encoding.UTF8.GetBytes(chArr);
                var buffer2 = new byte[buffer1.Length];
                fs.Position = 0;
                fs.Read(buffer2, 0, buffer2.Length);
                var same = true;
                for (int i = 0; i < buffer1.Length; i++)
                {
                    if (buffer1[i] != buffer2[i])
                    {
                        same = false;
                        break;
                    }
                }
                if (!same)
                {
                    fs.Position = 0;
                    sr = new StreamReader(fs, Encoding.GetEncoding("GBK"));
                }
            }
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    novel.Add(new Content(line));
                }
            }
            sr.Dispose();
        }
    }
}
