using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using iSpendDAL.ContextInterfaces;
using iSpendInterfaces;
using iSpendLogic;
using iSpendLogic.Models;
using iSpendWebApp.Controllers.ActionFilters;
using iSpendWebApp.Models;
using iSpendWebApp.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iSpendWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserContext _userContext;

        public UserController(IUserContext userContext)
        {
            _userContext = userContext;
        }


        // GET: User/Login
        public ActionResult Login()
        {
            return View("Login");
        }

        //
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("UserSession");
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var accountLogic = new UserLogic(_userContext);

            if (accountLogic.Login(username, password))
            {
                HttpContext.Session.SetString("UserSession", username);
                HttpContext.Session.SetInt32("UserId", accountLogic.GetAccountByUsername(username).UserId);
                RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login");
        }


        // GET: User/Details/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Details(int id)
        {
            var model = new UserViewModel();
            var accountLogic = new UserLogic(_userContext);
            model.Username = accountLogic.GetAccountByUsername(HttpContext.Session.GetString("UserSession")).Username;
            model.Email = accountLogic.GetAccountByUsername(HttpContext.Session.GetString("UserSession")).Email;
            return View("UserDetails", model);
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
                var accountLogic = new UserLogic(_userContext);
                if (ModelState.IsValid && accountLogic.IsUsernameTaken(model.Username) == false)
                {
                    accountLogic.AddUser(model.Username, model.Password, model.Email);
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
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Edit()
        {

            var model = new EditUserViewModel();
            var accountLogic = new UserLogic(_userContext);
            model.Username = accountLogic.GetAccountByUsername(HttpContext.Session.GetString("UserSession")).Username;
            model.Email = accountLogic.GetAccountByUsername(HttpContext.Session.GetString("UserSession")).Email;
            return View("EditUser", model);

        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Edit(EditUserViewModel model)
        {
            try
            {
                var accountLogic = new UserLogic(_userContext);
                if (ModelState.IsValid && accountLogic.IsUsernameTaken(model.Username) == false)
                {
                    model.UserId = accountLogic.GetAccountByUsername(HttpContext.Session.GetString("UserSession"))
                        .UserId;
                    accountLogic.UpdateUserDetails(model);
                    HttpContext.Session.SetString("UserSession", model.Username);
                }
                else if (accountLogic.IsUsernameTaken(model.Username) == true)
                {
                    ViewBag.Message = "That username is already taken please use another.";
                    return View("EditUser", model);
                }

                return RedirectToAction("Details");
            }
            catch
            {
                ViewBag.Message = "Oops something went wrong please try again!";
                return View("EditUser", model);
            }
        }
    }
}