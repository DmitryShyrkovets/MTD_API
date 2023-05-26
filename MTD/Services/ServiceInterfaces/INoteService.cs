using Services.ViewModels;

namespace Services.ServiceInterfaces;

public interface INoteService
{
    public Task<List<NoteModel>> TryGetNotes(int userId);
    public Task TryAddNote(NoteModel noteModel);
    public Task TryModifyNote(NoteModel model);
    public Task TryDeleteNote(NoteModel model);
}