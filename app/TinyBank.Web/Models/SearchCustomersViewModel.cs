using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TinyBank.Core.Model;
using TinyBank.Core.Services.Options;

namespace TinyBank.Web.Models
{
    public class SearchCustomersViewModel
    {
        public List<Customer> Customers { get; set; }
        public SearchCustomerOptions SearchOptions { get; set; }

        public SearchCustomersViewModel()
        {
            Customers = new List<Customer>();
            SearchOptions = new SearchCustomerOptions();
        }
    }
}
