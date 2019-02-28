using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendApp.Models;
using iSpendApp.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iSpendApp.Controllers
{
    public class TransactionController : Controller
    {
        AccountDAL data = new AccountDAL();

        // GET: Transaction
        public ActionResult Index()
        {            
            return View("~/Views/Account/AccountOverview.cshtml");
        }

        // GET: Transaction/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: Transaction/Create
        public ActionResult Create()
        {;
            return View("~/Views/Transaction/CreateTransaction.cshtml");
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransactionViewModel transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int accountId = Convert.ToInt32(TempData["AccountId"].ToString());
                    data.AddTransaction(accountId, transaction.Name, transaction.Amount);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return RedirectToAction(nameof(Create));
                }
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: Transaction/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Transaction/Edit/5
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

        // GET: Transaction/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Transaction/Delete/5
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