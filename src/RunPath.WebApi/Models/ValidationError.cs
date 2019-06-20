using System.Collections.Generic;

namespace RunPath.WebApi.Models
{
    public class ValidationError
    {
        public string ErrorType { get; set; }
        public IEnumerable<string> ErrorCodes { get; set; }

        public ValidationError(string errorType, IEnumerable<string> errorCodes)
        {
            ErrorType = errorType;
            ErrorCodes = errorCodes;
        }
    }
}
