using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationCore.Controllers
{
    [Authorize(Policy = "AdministratorOnly")]
    [Authorize(Policy = "EmployeeId")]
    [Authorize(Policy = "BuildingEntry")]
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}