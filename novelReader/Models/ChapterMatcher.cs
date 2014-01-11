using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace novelReader.Models
{
    public class ChapterMatcher
    {
        static Regex chapterRegex = new Regex(@"(?:^\s*|^\s*第.*?)(第[^\s,.，。]*?[章篇]\s?.*)");

        public static Match Exec(string text)
        {
            return chapterRegex.Match(text);
        }
    }
}
