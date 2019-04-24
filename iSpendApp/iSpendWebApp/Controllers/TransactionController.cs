using System;
using System.Collections.Generic;
using iSpendDAL.ContextInterfaces;
using iSpendLogic;
using iSpendWebApp.Models.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using iSpendInterfaces;
using iSpendWebApp.Controllers.ActionFilters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Console.Internal;

namespace iSpendWebApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionLogic _transactionLogic;
        private readonly UserLogic _userLogic;
        private readonly AccountLogic _accountLogic;
        private readonly SavingLogic _savingLogic;
        private readonly IFileProvider _fileProvider;

        public TransactionController(ITransactionContext transactionContext, IUserContext userContext, IAccountContext accountContext,ISavingsContext savingsContext ,IFileProvider fileProvider)
        {
            _transactionLogic = new TransactionLogic(transactionContext);
            _userLogic = new UserLogic(userContext);
            _accountLogic = new AccountLogic(accountContext);
            _savingLogic = new SavingLogic(savingsContext);
            _fileProvider = fileProvider;
         
        }


        // GET: Transaction
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Index(int id)
        {
            var userHasAccess = false;
            var usersContext = _accountLogic.GetAccountUsers(id).ToList();

            foreach (var user in usersContext)
            {
                if (user.Username == HttpContext.Session.GetString("UserSession"))
                {
                    userHasAccess = true;
                }
            }
            if (!userHasAccess)
            {
                return RedirectToAction("Index", "Account");
            }

            var accountDetails = _accountLogic.GetAccountById(id);

            var categoriesContext = _transactionLogic.GetCategories();
            var categories = categoriesContext.Select(category => new SelectListItem(category.Name, category.Name)).ToList();
            ViewBag.Categories = categories;

            ViewBag.BillId = id;
            ViewBag.BillName = accountDetails.AccountName;
            ViewBag.Balance = accountDetails.AccountBalance;
            ViewBag.FileProvider = _fileProvider.GetDirectoryContents("wwwroot/Icons/Category").ToList().Select(icon => icon.Name).ToList();
            ViewBag.Savings = _savingLogic.GetOngoingUserSavings((int)HttpContext.Session.GetInt32("UserId"));
            ViewBag.Accounts = _accountLogic.GetUserAccounts(HttpContext.Session.GetString("UserSession"));
            ViewBag.Users = _userLogic.GetAllUsers();

            var context = _transactionLogic.GetBillTransactions(id);
            var model = context.Select(trans => new TransactionsViewModel(trans.TransactionId, trans.AccountId, trans.TransactionName, trans.TransactionAmount, trans.Category, trans.IconId, trans.TimeOfTransaction, _fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList())).ToList();
            return View("~/Views/Transaction/Transactions.cshtml", model);
        }


        // GET: Transaction/Create
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Create(int id)
        {
            var categoriesContext = _transactionLogic.GetCategories();
            ViewBag.categories = categoriesContext.Select(category => new SelectListItem(category.Name, category.Name)).ToList();
            ViewBag.FileProvider = _fileProvider.GetDirectoryContents("wwwroot/Icons/Category").ToList().Select(icon => icon.Name).ToList();
            var model = new TransactionsViewModel(id,ViewBag.FileProvider);
            return View("~/Views/Transaction/CreateTransaction.cshtml",model);
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Create(TransactionsViewModel model)
        {
            try
            {
                _transactionLogic.CreateTransaction(model);
                _accountLogic.RefreshAccountBalance(model.AccountId);
                return RedirectToAction(nameof(Index), "Transaction", new { id = model.AccountId });
            }
            catch
            {
                return RedirectToAction("Create", "Transaction");
            }
        }

        // GET: Transaction/Edit/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Edit(int id, int billId)
        {
            var context = _transactionLogic.GetTransactionById(id, billId);
            var categoriesContext = _transactionLogic.GetCategories();
            var categories = categoriesContext.Select(category => category.Name).ToList();
            var model = new TransactionsViewModel(context.TransactionId, context.AccountId, context.TransactionName, context.TransactionAmount, context.Category, context.IconId, context.TimeOfTransaction, _fileProvider.GetDirectoryContents("wwwroot/Icons/Category").ToList().Select(icon => icon.Name).ToList());
            return View("~/Views/Transaction/EditTransaction.cshtml", model);
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Edit(int id, TransactionsViewModel model)
        {
            try
            {
                _transactionLogic.UpdateTransaction(id, model);

                return RedirectToAction("Index", "Transaction", new{id = model.AccountId});
            }
            catch
            {
                return View("~/Views/Transaction/EditTransaction.cshtml",model);
            }
        }

        // GET: Transaction/Delete/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Delete(int id, int accountId)
        {
            _transactionLogic.DeleteTransaction(id, accountId);
            return RedirectToAction("Index","Transaction", new {id = accountId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult CreateScheduledTransaction(TransactionsViewModel model)
        {
            try
            {
                _transactionLogic.AddScheduledTransaction(model);
                return RedirectToAction("Index", "Transaction", new {id = model.AccountId});
            }
            catch
            {
                return RedirectToAction("Index", "Transaction", new { id = model.AccountId});
            }
        }
    }
}