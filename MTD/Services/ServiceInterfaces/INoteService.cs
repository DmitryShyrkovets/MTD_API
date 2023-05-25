using Services.ViewModels;

namespace Services.ServiceInterfaces;

public interface INoteService
{
    public Task<List<NoteModel>> TryGetNotes(UserModel userModel);
    public Task TryAddNote(NoteModel noteModel, UserModel userModel);
    public Task TryModifyNote(NoteModel model);
    public Task TryDeleteNote(int id);
}