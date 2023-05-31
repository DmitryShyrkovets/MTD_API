namespace Services.Models.Note.Requests;

public class DeleteNoteRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
}