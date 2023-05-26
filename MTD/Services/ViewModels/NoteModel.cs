namespace Services.ViewModels;

public class NoteModel
{
    public int? Id { get; set; }
    public int? UserId { get; set; }
    public string? Category { get; set; }
    public string? Name { get; set; }
    public string? Text { get; set; }
}