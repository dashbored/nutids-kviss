using DNWebScraper;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var scraper = new WebScraperService();
            var result = await scraper.ScrapeWebsite("https://www.dn.se/nyheter/nutidstestet/nutidstestet-vecka-44-6/");
            Console.ReadLine();            
        }
    }
}
