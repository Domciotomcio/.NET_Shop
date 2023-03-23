using AW_lab10.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AW_lab10.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ForAdmin()
        {
            ViewData["Info"] = "For Admin"; 
            return View("Info"); 
        }
        
        [AllowAnonymous]
        public IActionResult ForAll()
        { 
            ViewData["Info"] = "For All"; return View("Info"); 
        }

        [Authorize]
        public IActionResult ForLogIn()
        { 
            ViewData["Info"] = "For Log In"; return View("Info"); 
        }
        [Authorize(Roles = "Admin, Dean")]
        public IActionResult ForAdminOrDean()
        { 
            ViewData["Info"] = "For Admin or Dean"; return View("Info"); 
        }

    }
}
