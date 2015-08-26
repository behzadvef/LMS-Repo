using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS_Proj.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Learning Management System (LMS)";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Mail: contact@Lexicon.Lex";

            return View();
        }
    }
}