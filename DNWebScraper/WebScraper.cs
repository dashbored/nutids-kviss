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

namespace DNWebScraper
{
    public class WebScraper
    {
        private string Title { get; set; }
        private string URL { get; set; }
        //private string siteUrl = "https://www.dn.se/nyheter/nutidstestet/nutidstestet-vecka-44-6/";

        public async Task<WebContent> ScrapeWebsite(string siteUrl)
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(siteUrl);
            cancellationToken.Token.ThrowIfCancellationRequested();

            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            HtmlParser parser = new HtmlParser();
            var doc =  parser.ParseDocument(response);
            return GetScrapeResults(doc);
        }

        private WebContent GetScrapeResults(IHtmlDocument document)
        {
            List<IElement> questionsHtml = document.All.Where(x => x.Id != null && x.Id.StartsWith("quiz-question-")).ToList();
            List<IElement> questionForm = questionsHtml.Select(x => x.ParentElement).ToList();

            List<string> correctAnswers = document.All.Where(x => x.HasAttribute("data-question-number"))
                                                        .Select(x => x.Children.First(y => y.LocalName.Equals("div")))
                                                        .Select(x => x.Children.First(y => y.ClassList.Length == 0))
                                                        .Select(x => x.TextContent)
                                                        .Select(x => x.Substring(x.IndexOf("Rätt svar:") + 10))
                                                        .Select(x => x.Substring(0, x.IndexOf("\n"))).ToList();
            var alternatives = questionForm.Select(x => x.Children.Where(y => y.LocalName.Equals("label")).ToList()).ToList();

            Question[] questions = questionsHtml.Zip(alternatives, (x, y) => new Question() {
                Title = x.InnerHtml,
                Alternatives = y.Select(z => z.InnerHtml).ToArray()
            }).ToArray();
            var content = new WebContent(questions);

            return content;
        }

        public IEnumerator<string[]> GetAlternatives(IElement x)
        {
            yield return new string[0];
        }

    }
}
