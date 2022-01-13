using currencyConvertor.Models;
using Newtonsoft.Json.Linq;

namespace currencyConvertor.Controllers;

public static class RequestHelper
    {
        public const string FreeBaseUrl = "https://free.currconv.com/api/v7/";
        //public const string FreeBaseUrl = "https://free.currencyconverterapi.com/api/v6/";
        //public const string PremiumBaseUrl = "https://api.currencyconverterapi.com/api/v6/";

        public static async Task<List<Currency>> GetAllCurrencies(string apiKey = null)
        {
            var url = $"{FreeBaseUrl}currencies?apiKey={apiKey}";
            /*if (!string.IsNullOrEmpty(apiKey))
                url = FreeBaseUrl + "currencies";
            else
                url = PremiumBaseUrl + "currencies" + "?apiKey=" + apiKey;*/

            var jsonString =await GetResponse(url);

            var data = JObject.Parse(jsonString)["results"]?.ToArray();
            return data.Select(item => item.First.ToObject<Currency>()).ToList();
        }

        public static async Task<List<Country>> GetAllCountries(string apiKey = null)
        {
            var url = $"{FreeBaseUrl}countries?apiKey={apiKey}";
            /*if (string.IsNullOrEmpty(apiKey))
                url = FreeBaseUrl + "countries";
            else
                url = PremiumBaseUrl + "countries" + "?apiKey=" + apiKey;*/

            var jsonString =await GetResponse(url);

            var data = JObject.Parse(jsonString)["results"]?.ToArray();

            return data?.Select(item => item.First.ToObject<Country>()).ToList();
        }

        public static async Task<List<CurrencyHistory>> GetHistoryRange(CurrencyType from, CurrencyType to, string startDate, string endDate, string apiKey = null)
        {
            var url = $"{FreeBaseUrl}convert?q={from}_{to}&compact=ultra&date={startDate}&endDate={endDate}&apiKey={apiKey}";
            /*if (string.IsNullOrEmpty(apiKey))
                url = FreeBaseUrl + "convert?q=" + from + "_" + to + "&compact=ultra&date=" + startDate + "&endDate=" + endDate;
            else
                url = PremiumBaseUrl + "convert?q=" + from + "_" + to + "&compact=ultra&date=" + startDate + "&endDate=" + endDate + "&apiKey=" + apiKey;*/

            var jsonString =await GetResponse(url);
            var data = JObject.Parse(jsonString).First.ToArray();
            return (from item in data
                    let obj = (JObject)item
                    from prop in obj.Properties()
                    select new CurrencyHistory
                    {
                        Date = prop.Name,
                        ExchangeRate = item[prop.Name].ToObject<double>()
                    }).ToList();
        }

        public static async Task<CurrencyHistory> GetHistory(CurrencyType from, CurrencyType to, string date, string apiKey = null)
        {
            var url = $"{FreeBaseUrl}convert?q={from}_{to}&compact=ultra&date={date}&apiKey={apiKey}";
            /*if (string.IsNullOrEmpty(apiKey))
                url = FreeBaseUrl + "convert?q=" + from + "_" + to + "&compact=ultra&date=" + date;
            else
                url = PremiumBaseUrl + "convert?q=" + from + "_" + to + "&compact=ultra&date=" + date + "&apiKey=" + apiKey;*/

            var jsonString = await GetResponse(url);
            var data = JObject.Parse(jsonString);
            return data.Properties().Select(prop => new CurrencyHistory
            {
                Date = prop.Name,
                ExchangeRate = data[prop.Name][date].ToObject<double>()
            }).FirstOrDefault();
        }

        public static async Task<double> ExchangeRate(string from, string to, string? apiKey = null)
        {
            var url = $"{FreeBaseUrl}convert?q={from}_{to}&compact=y&apiKey={apiKey}";
            /*if (string.IsNullOrEmpty(apiKey))
                url = FreeBaseUrl + "convert?q=" + from + "_" + to + "&compact=y";
            else
                url = PremiumBaseUrl + "convert?q=" + from + "_" + to + "&compact=y&apiKey=" + apiKey;*/

            var jsonString =await GetResponse(url);
            return JObject.Parse(jsonString).First.First["val"].ToObject<double>();
        }

        private static async Task<string> GetResponse(string url)
        {
            string jsonString;
            
            using var client = new HttpClient();
            var stream= await client.GetStreamAsync(url);
            /*var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using var response = (HttpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();*/
            using var reader = new StreamReader(stream);
            jsonString = await reader.ReadToEndAsync();

            return jsonString;
        }
    }