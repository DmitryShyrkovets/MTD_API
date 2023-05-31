using Models.DbModels;

namespace Models.RepositoryInterfaces;

public interface INoteRepository
{
    public Task<List<Note>> GetNotes(int userId);
    public Task<Note> GetNote(int userId, int noteId);
    public Task AddNote(Note note);
    public Task UpdateNote(Note note);
    public Task DeleteNote(Note note);
}