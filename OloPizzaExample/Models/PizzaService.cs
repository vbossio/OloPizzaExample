using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OloPizzaExample.Models
{
    public class PizzaService
    {
        public IEnumerable<PizzaCounts> GetMostPopularToppings(string sUrl, int topCount)
        {
            IEnumerable<PizzaCounts> finalList = null;
            var objReader = GetPizzaData(sUrl);
            if (!objReader.EndOfStream)
            {
                var pizzaData = JsonConvert.DeserializeObject(objReader.ReadToEnd());
                JArray pizzaArray = (JArray)pizzaData;

                // re-format array to list of pizza items with toppings as sorted, comma-delimited string
                List<Pizza> pizzaList = new List<Pizza>();
                foreach (var item in pizzaArray.Children())
                {
                    var itemProperties = item.Children<JProperty>();
                    var myElement = itemProperties.FirstOrDefault(x => x.Name == "toppings");
                    JArray tops = (JArray)myElement.Value;
                    var toppings = from t in tops
                                   orderby t
                                   select t;
                    var s = string.Join(",", toppings);
                    var p = new Pizza();
                    p.toppings = s;
                    pizzaList.Add(p);
                }

                // group by sorted, comma-delimited list of toppings
                finalList = pizzaList.GroupBy(p => p.toppings).OrderByDescending(p => p.Count()).Take(topCount)
                        .Select(group => new PizzaCounts { toppings = group.Key, count = group.Count() });
            }

            return finalList;
        }

        public StreamReader GetPizzaData(string sUrl)
        {
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sUrl);

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            return objReader;
        }
    }
}
