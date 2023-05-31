using Services.DtoModels;

namespace Services.ServiceInterfaces;

public interface INoteService
{
    public Task<List<NoteDto>> TryGetNotes(int userId);
    public Task TryAddNote(NoteDto noteDto);
    public Task TryUpdateNote(NoteDto dto);
    public Task TryDeleteNote(NoteDto dto);
}