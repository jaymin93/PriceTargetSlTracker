using HtmlAgilityPack;
using System.Diagnostics;
using System.Xml;

namespace PriceTargetSlTracker
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            List<BTST> btsts = new List<BTST>()
            {
                new BTST() { symbol = "RUSHIL", buyprice = 318,isActive = true,SL= 309,TargetList = new List<decimal>(){312,315,320,325,330,340 } },
                //new BTST() { symbol = "RUSHIL", buyprice = 380,isActive = true,SL= 309,TargetList = new List<decimal>(){312,315,320,325,330,340 } }

            };

            while (true)
            {
                foreach (var item in btsts)
                {
                    var cmpOfSymbol = await GetCurrentRateofStock(item.symbol);
                    item.cmp = cmpOfSymbol;
                    ISSLHitOrNearBy(item);
                    var nearByTargetValue = FindNearestTagrgetValue(item.TargetList, cmpOfSymbol);
                    IsTaregtHit(item,cmpOfSymbol, nearByTargetValue);
                    Console.WriteLine($"price of {item.symbol} has changed {CalculatePercentageChange(item.buyprice, cmpOfSymbol)} ") ;
                }
            }
        }

        public static bool ISSLHitOrNearBy(BTST btst)
        {
            if (btst == null) return false;

            if (btst.cmp - btst.SL <= 5)
            {
                WriteToConsole($"{btst.symbol} is near by to SL Or SL Is already hit please look at it and take action", ConsoleColor.Red);
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
# if DEBUG
                Stopwatch sw = new();
                sw.Start();
#endif
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
#if DEBUG
                        sw.Stop();
                        Console.WriteLine($"{Environment.NewLine}current price for {symbol} is {price} time took {sw.ElapsedMilliseconds} seconds {Environment.NewLine}");
#endif
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
            if (consoleColor == ConsoleColor.Red)
            {
                Console.Beep();
            }
        }

        public static void IsTaregtHit(BTST bTST, decimal cmp,decimal NearByTarget)
        {
                if (NearByTarget - cmp <= 5)
                {
                    WriteToConsole($"{bTST.symbol} is near by to Target {NearByTarget}", ConsoleColor.Green);
                }
        }

        static decimal FindNearestTagrgetValue(List<decimal> values, decimal cmp)
        {
            decimal nearestValue = values[0];
            decimal smallestDifference = Math.Abs(cmp - nearestValue);

            foreach (decimal value in values)
            {
                decimal difference = Math.Abs(cmp - value);

                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    nearestValue = value;
                }
            }

            return nearestValue;
        }

        static decimal CalculatePercentageChange(decimal oldValue, decimal newValue)
        {
            decimal changePercentage = ((newValue - oldValue) / oldValue) * 100;
            return changePercentage;
        }

    }
}
