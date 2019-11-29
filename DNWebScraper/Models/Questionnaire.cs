using System;
using System.Collections.Generic;
using System.Text;

namespace DNWebScraper.Models
{
    public class Questionnaire
    {
        public List<string> Answers { get; set; }

        public Questionnaire()
        {
            Answers = new List<string>();
        }
    }
}
