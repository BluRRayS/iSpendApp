using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using iSpendDAL.ContextInterfaces;
using iSpendLogic;
using iSpendWebApp.Models;
using iSpendWebApp.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iSpendWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IAccountContext _accountContext;

        public UserController(IAccountContext accountContext)
        {
            _accountContext = accountContext;
        }


        // GET: User/Login
        public ActionResult Login()
        {
            return View("Login");
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var accountLogic = new AccountLogic(_accountContext);

            if (accountLogic.Login(username,password))
            {
                HttpContext.Session.SetString("UserSession",username);
                RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login");
        }


        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return View("UserDetails");
            }

            return RedirectToAction("Login");
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View("Register");
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel model)
        {
            try
            {
                var accountLogic = new AccountLogic(_accountContext);
                if (ModelState.IsValid && accountLogic.IsUsernameTaken(model.UserName)==false)
                {
                    accountLogic.AddUser(model.UserName, model.Password, model.EmailAddress);                   
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ViewBag.Message = "That username is already taken please use another.";
                    return View("Register");
                }
                
            }
            catch
            {
                ViewBag.Message = "Oops something went wrong please try again!";
                return View("Register");
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("UserSession")!=null)
            {
                return View("EditUser");
            }

            return RedirectToAction("Login");

        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Details");
            }
            catch
            {
                return View("EditUser");
            }
        }
    }
}