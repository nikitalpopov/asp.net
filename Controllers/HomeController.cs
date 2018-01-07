using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using asp.net.Models;

namespace asp.net.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult First()
        {
            ViewData["function"] = HttpContext.Request.Query["function"];
            if ((ViewData["function"]).ToString().Length < 1) ViewData["function"] = "1";

            return View();
        }

        public IActionResult Second()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Third()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
