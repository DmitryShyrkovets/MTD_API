using Models.DbModels;

namespace Models.RepositoryInterfaces;

public interface INoteRepository
{
    public Task<List<Note>> GetNotes(User user);
    public Task AddNote(Note model, User user);
    public Task ModifyNote(Note model);
    public Task DeleteNote(int id);
}