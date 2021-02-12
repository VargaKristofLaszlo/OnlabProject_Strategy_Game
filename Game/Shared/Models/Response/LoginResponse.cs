using System;

namespace Shared.Models.Response
{
    public class LoginResponse
    {
        public DateTime? ExpireDate { get; set; }
        public string AccessToken { get; set; }
    }
}
