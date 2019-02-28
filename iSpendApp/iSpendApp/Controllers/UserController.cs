using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using iSpendApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using System.Configuration;


namespace iSpendApp.Controllers
{
    public class UserController : Controller
    {
        private string connectionString =
            "Server=mssql.fhict.local;Database=dbi412182;User Id=dbi412182;Password=!LeGo2001;";
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult CreateUser(UserViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand command = new SqlCommand("INSERT INTO [User](UserName,Password,Email,DateOfCreation) VALUES (@UserName,@Password,@Email,@DateOfCreation)", conn);
                        command.Parameters.AddWithValue("@UserName", user.Username);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@DateOfCreation", DateTime.Now);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    return View("~/Views/Home/Register.cshtml");
                }
            }
            catch
            {
                return View("~/Views/Home/Register.cshtml");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string userName, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("[dbo].[UserLogin]", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", userName);
                    command.Parameters.AddWithValue("@Password", password);
                    command.ExecuteNonQuery();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                           
                        }
                    }
                    conn.Close();
                }
                return View("~/Views/Home/index.cshtml");
            }
            catch
            {
                ViewBag.Message = "Credentials are wrong or account does not exist";
                return View("~/Views/Home/Login.cshtml");
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
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

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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