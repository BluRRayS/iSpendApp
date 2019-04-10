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
        private readonly BillLogic _billLogic;

        public SavingsController(ISavingsContext context, IBillContext billContext, IFileProvider fileProvider)
        {
            _savingLogic = new SavingLogic(context);
            _fileProvider = fileProvider;
            _billLogic = new BillLogic(billContext);
        }


        // GET: Savings
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Bill");
        }

        // GET: Savings/Details/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Details(int id)
        {
            var context = _savingLogic.GetSavingById(id);
            var model = new SavingsViewModel(context.UserId,id,context.SavingName,context.SavingCurrentAmount,context.SavingsGoalAmount,context.State,context.IconId,context.GoalDate);
            return View("~/Views/Savings/SavingDetails.cshtml",model);
        }

        // GET: Savings/Create
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Create()
        {
            ViewBag.Bills = _billLogic.GetUserBills(HttpContext.Session.GetString("UserSession")) ;
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
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
                return RedirectToAction(nameof(Index), "Bill");
            }
            catch
            {
                return RedirectToAction("Index", "Bill");
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
                return RedirectToAction("Index", "Bill");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("Index", "Bill");
            }
            
        }
    }
}