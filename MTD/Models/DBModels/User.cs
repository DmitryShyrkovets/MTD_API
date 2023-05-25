namespace Models.DbModels;

public class User
{
    public int Id { get; set; }
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public List<Note>? Notes { get; set; }

}