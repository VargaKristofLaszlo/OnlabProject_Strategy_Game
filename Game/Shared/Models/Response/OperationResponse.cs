using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.Models.Response
{
    public class OperationResponse : BaseResponse 
    {
        public OperationResponse() : base() { }

        protected OperationResponse(string message, bool isSuccess, IEnumerable<string> errors = null) : base(message, isSuccess, errors) { }

        public static OperationResponse Failed(string message, IEnumerable<string> errors = null)
            => new OperationResponse(message, false, errors);
        

        public static OperationResponse Succeeded(string message)
            => new OperationResponse(message, true);
        
    }


    public class OperationResponse<T> : OperationResponse
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        public OperationResponse(): base() {}

        private OperationResponse(string message, bool isSuccess, IEnumerable<string> errors = null) : base(message, isSuccess, errors) { }
       
       
        private OperationResponse(string message, bool isSuccess, T data, IEnumerable<string> errors = null) :base(message,isSuccess,errors)
            =>  Data = data;        
        

        public static new OperationResponse<T> Failed(string message, IEnumerable<string> errors = null)
            => new OperationResponse<T>(message, false, errors);
        

        public static OperationResponse<T> Succeeded(string message, T data)
            => new OperationResponse<T>(message, true, data);
        

    }
}
