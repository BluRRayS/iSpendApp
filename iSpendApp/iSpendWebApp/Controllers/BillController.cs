using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;
using iSpendLogic;
using iSpendWebApp.Controllers.ActionFilters;
using iSpendWebApp.Models;
using iSpendWebApp.Models.Bill;
using iSpendWebApp.Models.Savings;
using iSpendWebApp.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace iSpendWebApp.Controllers
{
    public class BillController : Controller
    {
        private readonly IBillContext _billContext;
        private readonly BillLogic _billLogic;
        private readonly SavingLogic _savingLogic;
        private readonly IFileProvider _fileProvider;

        public BillController(IBillContext billContext, ISavingsContext savingsContext, IFileProvider fileProvider)
        {
            _billContext = billContext;
            _billLogic = new BillLogic(_billContext);
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

                var billContext = _billContext.GetBillsByUsername(HttpContext.Session.GetString("UserSession"));
                var bills = (billContext.Select(bill => new BillViewModel(bill.BillId, bill.BillName, bill.BillBalance, bill.Transactions, bill.IconId, bill.AccountIds, _fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList(), _billLogic.GetAccountReservations(bill.BillId))));

                var savingContext = _savingLogic.GetUserSavings((int)HttpContext.Session.GetInt32("UserId"));
                var savings = savingContext.Select(saving => new SavingsViewModel(saving.UserId, saving.SavingId, saving.SavingName, saving.SavingCurrentAmount, saving.SavingsGoalAmount, saving.State, saving.IconId, saving.GoalDate));

                var model = new LandingPageViewModel(bills, savings);
                return View("~/Views/Shared/Overview.cshtml", model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
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
            var model = new BillViewModel(_fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList());
            return View("CreateBill", model);

        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Create(BillViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _billLogic.AddBill(model, Convert.ToInt32(HttpContext.Session.GetInt32("UserId")));
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
            var context = _billLogic.GetBillById(id);
            var model = new BillViewModel(context.BillId, context.BillName, context.BillBalance, context.Transactions, context.IconId, context.AccountIds, _fileProvider.GetDirectoryContents("wwwroot/Icons/Bill").ToList().Select(icon => icon.Name).ToList(), new List<IReservation>());
            return View("EditBill", model);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Edit(int id, BillViewModel model)
        {
            try
            {
                _billLogic.UpdateBill(id, model.BillName, model.IconId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Overview");
            }
        }

        // GET: Account/Delete/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Delete(int id)
        {
            return View("DeleteBill");
        }


        // POST: Account/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _billLogic.RemoveBill(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Overview");
            }
        }

    }
}