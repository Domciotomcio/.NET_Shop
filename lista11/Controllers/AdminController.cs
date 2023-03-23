using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lista11.Controllers
{
    [Authorize(Policy = "DenyAdmin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        { ViewData["Info"] = "AdminController -> For All"; return View("Info"); }
        [Authorize(Roles = "Dean")]
        public IActionResult ForDean()
        { ViewData["Info"] = "AdminController -> For (Admin and Dean)"; return View("Info"); }
        public IActionResult ForAdmin()
        {
            ViewData["Info"] = "AdminController -> For Admin"; return View("Info"); }
        }
    }
