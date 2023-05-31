using Models.DbModels;
using Models.RepositoryInterfaces;

namespace RepositoryForTest;

public class NoteRepository : INoteRepository
{
    private readonly AppContextLocal _context;

    public NoteRepository()
    {
        _context = new AppContextLocal();
    }
    public async Task<List<Note>> GetNotes(int userId)
    {
        return _context.Notes.Where(n => n.UserId == userId).ToList();
    }

    public async Task AddNote(Note model)
    {
        _context.Notes.Add(model);
    }

    public async Task UpdateNote(Note model)
    {
        var note = _context.Notes.FirstOrDefault(n => n.Id == model.Id);
        
        if (note is null)
            throw new Exception("Note is not found!");

        note.Name = model.Name;
        note.Description = model.Description;
        note.Done = model.Done;
        note.DoneAt = model.DoneAt;

    }

    public async Task DeleteNote(int id)
    {
        var note = _context.Notes.FirstOrDefault(n => n.Id == id);
        
        if (note is null)
            throw new Exception("Note is not found!");

        _context.Notes.Remove(note);
    }
}