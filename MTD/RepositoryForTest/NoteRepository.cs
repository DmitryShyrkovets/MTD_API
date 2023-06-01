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

    public async Task<Note> GetNote(int userId, int noteId)
    {
        return _context.Notes.FirstOrDefault(n => n.Id == noteId && n.UserId == userId);
    }

    public async Task AddNote(Note note)
    {
        var userDb = _context.Users.FirstOrDefault(n => n.Id == note.UserId);
        
        if (userDb is null)
            throw new Exception("User for note is not found!");
        
        _context.Notes.Add(note);
    }

    public async Task UpdateNote(Note note)
    {
        var noteDb = _context.Notes.FirstOrDefault(n => n.Id == note.Id && n.UserId == note.UserId);
        
        if (noteDb is null)
            throw new Exception("Note is not found!");

        noteDb.Name = note.Name;
        noteDb.Description = note.Description;
        noteDb.IsDone = note.IsDone;
        noteDb.DoneAt = note.DoneAt;

    }

    public async Task DeleteNote(Note note)
    {
        var noteDb = _context.Notes.FirstOrDefault(n => n.Id == note.Id && n.UserId == note.UserId);
        
        if (noteDb is null)
            throw new Exception("Note is not found!");

        _context.Notes.Remove(noteDb);
    }
}