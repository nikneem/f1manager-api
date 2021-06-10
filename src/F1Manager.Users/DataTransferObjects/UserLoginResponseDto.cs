namespace F1Manager.Users.DataTransferObjects
{
    public sealed class UserLoginResponseDto
    {

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string JwtToken { get; set; }

    }
}
