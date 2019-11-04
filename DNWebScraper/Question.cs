using System;
using System.Collections.Generic;
using System.Text;

namespace DNWebScraper
{
    public class Question
    {
        public string Title { get; set; }
        public string[] Alternatives { get; set; }
        public string CorrectAlternative { get; set; }

        public Question(string title, string[] alternatives, string correctAlternative)
        {

        }
    }
}
