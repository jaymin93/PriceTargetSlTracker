using HtmlAgilityPack;
using System.Diagnostics;
using System.Xml;

namespace PriceTargetSlTracker
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            List<BTST> btsts = new List<BTST>() { new BTST() { symbol = "RUSHIL", buyprice = 380,isActive = true,SL= 309,Tgt1=312,Tgt2=315,Tgt3=320,Tgt4=325, Tgt5=330,Tgt6= 340 }, new BTST() { symbol = "DLINKINDIA", buyprice = 330, isActive = true, SL = 300, Tgt1 = 349, Tgt2 = 350, Tgt3 = 360, Tgt4 = 375, Tgt5 = 380, Tgt6 = 390 } };

            while (true)
            {
                foreach (var item in btsts)
                {
                    var cmpOfSymbol =await GetCurrentRateofStock(item.symbol);
                    item.cmp = cmpOfSymbol;
                    ISSLHitOrNearBy(item);
                    IsTaregtHit(item, item.Tgt1, 1);
                    IsTaregtHit(item, item.Tgt2, 2);
                    IsTaregtHit(item, item.Tgt3, 3);
                    IsTaregtHit(item, item.Tgt4, 4);
                    IsTaregtHit(item, item.Tgt5, 5);
                    IsTaregtHit(item, item.Tgt6, 6);
                }
                await Task.Delay(200);
            }
        }

        public static bool ISSLHitOrNearBy(BTST btst)
        {
            if (btst == null) return false;

            if (btst.cmp - btst.SL <= 5)
            {
                WriteToConsole($"{btst.symbol} is near by to SL Or SL Is already hit please look at it and take action",ConsoleColor.Red);
            return true;
            }
            else
            {
                
                return false;
            }
        }
        
        public static async Task<decimal> GetCurrentRateofStock(string symbol)
        {
            try
            {
                Stopwatch sw = new();
                sw.Start();
                string url = $"https://www.google.com/finance/quote/{symbol}:NSE?hl=en";

                using (HttpClient client = new())
                {
                    string html = await client.GetStringAsync(new Uri(url));

                    HtmlDocument doc = new();
                    doc.LoadHtml(html);
                    HtmlNode priceNode = doc.DocumentNode.SelectSingleNode("//div[@class='YMlKec fxKbKc']");
                    string priceText = priceNode?.InnerText?.Trim().Replace("₹", "");

                    if (decimal.TryParse(priceText, out decimal price))
                    {
                        sw.Stop();
                        Console.WriteLine($"{Environment.NewLine}current price for {symbol} is {price} time took {sw.ElapsedMilliseconds} seconds {Environment.NewLine}");
                        return price;
                    }
                    else
                    {
                        return default;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default;
            }
        }

        public static void WriteToConsole(string TextToWrite, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine($"{Environment.NewLine}{TextToWrite}{Environment.NewLine}");
            Console.ResetColor();
            if (consoleColor ==  ConsoleColor.Red)
            {
                Console.Beep();
            }
        }

        public static void IsTaregtHit(BTST btst, decimal targertToCompare,int targerNum)
        {
            if (targertToCompare!= default && targertToCompare - btst.cmp <= 5)
            {
                WriteToConsole($"{btst.symbol} is near by to Target {targerNum}", ConsoleColor.Green);
            }
        }
   
    }
}
