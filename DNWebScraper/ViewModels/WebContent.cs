using System;
using System.Collections.Generic;
using System.Text;
using DNWebScraper.Models;

namespace DNWebScraper.ViewModels
{
    public class WebContent
    {
        public Question[] Questions { get; set; }

        public WebContent(int questions)
        {
            Questions = new Question[questions];
        }

        public WebContent(Question[] questions)
        {
            Questions = questions;
        }
    }
}
