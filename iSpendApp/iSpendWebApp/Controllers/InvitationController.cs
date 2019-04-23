using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iSpendDAL.ContextInterfaces;
using iSpendLogic;
using iSpendWebApp.Controllers.ActionFilters;
using iSpendWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iSpendWebApp.Controllers
{
    public class InvitationController : Controller
    {
        private readonly AccountLogic _accountLogic;
        private readonly UserLogic _userLogic;
        private readonly InvitationLogic _invitationLogic;
        public InvitationController(IAccountContext accountContext, IUserContext userContext, IInvitationContext invitationContext)
        {
            _accountLogic = new AccountLogic(accountContext);   
            _userLogic = new UserLogic(userContext);
            _invitationLogic = new InvitationLogic(invitationContext);
        }

        // GET: Invitation
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Index()
        {
            ViewBag.Accounts = _accountLogic.GetUserAccounts(HttpContext.Session.GetString("UserSession"));
            ViewBag.Users = _userLogic.GetAllUsers();
            var context = _invitationLogic.GetUserInvitations((int) HttpContext.Session.GetInt32("UserId"));
            var model = context.Select(invite => new InvitationViewModel(invite)).ToList();
            return View("~/Views/Invitation/Invitations.cshtml", model);
        }

        // POST: Invitation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Create(InvitationViewModel model)
        {
            try
            {
                model.UserIdSender = (int)HttpContext.Session.GetInt32("UserId");
                model.Date = DateTime.Now;
                _invitationLogic.CreateInvite(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Invitation/Delete/5
        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult Delete(int id)
        {
            try
            {
                _invitationLogic.DeleteInvite(id);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [ServiceFilter(typeof(AuthorizationActionFilter))]
        public ActionResult AcceptInvite(int id)
        {
            try
            {
                _invitationLogic.AcceptInvite(id);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}