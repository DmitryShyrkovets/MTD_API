using Services.Models.Note;
using Services.Models.Note.Requests;

namespace Services.ServiceInterfaces;

public interface INoteService
{
    public Task<List<NoteModel>> TryGetNotes(int userId);
    public Task TryAddNote(CreateNoteRequest createNoteRequest);
    public Task TryUpdateNote(UpdateNoteRequest updateNoteRequest);
    public Task TryDeleteNote(DeleteNoteRequest deleteNoteRequest);
}