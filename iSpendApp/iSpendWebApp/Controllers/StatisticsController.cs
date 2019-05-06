using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendWebApp.Controllers.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace iSpendWebApp.Controllers
{
    public class StatisticsController : Controller
    {
        [HttpGet]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public IActionResult Index()
        {
            var categories = new List<string>();
            categories.Add("Groceries");
            categories.Add("health");
            categories.Add("Taxes");
            categories.Add("Rent");
            categories.Add("Fuel");
            categories.Add("Clothes");

            var numbers = new List<int>();
            for (int i = 0; i < categories.Count; i++)
            {
                numbers.Add(new Random().Next(1,20));
            }



            ViewBag.Categories = Newtonsoft.Json.JsonConvert.SerializeObject(categories); 
            ViewBag.CategoriesCosts = Newtonsoft.Json.JsonConvert.SerializeObject(numbers);

            return View("~/Views/Statistics/Dashboard.cshtml");
        }
    }
}