using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using Models.RepositoryInterfaces;

namespace Repository;

public class NoteRepository: INoteRepository
{
    private readonly ApplicationContext _context;
    public NoteRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<Note>> GetNotes(int userId)
    {
        return await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
    }

    public async Task<Note> GetNote(int userId, int noteId)
    {
        return await _context.Notes.FirstOrDefaultAsync(n => n.Id == noteId && n.UserId == userId);
    }

    public async Task AddNote(Note note)
    {
        var user = await _context.Users.FirstOrDefaultAsync(n => n.Id == note.UserId);
        
        if (user is null)
            throw new Exception("User for note is not found!");
        
        _context.Notes.Add(note);
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNote(Note note)
    {
        var noteDb = await _context.Notes.FirstOrDefaultAsync(n => n.Id == note.Id && n.UserId == note.UserId);
        
        if (noteDb is null)
            throw new Exception("Note is not found!");

        noteDb.Name = note.Name;
        noteDb.Description = note.Description;
        noteDb.Done = note.Done;
        noteDb.DoneAt = note.DoneAt;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNote(Note note)
    {
        var noteDb = await _context.Notes.FirstOrDefaultAsync(n => n.Id == note.Id && n.UserId == note.UserId);
        
        if (noteDb is null)
            throw new Exception("Note is not found!");

        _context.Notes.Remove(noteDb);
        
        await _context.SaveChangesAsync();
    }
}