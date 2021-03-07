namespace Game.Shared.Models.Request
{
    public class UserBanRequest
    {
        public string Username { get; set; }

        public string CauseOfBan { get; set; }

        public string Message { get; set; }


    }
}
