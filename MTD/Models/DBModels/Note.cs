namespace Models.DbModels;

public class Note
{
    public int Id { get; set; }
    public string? Category { get; set; }
    public string? Name { get; set; }
    public string? Text { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
}