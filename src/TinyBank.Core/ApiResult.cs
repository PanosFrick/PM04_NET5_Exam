using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBank.Core
{
    public class ApiResult<T>
    {
        public int Code { get; set; }
        public string ErrorText { get; set; }
        public T Data { get; set; }

        public ApiResult()
        {
            Code = Constants.ApiResultCode.Success;
        }

        public bool IsSuccessful()
        {
            return Code == Constants.ApiResultCode.Success;
        }

        public ApiResult<Y> ToResult<Y>()
        {
            return new ApiResult<Y>() {
                Code = Code,
                ErrorText = ErrorText
            };
        }

        public static ApiResult<T> CreateSuccessful(T data)
        {
            return new ApiResult<T>() {
                Data = data
            };
        }

        public static ApiResult<T> CreateFailed(
            int code, string errorText)
        {
            return new ApiResult<T>() {
                Code = code,
                ErrorText = errorText
            };
        }
    }
}
