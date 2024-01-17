using Microsoft.AspNetCore.Mvc;
using PairOfEmployees.Services.Data.Contracts;
using PairOfEmployees.Web.Models;
using System.Diagnostics;

namespace PairOfEmployees.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICsvService _csvService;

        public HomeController(
            IEmployeeService employeeService, 
            ICsvService csvService)
        {
            _employeeService = employeeService;
            _csvService = csvService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var data = _csvService.LoadData(file);

                var result = _employeeService.GetLongestWorkingPair(data);

                return PartialView("_PairOfEmployeesTable", result);
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
