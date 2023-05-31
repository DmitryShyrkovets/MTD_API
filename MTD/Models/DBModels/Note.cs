namespace Models.DbModels;

public class Note
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Done { get; set; } = false;
    public DateTime CreateAt { get; set; }
    public DateTime? DoneAt { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
}