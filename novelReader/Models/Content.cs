using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace novelReader.Models
{
    public class Content
    {
        public String Text { get; set; }
        public String Header { get; set; }
        public Boolean IsHeader { get; set; }

        public Content(String text)
        {
            this.Text = text.Trim();

            Match matchResult = ChapterMatcher.Exec(this.Text);

            this.Header = matchResult.Value;
            this.IsHeader = matchResult.Success;
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
