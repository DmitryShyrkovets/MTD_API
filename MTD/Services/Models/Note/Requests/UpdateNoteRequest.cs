namespace Services.Models.Note.Requests;

public class UpdateNoteRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Done { get; set; }
}