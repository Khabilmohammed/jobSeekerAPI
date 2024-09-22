using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.Models
{
    public static class ResponseHelper
    {
        public static APIResponse Success(object result = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new APIResponse
            {
                StatusCode = statusCode,
                IsSuccess = true,
                Result = result
            };
        }

        public static APIResponse Error(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new APIResponse
            {
                StatusCode = statusCode,
                IsSuccess = false,
                ErrorMessages = errorMessages
            };
        }

        public static APIResponse Error(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return Error(new List<string> { errorMessage }, statusCode);
        }
    }
}
