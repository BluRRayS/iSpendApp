using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;
using iSpendLogic;
using iSpendWebApp.Models;
using iSpendWebApp.Models.Bill;
using iSpendWebApp.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iSpendWebApp.Controllers
{
    public class BillController : Controller
    {
        private readonly IBillContext _billContext;
        private readonly BillLogic _billLogic;
        public BillController(IBillContext billContext)
        {
            _billContext = billContext;
            _billLogic = new BillLogic(_billContext);
        }

        // GET: Bill
        public ActionResult Index()
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");

            var context = _billContext.GetBillsByUsername(HttpContext.Session.GetString("UserSession"));
            var models = context.Select(bill => new BillViewModel(bill.BillId, bill.BillName, bill.BillBalance, bill.Transactions, bill.IconId, bill.AccountIds)).ToList();
            return View("BillOverview", models);

        }

        // GET: Account/Details/5
        public ActionResult Details()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {

                return View("~/Views/Transaction/Transactions.cshtml");
            }

            return RedirectToAction("Login", "User");
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return View("CreateBill");
            }

            return RedirectToAction("Login", "User");
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BillViewModel model)
        {
            if (HttpContext.Session.GetString("UserSession") == null) return RedirectToAction("Login", "User");
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
        public ActionResult Edit(int id)
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

            var context = _billLogic.GetBillById(id);
            var model = new BillViewModel(context.BillId, context.BillName, context.BillBalance, context.Transactions, context.IconId, context.AccountIds);
            return View("EditBill", model);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BillViewModel model)
        {
            try
            {

                _billLogic.UpdateBill(id, model.BillName, model.IconId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("BillOverview");
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("UserSession") == null) return RedirectToAction("Login", "User");
            return View("DeleteBill");
        }


        // POST: Account/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _billLogic.RemoveBill(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("BillOverview");
            }
        }
    }
}