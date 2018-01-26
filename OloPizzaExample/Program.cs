using System;
using System.Collections.Generic;

using OloPizzaExample.Models;

namespace OloPizzaExample
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string sUrl = "http://files.olo.com/pizzas.json";
            IEnumerable<PizzaCounts> finalList = null;

            var ps = new PizzaService();
            try
            {
                finalList = ps.GetMostPopularToppings(sUrl, 20);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += Environment.NewLine + ex.InnerException.Message;
                Console.WriteLine("Exception thrown: " + ex.Message);
            }

            if (finalList != null)
            {
                int i = 0;
                foreach (var item in finalList)
                {
                    i++;
                    Console.WriteLine(i.ToString() + ". " + item.toppings + " - " + item.count);
                }
            }

            Console.ReadLine();
        }
    }
}
