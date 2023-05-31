using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Models.DbModels;
using Models.RepositoryInterfaces;
using Services.ServiceInterfaces;
using Services.DtoModels;

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


    public async Task<List<NoteDto>> TryGetNotes(int userId)
    {
        if (userId == 0)
            throw new Exception("User id is null");

        var notesCache = _cache?.Get<List<NoteDto>>("notes" + userId);

        if (notesCache is null)
        {
            var notes = await _repository.GetNotes(userId);
            var noteModels = _mapper.Map<List<NoteDto>>(notes);
            
            _cache?.Set("notes" + userId, noteModels, options);

            return noteModels;
        }
        

        return notesCache;
    }

    public async Task TryAddNote(NoteDto noteDto)
    {
        if (string.IsNullOrEmpty(noteDto.Name) || string.IsNullOrEmpty(noteDto.Description))
            throw new Exception("We must enter data!");

        if (noteDto.UserId is null || noteDto.UserId == 0)
            throw new Exception("User id is null!");
        
        var note = _mapper.Map<Note>(noteDto);
        
        note.CreateAt = DateTime.Now;
        note.Done = false;
        note.DoneAt = null;

        await _repository.AddNote(note);

        await RewriteCache(noteDto.UserId ?? 0);
    }

    public async Task TryUpdateNote(NoteDto dto)
    {
        if (dto.Id is null || string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Description))
            throw new Exception("We must enter data! ");
        
        if (dto.UserId is null || dto.UserId == 0)
            throw new Exception("User id is null!");

        var note = _mapper.Map<Note>(dto);

        if (note.Done)
            note.DoneAt = DateTime.Now;
        else
            note.DoneAt = null;

        await _repository.UpdateNote(note);
        
        await RewriteCache(dto.UserId ?? 0);
    }

    public async Task TryDeleteNote(NoteDto dto)
    {
        await _repository.DeleteNote(dto.Id ?? throw new Exception("Note id is null! "));
        
        await RewriteCache(dto.UserId ?? throw new Exception("User id is null! "));
    }

    private async Task RewriteCache(int userId)
    {
        _cache?.Remove("notes" + userId);
        
        var notes = await TryGetNotes(userId);

        _cache?.Set("notes" +userId, notes, options);
    }
}