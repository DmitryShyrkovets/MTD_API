namespace Services.Models.Note.Requests;

public class CreateNoteRequest
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}