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
using TinyBank.Core.Services.Options;
using TinyBank.Web.Extensions;
using TinyBank.Web.Models;

namespace TinyBank.Web.Controllers
{
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customers;
        private readonly ILogger<HomeController> _logger;
        private readonly TinyBankDbContext _dbContext;

        // Path: '/customer'
        public CustomerController(
            TinyBankDbContext dbContext,
            ILogger<HomeController> logger,
            ICustomerService customers)
        {
            _logger = logger;
            _customers = customers;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index(SearchCustomerOptions options)
        {
            options = options ?? new SearchCustomerOptions();
            options.MaxResults = 100;

            var customers = _customers.Search(options)
                .OrderByDescending(c => c.AuditInfo.Created)
                .ToList();

            return View(
                new SearchCustomersViewModel() {
                    Customers = customers,
                    SearchOptions = options
                });
        }

        [HttpGet("{id:guid}")]
        public IActionResult Detail(Guid id)
        {
            var result = _customers.GetById(id);

            if (!result.IsSuccessful()) {
                return result.ToActionResult();
            }

            return View(result.Data);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateDetails(Guid id,
            [FromBody] UpdateCustomerOptions options)
        {
            var result = _customers.Update(id, options);

            if (!result.IsSuccessful()) {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpGet("{id:guid}/accounts")]
        public IActionResult Accounts(Guid id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Register(
           [FromBody] RegisterCustomerOptions options)
        {
            return Ok(options);
        }
    }
}
