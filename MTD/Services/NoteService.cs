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


    public async Task<List<NoteModel>> TryGetNotes(int userId)
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

    public async Task TryAddNote(CreateNoteRequest createNoteRequest)
    {
        if (string.IsNullOrEmpty(createNoteRequest.Name) || string.IsNullOrEmpty(createNoteRequest.Description))
            throw new Exception("We must enter data!");

        if (createNoteRequest.UserId == 0)
            throw new Exception("User id is null!");
        
        var noteDb = _mapper.Map<Note>(createNoteRequest);

        await _repository.AddNote(noteDb);

        await RewriteCache(noteDb.UserId);
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
        
        await RewriteCache(noteDb.UserId);
    }

    public async Task TryDeleteNote(DeleteNoteRequest deleteNoteRequest)
    {
        if (deleteNoteRequest.Id == 0)
            throw new Exception("Note id is null!");
        if (deleteNoteRequest.UserId == 0)
            throw new Exception("User id is null!");
        
        var noteDb = _mapper.Map<Note>(deleteNoteRequest);
        
        await _repository.DeleteNote(noteDb);
        
        await RewriteCache(deleteNoteRequest.UserId);
    }

    private async Task RewriteCache(int userId)
    {
        _cache?.Remove("notes" + userId);
        
        var notes = await TryGetNotes(userId);

        _cache?.Set("notes" +userId, notes, options);
    }
}