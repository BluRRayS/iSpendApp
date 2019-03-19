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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iSpendWebApp.Controllers
{
    public class BillController : Controller
    {
        private readonly IBillContext _billContext;

        public BillController(IBillContext billContext)
        {
            _billContext = billContext;
        }

        // GET: Account
        public ActionResult Index()
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username != null)
            {
                var billLogic = new BillLogic(_billContext);
                IEnumerable<IBill> models = new List<BillViewModel>();
                models = billLogic.GetUserBills(username).ToList();
                return View("BillOverview", models as IEnumerable<BillViewModel>);
            }

            return RedirectToAction("Login", "User");
        }

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            if (HttpContext.Session.IsAvailable)
            {
                return View("AccountDetails");
            }

            return RedirectToAction("Login", "User");
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View("CreateAccount");
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("CreateAccount");
            }
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