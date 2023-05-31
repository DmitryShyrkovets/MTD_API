namespace Services.Models.User.Requests;

public class UpdateEmailRequest
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}