﻿using System;
using System.Collections.Generic;
using System.Linq;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;
using iSpendLogic;
using iSpendWebApp.Controllers.ActionFilters;
using iSpendWebApp.Models;
using iSpendWebApp.Models.Account;
using iSpendWebApp.Models.Savings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace iSpendWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountLogic _accountLogic;
        private readonly SavingLogic _savingLogic;
        private readonly IFileProvider _fileProvider;

        public AccountController(IAccountContext billContext, ISavingsContext savingsContext, IFileProvider fileProvider)
        {
            _accountLogic = new AccountLogic(billContext);
            _fileProvider = fileProvider;
            _savingLogic = new SavingLogic(savingsContext);
        }

        // GET: Bill
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Index()
        {
            try
            {
                ViewBag.FileProvider = _fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList();

                var accountContext = _accountLogic.GetAccountsByUsername(HttpContext.Session.GetString("UserSession"));
                var accounts = (accountContext.Select(account => new AccountViewModel(account.AccountId, account.AccountName, account.AccountBalance, account.Transactions, account.IconId, account.UserIds, _fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList(), _accountLogic.GetAccountReservations(account.AccountId))));
                
                var savingContext = _savingLogic.GetOngoingUserSavings((int)HttpContext.Session.GetInt32("UserId"));
                var savings = savingContext.Select(saving => new SavingsViewModel(saving.UserId, saving.SavingId, saving.SavingName, saving.SavingCurrentAmount, saving.SavingsGoalAmount, saving.State, saving.IconId, saving.GoalDate));

                var model = new LandingPageViewModel(accounts, savings);
                return View("~/Views/Shared/Overview.cshtml", model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("Index","Home");
            }
            
        }

        // GET: Account/Details/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Details()
        {
            return View("~/Views/Transaction/Transactions.cshtml");

        }

        // GET: Account/Create
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Create()
        {
            var model = new AccountViewModel(_fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList());
            return View("CreateBill", model);

        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Create(AccountViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _accountLogic.AddAccount(model, Convert.ToInt32(HttpContext.Session.GetInt32("UserId")));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("CreateBill");
            }
        }

        // GET: Account/Edit/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Edit(int id)
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
            var context = _accountLogic.GetAccountById(id);
            var model = new AccountViewModel(context.AccountId, context.AccountName, context.AccountBalance, context.Transactions, context.IconId, context.UserIds, _fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList(), new List<IReservation>());
            return View("EditBill", model);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Edit(int id, AccountViewModel model)
        {
            try
            {
                _accountLogic.UpdateAccount(id, model.AccountName, model.IconId);
                return RedirectToAction(nameof(Index),"Account");
            }
            catch
            {
                return View("Overview");
            }
        }

        // POST: Account/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Delete(AccountViewModel model)
        {
            try
            {
                _accountLogic.RemoveAccount(model.AccountId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction(nameof(Index));
            }
        }

    }
}