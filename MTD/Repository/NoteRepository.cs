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

    public async Task AddNote(Note model)
    {
        _context.Notes.Add(model);
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNote(Note model)
    {
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == model.Id);
        
        if (note is null)
            throw new Exception("Note is not found!");

        note.Name = model.Name;
        note.Description = model.Description;
        note.Done = model.Done;
        note.DoneAt = model.DoneAt;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNote(int id)
    {
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
        
        if (note is null)
            throw new Exception("Note is not found!");

        _context.Notes.Remove(note);
        
        await _context.SaveChangesAsync();
    }
}