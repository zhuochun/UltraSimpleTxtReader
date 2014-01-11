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
        static Regex chapterRegex = new Regex(@"(\b第.*[章篇]\b.*)");

        public static Match Exec(string text)
        {
            return chapterRegex.Match(text);
        }
    }
}
