using Models.DbModels;
using Models.RepositoryInterfaces;

namespace RepositoryForTest;

public class WorkingWithNotes : INoteRepository
{
    private readonly AppContextLocal _context;

    public WorkingWithNotes()
    {
        _context = new AppContextLocal();
    }
    public async Task<List<Note>> GetNotes(User user)
    {
        return _context.Notes.Where(n => n.UserId == user.Id).ToList();
    }

    public async Task AddNote(Note model, User user)
    {
        model.UserId = user.Id;
        
        _context.Notes.Add(model);
    }

    public async Task ModifyNote(Note model)
    {
        var note = _context.Notes.FirstOrDefault(n => n.Id == model.Id);
        
        if (note == null)
        {
            throw new Exception("Note is not found!");
        }

        note.Name = model.Name;
        note.Text = model.Text;
        note.Category = model.Category;
    }

    public async Task DeleteNote(int id)
    {
        var note = _context.Notes.FirstOrDefault(n => n.Id == id);
        
        if (note == null)
        {
            throw new Exception("Note is not found!");
        }

        _context.Notes.Remove(note);
    }
}