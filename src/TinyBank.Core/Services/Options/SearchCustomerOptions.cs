using System;
using System.Collections.Generic;

namespace TinyBank.Core.Services.Options
{
    public class SearchCustomerOptions
    {
        public Guid? CustomerId { get; set; }
        public string VatNumber { get; set; }
        public int? MaxResults { get; set; }
        public bool? TrackResults { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int? Skip { get; set; }
        public List<string> CountryCodes { get; set; }

        public SearchCustomerOptions()
        {
            CountryCodes = new List<string>();
        }
    }
}
