using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Models.DbModels;
using Models.RepositoryInterfaces;
using Services.ServiceInterfaces;
using Services.ViewModels;

namespace Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _repository;
    private readonly IMapper _mapper;
    private IMemoryCache _cache;
    
    private readonly MemoryCacheEntryOptions options;
    
    public NoteService(INoteRepository repository, IMapper mapper, IMemoryCache cache = null)
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

        var notesCache = _cache?.Get<List<NoteModel>>("notes" + userId);

        if (notesCache == null)
        {
            var notes = await _repository.GetNotes(userId);
            var noteModels = _mapper.Map<List<NoteModel>>(notes);
            
            _cache?.Set("notes" + userId, noteModels, options);

            return noteModels;
        }
        

        return notesCache;
    }

    public async Task TryAddNote(NoteModel noteModel)
    {
        if (noteModel.Category == null ||noteModel.Name == null || noteModel.Text == null)
            throw new Exception("We must enter data!");

        if (noteModel.UserId == null || noteModel.UserId == 0)
            throw new Exception("User id is null!");
        
        var note = _mapper.Map<Note>(noteModel);

        await _repository.AddNote(note);

        await RewriteCache(noteModel.UserId ?? 0);
    }

    public async Task TryModifyNote(NoteModel model)
    {
        if (model.Id == null || model.Category == null ||model.Name == null || model.Text == null)
            throw new Exception("We must enter data! ");
        
        if (model.UserId == null || model.UserId == 0)
            throw new Exception("User id is null!");

        var note = _mapper.Map<Note>(model);
        
        await _repository.ModifyNote(note);
        
        await RewriteCache(model.UserId ?? 0);
    }

    public async Task TryDeleteNote(NoteModel model)
    {
        await _repository.DeleteNote(model.Id ?? throw new Exception("Note id is null! "));
        
        await RewriteCache(model.UserId ?? throw new Exception("User id is null! "));
    }

    private async Task RewriteCache(int userId)
    {
        _cache?.Remove("notes" + userId);
        
        var notes = await TryGetNotes(userId);

        _cache?.Set("notes" +userId, notes, options);
    }
}