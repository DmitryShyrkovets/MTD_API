namespace Services.Models.Note;

public class NoteModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDone { get; set; } = false;
    public DateTime CreateAt { get; set; }
    public DateTime? DoneAt { get; set; }
}