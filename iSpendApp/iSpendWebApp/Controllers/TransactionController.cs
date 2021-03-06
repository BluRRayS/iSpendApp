﻿using System;
using System.Collections.Generic;
using System.IO;
using iSpendDAL.ContextInterfaces;
using iSpendLogic;
using iSpendWebApp.Models.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CsvHelper;
using iSpendWebApp.Controllers.ActionFilters;
using iSpendWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

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

            var transactionsContext = _transactionLogic.GetBillTransactions(id);
            var transactions = transactionsContext.Select(trans => new TransactionsViewModel(trans.TransactionId, trans.AccountId, trans.TransactionName, trans.TransactionAmount, trans.Category, trans.IconId, trans.TimeOfTransaction, _fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList())).ToList();

            var scheduledTransactionsContext = _transactionLogic.GetAccountScheduledTransactions(id);
            var scheduledTransactions = scheduledTransactionsContext.Select(trans => new TransactionsViewModel(trans.TransactionId, trans.AccountId, trans.TransactionName, trans.TransactionAmount, trans.Category, trans.IconId, trans.TimeOfTransaction, _fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList())).ToList();

            var model = new TransactionOverviewViewModel(transactions,scheduledTransactions);
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
                return RedirectToAction("Index", "Transaction", new {id = model.AccountId});
            }
        }

        // GET: Transaction/Edit/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Edit(int id, int accountId)
        {
            var context = _transactionLogic.GetTransactionById(id, accountId);
            var categoriesContext = _transactionLogic.GetCategories();
            var categories = categoriesContext.Select(category => category.Name).ToList();
            var model = new TransactionsViewModel(context.TransactionId, accountId, context.TransactionName, context.TransactionAmount, context.Category, context.IconId, context.TimeOfTransaction, _fileProvider.GetDirectoryContents("wwwroot/Icons/Category").ToList().Select(icon => icon.Name).ToList());
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
                _accountLogic.RefreshAccountBalance(model.AccountId);
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
            try
            {
                _transactionLogic.DeleteTransaction(id, accountId);
                _accountLogic.RefreshAccountBalance(accountId);
                return RedirectToAction("Index", "Transaction", new { id = accountId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View("Error");
            }
           
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult EditScheduledTransaction(TransactionsViewModel model)
        {
            if (!@ModelState.IsValid)
                return RedirectToAction("EditScheduledTransaction", "Transaction", new {id = model.TransactionId});
            try          
            {
                _transactionLogic.EditScheduledTransaction(model);
                return RedirectToAction("Index", "Transaction", new { id = model.AccountId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("Index", "Transaction", new { id = model.AccountId }); throw;
            }
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult EditScheduledTransaction(int id)
        {
            var context = _transactionLogic.GetScheduledTransactionById(id);
            var model = new TransactionsViewModel(context.TransactionId,context.AccountId,context.TransactionName,context.TransactionAmount,context.Category,context.IconId,context.TimeOfTransaction, _fileProvider.GetDirectoryContents("wwwroot/Icons/Category").ToList().Select(icon => icon.Name).ToList());
            return View("EditScheduledTransactionPartial",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult DeleteScheduledTransaction(TransactionsViewModel model)
        {
            if (!@ModelState.IsValid)
                return RedirectToAction("DeleteScheduledTransaction", "Transaction", new {id = model.TransactionId});
            try
            {
                _transactionLogic.RemoveScheduledTransaction(model.TransactionId);
                return RedirectToAction("Index", "Transaction", new { id = model.AccountId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("Index", "Transaction", new { id = model.AccountId }); throw;
            }
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult DeleteScheduledTransaction(int id)
        {
            var context = _transactionLogic.GetScheduledTransactionById(id);
            var model = new TransactionsViewModel(context.TransactionId, context.AccountId, context.TransactionName, context.TransactionAmount, context.Category, context.IconId, context.TimeOfTransaction, _fileProvider.GetDirectoryContents("wwwroot/Icons/Category").ToList().Select(icon => icon.Name).ToList());
            return View("DeleteScheduledTransactionPartial", model);
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult ImportTransactions(ImportTransactionsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accountId = model.AccountId;
                    using (var reader = new StreamReader(model.Transactions.OpenReadStream()))
                    using (var csv = new CsvReader(reader))
                    {
                        csv.Configuration.Delimiter = ",";
                        csv.Configuration.HasHeaderRecord = true;
                        var good = new List<RabobankTransactions>();
                        var bad = new List<string>();
                        var isRecordBad = false;
                        csv.Configuration.BadDataFound = context =>
                        {
                            isRecordBad = true;
                            bad.Add(context.RawRecord);
                        };
                        while (csv.Read())
                        {
                            var record = csv.GetRecord<RabobankTransactions>();
                            if (!isRecordBad)
                            {
                                record.TimeOfTransaction.ToString("yyyy-MM-dd");
                                good.Add(record);
                            }

                            isRecordBad = false;
                        }
                        _transactionLogic.ImportTransactions(good, accountId);
                        good.Clear();
                        bad.Clear();
                        _accountLogic.RefreshAccountBalance(accountId);
                    }
                }
                   
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("Index", "Transaction", new {id = model.AccountId});
        }
    }
}