using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Models.DbModels;
using Models.RepositoryInterfaces;
using Services.Models.Note;
using Services.Models.Note.Requests;
using Services.ServiceInterfaces;

namespace Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _repository;
    private readonly IMapper _mapper;
    private IMemoryCache _cache;
    
    private readonly MemoryCacheEntryOptions options;
    
    public NoteService(INoteRepository repository, IMapper mapper, IMemoryCache? cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        
        options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
    }


    public async Task<List<NoteModel>> GetNotes(int userId)
    {
        if (userId == 0)
            throw new Exception("User id is null");

        var notes = _cache?.Get<List<NoteModel>>("notes" + userId);

        if (notes is null)
        {
            var notesDb = await _repository.GetNotes(userId);
            
            notes = _mapper.Map<List<NoteModel>>(notesDb);
            
            _cache?.Set("notes" + userId, notes, options);
        }
        

        return notes;
    }
    
    public async Task<NoteModel> GetNote(int userId, int noteId)
    {
        if (userId == 0)
            throw new Exception("User id is null");

        var note = _cache?.Get<NoteModel>(userId +"note" + noteId);

        if (note is null)
        {
            var noteDb = await _repository.GetNote(userId, noteId);

            if (noteDb is null)
                throw new Exception("Note is not found!");
        
            note = _mapper.Map<NoteModel>(noteDb);
            
            _cache?.Set(userId +"note" + noteId, note, options);
        }
        
        return note;
    }

    public async Task TryAddNote(CreateNoteRequest createNoteRequest)
    {
        if (string.IsNullOrEmpty(createNoteRequest.Name) || string.IsNullOrEmpty(createNoteRequest.Description))
            throw new Exception("We must enter data!");

        if (createNoteRequest.UserId == 0)
            throw new Exception("User id is null!");
        
        var noteDb = _mapper.Map<Note>(createNoteRequest);

        await _repository.AddNote(noteDb);
        
        var note = _mapper.Map<NoteModel>(noteDb);

        _cache?.Set(note.UserId +"note" + note.Id, note, options);
        
        await RewriteNotesCache(noteDb.UserId);
    }

    public async Task TryUpdateNote(UpdateNoteRequest updateNoteRequest)
    {
        if (string.IsNullOrEmpty(updateNoteRequest.Name) || string.IsNullOrEmpty(updateNoteRequest.Description))
            throw new Exception("We must enter data! ");
        
        if (updateNoteRequest.UserId == 0)
            throw new Exception("User id is null!");

        var noteDb = _mapper.Map<Note>(updateNoteRequest);

        if (noteDb.Done)
            noteDb.DoneAt = DateTime.Now;
        else
            noteDb.DoneAt = null;

        await _repository.UpdateNote(noteDb);
        
        _cache?.Remove(noteDb.UserId + "note" + noteDb.Id);
        
        var note = await GetNote(noteDb.UserId,  noteDb.Id);

        _cache?.Set(note.UserId +"note" + note.Id, note, options);
        
        await RewriteNotesCache(noteDb.UserId);
    }

    public async Task TryDeleteNote(DeleteNoteRequest deleteNoteRequest)
    {
        if (deleteNoteRequest.Id == 0)
            throw new Exception("Note id is null!");
        if (deleteNoteRequest.UserId == 0)
            throw new Exception("User id is null!");
        
        var noteDb = _mapper.Map<Note>(deleteNoteRequest);

        await _repository.DeleteNote(noteDb);
        
        _cache?.Remove(deleteNoteRequest.UserId + "note" + deleteNoteRequest.Id);

        await RewriteNotesCache(deleteNoteRequest.UserId);
    }

    private async Task RewriteNotesCache(int userId)
    {
        _cache?.Remove("notes" + userId);

        var notes = await GetNotes(userId);
        
        _cache?.Set("notes" +userId, notes, options);

    }
}