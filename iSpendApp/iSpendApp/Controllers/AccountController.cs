using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using iSpendApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iSpendApp.Controllers
{
    public class AccountController : Controller
    {
        AccountDAL data = new AccountDAL();

        // GET: Account
        public ActionResult Accounts()
        {
            return View();
        }

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.AccountId = id;
            return View("AccountOverview");
        }

        // GET: Account/Create
        public ActionResult CreateView()
        {
            return View("CreateAccount");
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewAccount account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   data.AddAccount(account);
                    return RedirectToAction(nameof(Accounts));
                }
                else { return RedirectToAction(nameof(CreateView)); }
            }
            catch
            {
                return View("Accounts");
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View("EditAccount");
        }

        // POST: Account/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                data.RemoveAccount(id);
                return RedirectToAction(nameof(Accounts));
            }
            catch
            {
                return View("Accounts");
            }
        }

        //POST: Account/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteAccount(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(connectionString);
        //        {
        //            conn.Open();
        //            SqlCommand command = new SqlCommand("DELETE FROM Account WHERE Id = @Id",conn);
        //            command.Parameters.AddWithValue("Id",id);
        //            command.ExecuteNonQuery();
        //            conn.Close();
        //        }

        //        return RedirectToAction(nameof(Accounts));
        //    }
        //    catch
        //    {
        //        return View("Accounts");
        //    }
        //}
    }
}