using AutoMapper;
using Mapper;
using RepositoryForTest;
using Services;
using Services.ViewModels;

namespace LocalTesting;

public class NotesTests
{
    private NoteService _service;
    private WorkingWithNotes repository;
    
    [SetUp]
    public void Setup()
    {
        MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingNote());
            mc.AddProfile(new MappingUser());
        });

        IMapper mapper = mappingConfig.CreateMapper();

        repository = new WorkingWithNotes();

        _service = new NoteService(repository, mapper);
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
        var newNote = new NoteModel
        {
            Id = 10,
            UserId = 1,
            Category = "testCategory",
            Name = "TestName",
            Text = "testText"
        };

        await _service.TryAddNote(newNote);
        
        var notes = await _service.TryGetNotes(newNote.UserId ?? 0);

        Assert.AreEqual(4, notes.Count);
    }
    
    [Test]
    public async Task ModifyNote()
    {
        var modifyNote = new NoteModel
        {
            Id = 3,
            UserId = 1,
            Category = "testCategory",
            Name = "TestName",
            Text = "testText"
        };

        await _service.TryAddNote(modifyNote);
        
        var notes = await _service.TryGetNotes(modifyNote.UserId ?? 0);

        var note = notes.FirstOrDefault(n => n.Id == modifyNote.Id && 
                                             n.Category == modifyNote.Category && 
                                             n.Name == modifyNote.Name && 
                                             n.Text == modifyNote.Text);

        Assert.IsNotNull(note);
    }
    
    [Test]
    public async Task DeleteNote()
    {
        var deletedNote = new NoteModel
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