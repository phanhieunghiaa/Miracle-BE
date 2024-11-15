
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
     public class ServicesResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public static ServicesResponse<T> SuccessResponse(T data)
        {
            return new ServicesResponse<T> { Data = data, Success = true };
        }

        public static ServicesResponse<T> SuccessResponseWithMessage(T data, string successMessage = "Success")
        {
            return new ServicesResponse<T> { Data = data, Success = true, SuccessMessage = successMessage };
        }


        public static ServicesResponse<T> ErrorResponse(string errorMessage)
        {
            return new ServicesResponse<T> { Success = false, ErrorMessage = errorMessage };
        }
    }
}
