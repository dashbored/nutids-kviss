using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
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
            var content = new WebContent(10);

            //foreach (var term in QueryTerms)
            //{
            //    articleLink = document.All.Where(x => x.ClassName == "views-field views-field-nothing" && (x.ParentElement.InnerHtml.Contains(term) || x.ParentElement.InnerHtml.Contains(term.ToLower())));
            //}

            //if (articleLink.Any())
            //{
            //    // Print Results: See Next Step
            //}
            return content;
        }
    }
}
