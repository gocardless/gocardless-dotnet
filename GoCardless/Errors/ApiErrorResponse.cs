using System;
using System.Text;
using GoCardless.Resources;

namespace GoCardless.Errors
{
    public class ApiErrorResponse : ApiResponse
    {
        public ApiError Error { get; set; }
    }
}
