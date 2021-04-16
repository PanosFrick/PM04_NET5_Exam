using System;
using System.Collections.Generic;

namespace TinyBank.Core.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VatNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? DateOfBirth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Constants.CustomerType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get; set; }

        #region Navigation properties

        /// <summary>
        /// 
        /// </summary>
        public List<Account> Accounts { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public AuditInfo AuditInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Customer()
        {
            CustomerId = Guid.NewGuid();
            IsActive = true;
            Accounts = new List<Account>();
            AuditInfo = new AuditInfo();
        }
    }
}
