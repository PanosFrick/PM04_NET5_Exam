using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Web.Models;

namespace TinyBank.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService _customers;
        private readonly ILogger<HomeController> _logger;
        private readonly TinyBankDbContext _dbContext;

        public HomeController(
            TinyBankDbContext dbContext,
            ILogger<HomeController> logger,
            ICustomerService customers)
        {
            _logger = logger;
            _customers = customers;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var accounts = _dbContext.Set<Account>()
                .Include(a => a.Customer)
                .Select(a => new {
                    AccountId = a.AccountId,
                    Description = a.Description,
                    Customer = new {
                        FirstName = a.Customer.Firstname,
                        LastName = a.Customer.Lastname
                    }
                })
                .ToList();

            return Json(accounts);
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
    }
}
