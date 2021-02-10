using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.Models.Response
{
    public class LoginResponse : BaseResponse
    {
        [JsonPropertyName("expiredate")]
        public DateTime? ExpireDate { get; set; }
        [JsonPropertyName("accesstoken")]
        public string AccessToken { get; set; }

        public LoginResponse():base()
        {

        }

        private LoginResponse(string message, bool isSuccess, string accessToken, DateTime? expireDate, IEnumerable<string> errors = null)
            :base(message,isSuccess, errors)
        {
            ExpireDate = expireDate;
            AccessToken = accessToken;
        }

        public static LoginResponse Failed(string message, IEnumerable<string> errors = null) 
        {
            return new LoginResponse(message, false, null, null, errors);
        }

        public static LoginResponse Succeeded(string message, string accessToken, DateTime? expireDate) 
        {
            return new LoginResponse(message, true, accessToken, expireDate);
        }
    }
}
