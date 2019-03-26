using System.Collections.Generic;
using iSpendDAL.ContextInterfaces;
using iSpendLogic;
using iSpendWebApp.Models.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace iSpendWebApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionLogic _transactionLogic;
        private readonly AccountLogic _accountLogic;
        private readonly BillLogic _billLogic;
        public TransactionController(ITransactionContext transactionContext, IAccountContext accountContext, IBillContext billContext)
        {
            _transactionLogic = new TransactionLogic(transactionContext);
            _accountLogic = new AccountLogic(accountContext);
            _billLogic = new BillLogic(billContext);
        }

        //Todo: Check if user has access to bill

        // GET: Transaction
        public ActionResult Index(int id)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            ViewBag.BillId = id;
            var context = _transactionLogic.GetBillTransactions(id);
            var model = new List<TransactionsViewModel>();
            foreach (var trans in context)
            {
                model.Add(new TransactionsViewModel(trans.TransactionId,trans.AccountId,trans.TransactionName,trans.TransactionAmount,trans.Category,trans.IconId,trans.TimeOfTransaction));
            }            
            return View("~/Views/Transaction/Transactions.cshtml", model);
        }

        // GET: Transaction/Details/5
        public ActionResult Details(int id)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");

            return View();
        }

        // GET: Transaction/Create
        public ActionResult Create(int id)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            var model = new TransactionsViewModel {AccountId = id};
            return View("~/Views/Transaction/CreateTransaction.cshtml",model);
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransactionsViewModel model)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            try
            {
                _transactionLogic.CreateTransaction(model);
                _billLogic.RefreshBillBalance(model.AccountId);
                return RedirectToAction(nameof(Index),new {id = model.AccountId});
            }
            catch
            {
                return RedirectToAction("Create", "Transaction");
            }
        }

        // GET: Transaction/Edit/5
        public ActionResult Edit(int id)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            return View();
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
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
        public ActionResult Delete(int Id)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            return View();
        }

        // POST: Transaction/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
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