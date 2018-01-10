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
            ViewData["function"] = HttpContext.Request.Query["func"];
            ViewData["result"] = null;
            if ((ViewData["function"]).ToString().Length < 1)
            {
                ViewData["function"] = "";
            }

            string switcher = ViewData["function"].ToString();

            switch (switcher)
            {
                case "Schaffer":
                    ViewData["some"] = "1";
                    ViewData["result"] = asp.net.ParticleProgram.Run(asp.net.Functions.schaffer,
                                        Convert.ToInt32(HttpContext.Request.Query["dimensions"]),
                                        Convert.ToInt32(HttpContext.Request.Query["particleCount"]),
                                        Convert.ToInt32(HttpContext.Request.Query["maxEpochs"]),
                                        Convert.ToDouble(HttpContext.Request.Query["minX"]),
                                        Convert.ToDouble(HttpContext.Request.Query["maxX"]),
                                        Convert.ToDouble(HttpContext.Request.Query["minAccError"])
                                       );
                    break;
                case "Sphere":
                    ViewData["some"] = "2";
                    ViewData["result"] = asp.net.ParticleProgram.Run(asp.net.Functions.sphere,
                                        Convert.ToInt32(HttpContext.Request.Query["dimensions"]),
                                        Convert.ToInt32(HttpContext.Request.Query["particleCount"]),
                                        Convert.ToInt32(HttpContext.Request.Query["maxEpochs"]),
                                        Convert.ToDouble(HttpContext.Request.Query["minX"]),
                                        Convert.ToDouble(HttpContext.Request.Query["maxX"]),
                                        Convert.ToDouble(HttpContext.Request.Query["minAccError"])
                                       );
                    break;
                case "Griewank":
                    ViewData["some"] = "3";
                    ViewData["result"] = asp.net.ParticleProgram.Run(asp.net.Functions.griewank,
                                        Convert.ToInt32(HttpContext.Request.Query["dimensions"]),
                                        Convert.ToInt32(HttpContext.Request.Query["particleCount"]),
                                        Convert.ToInt32(HttpContext.Request.Query["maxEpochs"]),
                                        Convert.ToDouble(HttpContext.Request.Query["minX"]),
                                        Convert.ToDouble(HttpContext.Request.Query["maxX"]),
                                        Convert.ToDouble(HttpContext.Request.Query["minAccError"])
                                       );
                    break;
                case "Rastrigin":
                    ViewData["some"] = "4";
                    ViewData["result"] = asp.net.ParticleProgram.Run(asp.net.Functions.rastrigin,
                                        Convert.ToInt32(HttpContext.Request.Query["dimensions"]),
                                        Convert.ToInt32(HttpContext.Request.Query["particleCount"]),
                                        Convert.ToInt32(HttpContext.Request.Query["maxEpochs"]),
                                        Convert.ToDouble(HttpContext.Request.Query["minX"]),
                                        Convert.ToDouble(HttpContext.Request.Query["maxX"]),
                                        Convert.ToDouble(HttpContext.Request.Query["minAccError"])
                                       );
                    break;
                case "Rosenbrock":
                    ViewData["some"] = "5";
                    ViewData["result"] = asp.net.ParticleProgram.Run(asp.net.Functions.rosenbrock,
                                        Convert.ToInt32(HttpContext.Request.Query["dimensions"]),
                                        Convert.ToInt32(HttpContext.Request.Query["particleCount"]),
                                        Convert.ToInt32(HttpContext.Request.Query["maxEpochs"]),
                                        Convert.ToDouble(HttpContext.Request.Query["minX"]),
                                        Convert.ToDouble(HttpContext.Request.Query["maxX"]),
                                        Convert.ToDouble(HttpContext.Request.Query["minAccError"])
                                       );
                    break;
                default:
                    ViewData["some"] = "default";
                    break;

            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
