using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirthdayCardGenerator.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CardForm()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CardForm(Models.BirthdayCardModel birthdayCard)
        {
            if (ModelState.IsValid)
            {
                return View("BirthdayCardSent", birthdayCard);
            }
            else
            {
                return View();
            }
        }
    }
}