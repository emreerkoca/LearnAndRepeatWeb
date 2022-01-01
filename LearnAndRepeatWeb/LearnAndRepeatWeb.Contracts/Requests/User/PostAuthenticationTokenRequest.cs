namespace LearnAndRepeatWeb.Contracts.Requests.User
{
    public class PostAuthenticationTokenRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
