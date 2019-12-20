using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DNWebScraper.Models;
using DNWebScraper.ViewModels;

namespace DNWebScraper.Services
{
    public class WebScraperService
    {
        private string Title { get; set; }
        private string URL { get; set; }

        public async Task<WebContent> ScrapeWebsite(string siteUrl)
        {
            var cancellationToken = new CancellationTokenSource();
            var httpClient = new HttpClient();
            var request = await httpClient.GetAsync(siteUrl, cancellationToken.Token);
            cancellationToken.Token.ThrowIfCancellationRequested();

            var response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            var parser = new HtmlParser();
            var doc = parser.ParseDocument(response);
            return GetScrapeResults(doc);
        }

        private WebContent GetScrapeResults(IHtmlDocument document)
        {
            var questionsHtml = document.All.Where(x => x.Id != null && x.Id.StartsWith("quiz-question-")).ToList();

            var questionForm = questionsHtml.Select(x => x.ParentElement).ToList();

            var alternatives = questionForm.Select(x => x.Children.Where(y => y.LocalName.Equals("label"))
                                                                  .Select(y => y.InnerHtml)
                                                                  .Select(y => y.Substring(y.IndexOf("<span>") + 6))
                                                                  .Select(y => y.Substring(0, y.IndexOf("</span>")))
                                                                  .ToArray()).ToList();

            var correctAnswers = document.All.Where(x => x.HasAttribute("data-question-number"))
                                                        .Select(x => x.Children.First(y => y.LocalName.Equals("div")))
                                                        .Select(x => x.Children.First(y => y.ClassList.Length == 0))
                                                        .Select(x => x.TextContent)
                                                        .Select(x => x.Substring(x.IndexOf("Rätt svar:") + 11))
                                                        .Select(x => x.Substring(0, x.IndexOf("\n"))).ToList();
            
            Question[] questions = questionsHtml.
                Zip(alternatives, (x, y) => new Question()
                {
                    Title = x.InnerHtml,
                    Alternatives = y
                }).ToArray();

            for (int i = 0; i < questions.Length; i++)
            {
                questions[i].QuestionNumber = i + 1;
                questions[i].CorrectAlternative = correctAnswers[i];
            }
            var content = new WebContent(questions);

            return content;
        }
    }
}
