using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Shared.Models.Response
{
    public abstract class BaseResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("issuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("errors")]
        public IEnumerable<string> Errors { get; set; }


        public BaseResponse()
        {

        }

        protected BaseResponse(string message, bool isSuccess, IEnumerable<string> errors = null) 
        {
            Message = message;
            IsSuccess = isSuccess;

            if (errors == null)
                Errors = Enumerable.Empty<string>();
            else
                Errors = errors;
        }
        
    }
}
