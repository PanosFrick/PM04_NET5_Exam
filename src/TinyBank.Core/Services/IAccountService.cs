using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBank.Core.Model;

namespace TinyBank.Core.Services
{
    public interface IAccountService
    {
        ApiResult<Account> Create(Guid customerId,
            Options.CreateAccountOptions options);
    }
}
