namespace BackEnd.Infrastructure
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public string SenderAddress { get; set; }
        public string SenderPassword { get; set; }
        public string AppUrl { get; set; }
    }
}
