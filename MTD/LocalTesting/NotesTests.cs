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
        var user = new UserModel
        {
            Id = 1
        };
        
        var notes = await _service.TryGetNotes(user);

        Assert.AreEqual(3, notes.Count);
    }
    
    [Test]
    public async Task CreateNote()
    {
        var user = new UserModel
        {
            Id = 1
        };
        
        var newNote = new NoteModel
        {
            Id = 10,
            Category = "testCategory",
            Name = "TestName",
            Text = "testText"
        };

        await _service.TryAddNote(newNote, user);
        
        var notes = await _service.TryGetNotes(user);

        Assert.AreEqual(4, notes.Count);
    }
    
    [Test]
    public async Task ModifyNote()
    {
        var user = new UserModel
        {
            Id = 1
        };
        
        var modifyNote = new NoteModel
        {
            Id = 3,
            Category = "testCategory",
            Name = "TestName",
            Text = "testText"
        };

        await _service.TryAddNote(modifyNote, user);
        
        var notes = await _service.TryGetNotes(user);

        var note = notes.FirstOrDefault(n => n.Id == modifyNote.Id && 
                                             n.Category == modifyNote.Category && 
                                             n.Name == modifyNote.Name && 
                                             n.Text == modifyNote.Text);

        Assert.IsNotNull(note);
    }
    
    [Test]
    public async Task DeleteNote()
    {
        var user = new UserModel
        {
            Id = 1
        };
        
        await _service.TryDeleteNote(2);
        
        var notes = await _service.TryGetNotes(user);
        var note = notes.FirstOrDefault(n => n.Id == 2);

        Assert.IsNull(note);
    }
}