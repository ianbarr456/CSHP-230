using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheLearningCenterReworked.Models;
using TheLearningCenterReworked.Repositories;

namespace TheLearningCenterReworked.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository userRepository;

        public AccountController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public ActionResult LogOff()
        {
            Session["UserEmail"] = null;
            //System.Web.Security.FormsAuthentication.SignOut();
            return Redirect("~/");
        }
        public ActionResult LogOn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userRepository.LogIn(model.UserEmail, model.Password);
                if (user != null)
                {
                    Session["UserEmail"] = model.UserEmail;
                    Session["UserID"] = model.UserId;
                    System.Web.Security.FormsAuthentication.SetAuthCookie(model.UserEmail, model.RememberMe);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "The user email or password provided is incorrect.");
            }
            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userRepository.Register(model.UserEmail, model.Password);
                if (user != null)
                {
                    Session["UserEmail"] = model.UserEmail;
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "User email already exists.");
            }
            return View(model);
        }
    }
}