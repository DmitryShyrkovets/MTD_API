using AutoMapper;
using Mapper;
using RepositoryForTest;
using Services;
using Services.DtoModels;

namespace LocalTesting;

public class NotesTests
{
    private NoteService _service;
    private NoteRepository repository;
    
    [SetUp]
    public void Setup()
    {
        MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingNote());
            mc.AddProfile(new MappingUser());
        });

        IMapper mapper = mappingConfig.CreateMapper();

        repository = new NoteRepository();

        _service = new NoteService(repository, mapper, null);
    }
    
    [Test]
    public async Task GetNotes()
    {
        int userId = 1;
        
        var notes = await _service.TryGetNotes(userId);

        Assert.AreEqual(3, notes.Count);
    }
    
    [Test]
    public async Task CreateNote()
    {
        var newNote = new NoteDto
        {
            Id = 10,
            UserId = 1,
            Name = "TestName",
            Description = "testText",
            CreateAt = DateTime.Now
        };

        await _service.TryAddNote(newNote);
        
        var notes = await _service.TryGetNotes(newNote.UserId ?? 0);

        Assert.AreEqual(4, notes.Count);
    }
    
    [Test]
    public async Task UpdateNote()
    {
        var modifyNote = new NoteDto
        {
            Id = 3,
            UserId = 1,
            Name = "TestName",
            Description = "testText",
            Done = true
        };

        await _service.TryUpdateNote(modifyNote);
        
        var notes = await _service.TryGetNotes(modifyNote.UserId ?? 0);

        var note = notes.FirstOrDefault(n => n.Id == modifyNote.Id && 
                                             n.Name == modifyNote.Name && 
                                             n.Description == modifyNote.Description &&
                                             n.Done == modifyNote.Done);

        Assert.IsNotNull(note);
    }
    
    [Test]
    public async Task DeleteNote()
    {
        var deletedNote = new NoteDto
        {
            Id = 2,
            UserId = 1
        };
        
        await _service.TryDeleteNote(deletedNote);
        
        var notes = await _service.TryGetNotes(deletedNote.UserId ?? 0);
        var note = notes.FirstOrDefault(n => n.Id == 2);

        Assert.IsNull(note);
    }
}