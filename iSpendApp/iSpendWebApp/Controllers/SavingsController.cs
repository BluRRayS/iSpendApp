using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendDAL.ContextInterfaces;
using iSpendLogic;
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
        public ActionResult Index()
        {
            if(HttpContext.Session.GetString("UserSession") == null) return RedirectToAction("Login", "User");
            return RedirectToAction("Index", "Bill");
        }

        // GET: Savings/Details/5
        public ActionResult Details(int id)
        {
            if(HttpContext.Session.GetString("UserSession") == null) return RedirectToAction("Login", "User");
            var context = _savingLogic.GetSavingById(id);
            var model = new SavingsViewModel(context.UserId,id,context.SavingName,context.SavingCurrentAmount,context.SavingsGoalAmount,context.State,context.IconId,context.GoalDate);
            return View("~/Views/Savings/SavingDetails.cshtml",model);
        }

        // GET: Savings/Create
        public ActionResult Create()
        {
            if(HttpContext.Session.GetString("UserSession") == null) return RedirectToAction("Login", "User");
            ViewBag.Bills = _billLogic.GetUserBills(HttpContext.Session.GetString("UserSession")) ;
            ViewBag.FileProvider = _fileProvider.GetDirectoryContents("wwwroot/Icons/Savings").ToList().Select(icon => icon.Name).ToList();
            return View("~/Views/Savings/CreateSaving.cshtml");
        }

        // POST: Savings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateSavingsViewModel model)
        {
            if (HttpContext.Session.GetString("UserSession") == null) return RedirectToAction("Login", "User");
            //try
            //{
                model.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _savingLogic.CreateSaving(model,model.WithdrawFromBillId);
                return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
                //return RedirectToAction("Create");
            //}
        }

        // GET: Savings/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Savings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if(HttpContext.Session.GetString("UserSession") == null) return RedirectToAction("Login", "User");
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
        public ActionResult Delete(SavingsViewModel model)
        {
            if (HttpContext.Session.GetString("UserSession") == null) return RedirectToAction("Login", "User");
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
        public ActionResult AddReservation(ReservationViewModel model)
        {
            if (HttpContext.Session.GetString("UserSession") == null) return RedirectToAction("Login", "User");
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