using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using Models.RepositoryInterfaces;

namespace Repository;

public class WorkingWithNotes: INoteRepository
{
    private readonly ApplicationContext _context;
    public WorkingWithNotes(ApplicationContext context)
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

    public async Task ModifyNote(Note model)
    {
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == model.Id);
        
        if (note == null)
        {
            throw new Exception("Note is not found!");
        }

        note.Name = model.Name;
        note.Text = model.Text;
        note.Category = model.Category;
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNote(int id)
    {
        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
        
        if (note == null)
        {
            throw new Exception("Note is not found!");
        }

        _context.Notes.Remove(note);
        
        await _context.SaveChangesAsync();
    }
}