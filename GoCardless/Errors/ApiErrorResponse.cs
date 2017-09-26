using System;
using System.Text;
using GoCardless.Resources;

namespace GoCardless.Errors
{
    public class ApiErrorResponse : ApiResponse
    {
        /// <summary>An ApiError object representing the details of the error.</summary>
        public ApiError Error { get; set; }
    }
}
