namespace Services.Models.User.Requests;

public class AuthUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}