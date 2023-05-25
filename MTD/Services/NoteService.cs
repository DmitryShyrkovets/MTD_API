using AutoMapper;
using Models.DbModels;
using Models.RepositoryInterfaces;
using Services.ServiceInterfaces;
using Services.ViewModels;

namespace Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _repository;
    private readonly IMapper _mapper;
    
    public NoteService(INoteRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


    public async Task<List<NoteModel>> TryGetNotes(UserModel userModel)
    {
        var user = _mapper.Map<User>(userModel);

        var notes = await _repository.GetNotes(user);

        return _mapper.Map<List<NoteModel>>(notes);
    }

    public async Task TryAddNote(NoteModel noteModel, UserModel userModel)
    {
        var note = _mapper.Map<Note>(noteModel);
        var user = _mapper.Map<User>(userModel);
        
        await _repository.AddNote(note, user);
    }

    public async Task TryModifyNote(NoteModel model)
    {
        var note = _mapper.Map<Note>(model);
        
        await _repository.ModifyNote(note);
    }

    public async Task TryDeleteNote(int id)
    {
        await _repository.DeleteNote(id);
    }
}