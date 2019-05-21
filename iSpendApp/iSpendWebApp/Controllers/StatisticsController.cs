using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;
using iSpendLogic;
using iSpendWebApp.Controllers.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iSpendWebApp.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly StatisticsLogic _statisticsLogic;

        public StatisticsController(IAccountContext accountContext, ITransactionContext transactionContext)
        {
            _statisticsLogic = new StatisticsLogic(accountContext, transactionContext);
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public IActionResult Index()
        {
            //Todo:Move logic to a logic class
            var categoryStats =
                _statisticsLogic.GetUserCategoryStatistics(HttpContext.Session.GetString("UserSession"));

            var totalBalanceStatistics =
                _statisticsLogic.GetTotalBalanceStatistics((int) HttpContext.Session.GetInt32("UserId"));

            var months = totalBalanceStatistics.MonthNumber.Select(monthNumber => DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthNumber)).ToList();

            ViewBag.Categories = Newtonsoft.Json.JsonConvert.SerializeObject(categoryStats.Categories);
            ViewBag.CategoriesCosts = Newtonsoft.Json.JsonConvert.SerializeObject(categoryStats.AvgCostPerCategory);
            ViewBag.TimeStamps = Newtonsoft.Json.JsonConvert.SerializeObject(months);
            ViewBag.AccountBalance = Newtonsoft.Json.JsonConvert.SerializeObject(totalBalanceStatistics.Balances);

            return View("~/Views/Statistics/Dashboard.cshtml");
        }
    }
}