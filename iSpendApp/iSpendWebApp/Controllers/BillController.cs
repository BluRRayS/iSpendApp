using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;
using iSpendLogic;
using iSpendWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iSpendWebApp.Controllers
{
    public class BillController : Controller
    {
        private readonly IBillContext _billContext;
        private BillLogic billLogic;
        public BillController(IBillContext billContext)
        {
            _billContext = billContext;
            billLogic = new BillLogic(_billContext);
        }

        // GET: Bill
        public ActionResult Index()
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username != null)
            {
                var models = new List<BillViewModel>();
                var context = _billContext.GetBillsByUsername(HttpContext.Session.GetString("UserSession"));
                foreach (var bill in context)
                {
                    models.Add(new BillViewModel(bill.BillId, bill.BillName, bill.BillBalance, bill.Transactions, bill.IconId, bill.AccountIds));
                }

                return View("BillOverview", models);
            }

            return RedirectToAction("Login", "User");
        }

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return View("BillDetails");
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
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {       
                        billLogic.AddBill(model, Convert.ToInt32(HttpContext.Session.GetInt32("UserId")));
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("CreateBill");
                }

            }

            return RedirectToAction("Login", "User");
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View("EditAccount");
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("EditAccount");
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        // POST: Account/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}