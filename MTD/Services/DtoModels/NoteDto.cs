namespace Services.DtoModels;

public class NoteDto
{
    public int? Id { get; set; }
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool Done { get; set; } = false;
    public DateTime CreateAt { get; set; }
    public DateTime? DoneAt { get; set; }
}