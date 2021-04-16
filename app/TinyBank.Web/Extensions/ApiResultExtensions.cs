using Microsoft.AspNetCore.Mvc;

using TinyBank.Core;

namespace TinyBank.Web.Extensions
{
    public static class ApiResultExtensions
    {
        public static ObjectResult ToActionResult<T>(
            this ApiResult<T> @this)
        {
            return new ObjectResult(@this.ErrorText) {
                StatusCode = @this.Code
            };
        }
    }
}
