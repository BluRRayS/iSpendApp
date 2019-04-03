﻿using System.Collections.Generic;
using iSpendDAL.ContextInterfaces;
using iSpendLogic;
using iSpendWebApp.Models.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Console.Internal;

namespace iSpendWebApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionLogic _transactionLogic;
        private readonly UserLogic _accountLogic;
        private readonly BillLogic _billLogic;
        private readonly IFileProvider _fileProvider;

        public TransactionController(ITransactionContext transactionContext, IAccountContext accountContext, IBillContext billContext, IFileProvider fileProvider)
        {
            _transactionLogic = new TransactionLogic(transactionContext);
            _accountLogic = new UserLogic(accountContext);
            _billLogic = new BillLogic(billContext);
            _fileProvider = fileProvider;
            
        }


        // GET: Transaction
        public ActionResult Index(int id,string billName,decimal balance)
        {
            var userHasAccess = false;
            if (HttpContext.Session.GetString("UserSession") == null) return RedirectToAction("Login", "User");
            var usersContext = _billLogic.GetBillUsers(id).ToList();
            foreach (var user in usersContext)
            {
                if (user.Username == HttpContext.Session.GetString("UserSession"))
                {
                    userHasAccess = true;
                }
            }
            if (!userHasAccess)
            {
                return RedirectToAction("Index", "Bill");
            }

            var categoriesContext = _transactionLogic.GetCategories();
            var categories = categoriesContext.Select(category => new SelectListItem(category.Name, category.Name)).ToList();
            ViewBag.Categories = categories;

            ViewBag.BillId = id;
            ViewBag.BillName = billName;
            ViewBag.Balance = balance;
            ViewBag.FileProvider = _fileProvider.GetDirectoryContents("wwwroot/Icons/Category").ToList().Select(icon => icon.Name).ToList();


            var context = _transactionLogic.GetBillTransactions(id);
            var model = context.Select(trans => new TransactionsViewModel(trans.TransactionId, trans.BillId, trans.TransactionName, trans.TransactionAmount, trans.Category, trans.IconId, trans.TimeOfTransaction, _fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList())).ToList();
            return View("~/Views/Transaction/Transactions.cshtml", model);
        }


        // GET: Transaction/Create
        public ActionResult Create(int id)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            var categoriesContext = _transactionLogic.GetCategories();
            ViewBag.categories = categoriesContext.Select(category => new SelectListItem(category.Name, category.Name)).ToList();
            ViewBag.FileProvider = _fileProvider.GetDirectoryContents("wwwroot/Icons/Category").ToList().Select(icon => icon.Name).ToList();
            var model = new TransactionsViewModel(id,ViewBag.FileProvider);
            return View("~/Views/Transaction/CreateTransaction.cshtml",model);
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransactionsViewModel model)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            try
            {
                _transactionLogic.CreateTransaction(model);
                _billLogic.RefreshBillBalance(model.BillId);
                return RedirectToAction(nameof(Index), new { id = model.BillId });
            }
            catch
            {
                return RedirectToAction("Create", "Transaction");
            }
        }

        // GET: Transaction/Edit/5
        public ActionResult Edit(int id, int billId)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            var context = _transactionLogic.GetTransactionById(id, billId);
            var categoriesContext = _transactionLogic.GetCategories();
            var categories = categoriesContext.Select(category => category.Name).ToList();
            var model = new TransactionsViewModel(context.TransactionId, context.BillId, context.TransactionName, context.TransactionAmount, context.Category, context.IconId, context.TimeOfTransaction, _fileProvider.GetDirectoryContents("wwwroot/Icons/Category").ToList().Select(icon => icon.Name).ToList());
            return View("~/Views/Transaction/EditTransaction.cshtml", model);
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TransactionsViewModel model)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            try
            {
                _transactionLogic.UpdateTransaction(id, model);

                return RedirectToAction("Index", "Transaction", model.BillId);
            }
            catch
            {
                return View("~/Views/Transaction/EditTransaction.cshtml");
            }
        }

        // GET: Transaction/Delete/5
        public ActionResult Delete(int id, int billId)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            _transactionLogic.DeleteTransaction(id, billId);
            return RedirectToAction("Index", "Bill");
        }
    }
}