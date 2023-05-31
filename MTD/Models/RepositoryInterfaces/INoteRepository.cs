using Models.DbModels;

namespace Models.RepositoryInterfaces;

public interface INoteRepository
{
    public Task<List<Note>> GetNotes(int userId);
    public Task AddNote(Note model);
    public Task UpdateNote(Note model);
    public Task DeleteNote(int id);
}