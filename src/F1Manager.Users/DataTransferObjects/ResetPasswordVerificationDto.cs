namespace F1Manager.Users.DataTransferObjects;

public class ResetPasswordVerificationDto
{
    public string UsernameOrEmail { get; set; }
    public string VerificationCode { get; set; }
}