using Services.Models.Note;
using Services.Models.Note.Requests;

namespace Services.ServiceInterfaces;

public interface INoteService
{
    public Task<List<NoteModel>> GetNotes(int userId);
    public Task<NoteModel> GetNote(int userId, int noteId);
    public Task TryAddNote(CreateNoteRequest createNoteRequest);
    public Task TryUpdateNote(UpdateNoteRequest updateNoteRequest);
    public Task TryDeleteNote(DeleteNoteRequest deleteNoteRequest);
}