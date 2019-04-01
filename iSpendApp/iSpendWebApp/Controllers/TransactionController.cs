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
        private readonly UserLogic _accountLogic;
        private readonly BillLogic _billLogic;
        public TransactionController(ITransactionContext transactionContext, IAccountContext accountContext, IBillContext billContext)
        {
            _transactionLogic = new TransactionLogic(transactionContext);
            _accountLogic = new UserLogic(accountContext);
            _billLogic = new BillLogic(billContext);
        }

        //Todo: Check if user has access to bill

        // GET: Transaction
        public ActionResult Index(int id)
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
            ViewBag.BillId = id;
            var context = _transactionLogic.GetBillTransactions(id);
            var model = context.Select(trans => new TransactionsViewModel(trans.TransactionId, trans.BillId, trans.TransactionName, trans.TransactionAmount, trans.Category, trans.IconId, trans.TimeOfTransaction)).ToList();
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
            var model = new TransactionsViewModel {BillId = id};
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
                _billLogic.RefreshBillBalance(model.BillId);
                return RedirectToAction(nameof(Index),new {id = model.BillId});
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
        public ActionResult Delete(int id, int billId)
        {
            var username = HttpContext.Session.GetString("UserSession");
            if (username == null) return RedirectToAction("Login", "User");
            _transactionLogic.DeleteTransaction(id, billId);
            return RedirectToAction("Index","Bill");
        }
    }
}