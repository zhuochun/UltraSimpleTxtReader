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

        private double readingTime;
        private double readingSpeed;

        public Content(String text)
        {
            this.Text = text.Trim();

            Match matchResult = ChapterMatcher.Exec(this.Text);

            this.Header = matchResult.Value;
            this.IsHeader = matchResult.Success;
        }

        public double EstimateReadingTime(int speed)
        {
            if (speed == this.readingSpeed) {
                return this.readingTime;
            }

            this.readingSpeed = speed;
            this.readingTime = calReadingTime(this.Text.Length, speed) + calBufferTime(this.Text.Length, speed);

            return this.readingTime;
        }

        private double calReadingTime(int length, int speed)
        {
            return ((double)length / speed) * 1000.0;
        }

        private double calBufferTime(int length, int speed)
        {
            if (length > speed * 2)
            {
                return calReadingTime(length - speed, speed * 3);
            }
            else if (length < speed)
            {
                return calReadingTime(length, speed * 2);
            }
            else
            {
                return 0.0;
            }
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
