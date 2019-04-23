using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendDAL.ContextInterfaces;
using iSpendLogic;
using iSpendWebApp.Controllers.ActionFilters;
using iSpendWebApp.Models;
using iSpendWebApp.Models.Savings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace iSpendWebApp.Controllers
{
    public class SavingsController : Controller
    {
        private readonly IFileProvider _fileProvider;
        private readonly SavingLogic _savingLogic;
        private readonly AccountLogic _billLogic;

        public SavingsController(ISavingsContext context, IAccountContext billContext, IFileProvider fileProvider)
        {
            _savingLogic = new SavingLogic(context);
            _fileProvider = fileProvider;
            _billLogic = new AccountLogic(billContext);
        }


        // GET: Savings
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Account");
        }

        // GET: Savings/Details/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Details(int id)
        {
            var context = _savingLogic.GetSavingById(id);
            var model = new SavingsViewModel(context.UserId,id,context.SavingName,context.SavingCurrentAmount,context.SavingsGoalAmount,context.State,context.IconId,context.GoalDate);
            ViewBag.FileProvider = _fileProvider.GetDirectoryContents("wwwroot/Icons/Savings").ToList().Select(icon => icon.Name).ToList();
            ViewBag.Accounts = _billLogic.GetUserAccounts(HttpContext.Session.GetString("UserSession"));
            return View("~/Views/Savings/SavingDetails.cshtml",model);
        }

        // GET: Savings/Create
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Create()
        {
            ViewBag.Bills = _billLogic.GetUserAccounts(HttpContext.Session.GetString("UserSession")) ;
            ViewBag.FileProvider = _fileProvider.GetDirectoryContents("wwwroot/Icons/Savings").ToList().Select(icon => icon.Name).ToList();
            return View("~/Views/Savings/CreateSaving.cshtml");
        }

        // POST: Savings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Create(CreateSavingsViewModel model)
        {
            try
            {
                model.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _savingLogic.CreateSaving(model, model.WithdrawFromBillId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Create");
            }
        }

        // GET: Savings/Edit/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Savings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Edit(SavingsViewModel model)
        {
            try
            {
                _savingLogic.UpdateSaving(model);

                return RedirectToAction(nameof(Details), "Savings", new {id = model.SavingId});
            }
            catch
            {
                return RedirectToAction(nameof(Details), "Savings", new { id = model.SavingId });
            }
        }



        // POST: Savings/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Delete(SavingsViewModel model)
        {
            try
            {
                _savingLogic.DeleteSaving(model.SavingId);
                return RedirectToAction(nameof(Index), "Account");
            }
            catch
            {
                return RedirectToAction("Index", "Account");
            }
        }
        // POST: Savings/AddReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult AddReservation(ReservationViewModel model)
        {
            try
            {
                _savingLogic.AddReservation(model);
                _savingLogic.RefreshSavingsAmount(model.SavingsId);
                return RedirectToAction("Index", "Account");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("Index", "Account");
            }
            
        }

        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult CompleteSaving(int id)
        {
            _savingLogic.CompleteSaving(id);
            return RedirectToAction("Index", "Account");
        }
    }

}