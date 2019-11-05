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

            var alternatives = questionsHtml.Select(x => GetAlternatives(x)).ToArray();
            Question[] questions = questionsHtml.Select(x => new Question() {
                Title = x.InnerHtml
            }).ToArray();
            //var content = new WebContent(questionsHtml.Count);

            return default;
        }

        public IEnumerator<string[]> GetAlternatives(IElement x)
        {
            yield return new string[0];
        }

    }
}
