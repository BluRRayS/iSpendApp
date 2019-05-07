using System;
using System.Collections.Generic;
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
        private readonly AccountLogic _accountLogic;
        private readonly TransactionLogic _transactionLogic;

        public StatisticsController(IAccountContext accountContext, ITransactionContext transactionContext)
        {
            _accountLogic = new AccountLogic(accountContext);
            _transactionLogic = new TransactionLogic(transactionContext);
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public IActionResult Index()
        {
            var accountIds = _accountLogic.GetUserAccounts(HttpContext.Session.GetString("UserSession"));
            var transactions = new List<ITransaction>();
            foreach (var id in accountIds)
            {
                transactions.AddRange(_transactionLogic.GetBillTransactions(id.AccountId).ToList());
            }

            transactions = transactions.Where(trans =>
                (trans.Category != "Upload") && (trans.Category != "Start" )&&( trans.Category != "SavingPlan")).ToList();       
            var categories= transactions.Select(trans => trans.Category).Distinct().ToList();

            var avgCostCategories = new List<decimal>();
            foreach (var category in categories)
            {
                avgCostCategories.Add(transactions.Where(trans => trans.Category == category).Sum(trans => trans.TransactionAmount));
            }
            var timeStamps = new List<DateTime>();
            var accountBalance = new List<decimal>();
            for (var i = 0; i < 6; i++)
            {
                timeStamps.Add((DateTime.Now).AddDays(i));
                accountBalance.Add(new Random().Next(0,900));
            }
           



            ViewBag.Categories = Newtonsoft.Json.JsonConvert.SerializeObject(categories); 
            ViewBag.CategoriesCosts = Newtonsoft.Json.JsonConvert.SerializeObject(avgCostCategories);
            ViewBag.TimeStamps = Newtonsoft.Json.JsonConvert.SerializeObject(timeStamps);
            ViewBag.AccountBalance = Newtonsoft.Json.JsonConvert.SerializeObject(accountBalance);

            return View("~/Views/Statistics/Dashboard.cshtml");
        }
    }
}