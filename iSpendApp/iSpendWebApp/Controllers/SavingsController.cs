using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendWebApp.Models.Savings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iSpendWebApp.Controllers
{
    public class SavingsController : Controller
    {
        // GET: Savings
        public ActionResult Index()
        {
           return RedirectToAction("Index", "Bill");
        }

        // GET: Savings/Details/5
        public ActionResult Details(int id)
        {
            var model = new SavingsViewModel();
            return View("~/Views/Savings/SavingDetails.cshtml",model);
        }

        // GET: Savings/Create
        public ActionResult Create()
        {
            return View("~/Views/Savings/CreateSaving.cshtml");
        }

        // POST: Savings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SavingsViewModel model)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Create");
            }
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

        // GET: Savings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Savings/Delete/5
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