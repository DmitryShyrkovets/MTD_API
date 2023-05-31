namespace Services.Models.User.Requests;

public class UpdatePasswordRequest
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string OldPassword { get; set; }
}