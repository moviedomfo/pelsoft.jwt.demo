namespace pelsoft.auth.Amazon.be
{
    public class LoogonUserResult
    {
        public string ErrorMessage { get; set; }
        public string LogResult { get; set; }
        public bool Autenticated { get; set; }

        public string Token { get; set; }
    }
}
