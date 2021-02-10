using System.Collections.Generic;
using System.Linq;

namespace BackEnd.Models.Response
{
    public class UsermanagerResponse
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public UsermanagerResponse(bool isSuccess, IEnumerable<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static UsermanagerResponse TaskFailed(IEnumerable<string> errors = null) 
        {
            if (errors == null)
                return new UsermanagerResponse(false, Enumerable.Empty<string>());

            return new UsermanagerResponse(false, errors);
        }
        
        public static UsermanagerResponse TaskCompletedSuccessfully() 
        {
            return new UsermanagerResponse(true, null);
        }
    }
}
