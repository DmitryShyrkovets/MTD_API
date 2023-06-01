using AutoMapper;
using Mapper;
using RepositoryForTest;
using Services;
using Services.Models.Note.Requests;

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
        
        var notes = await _service.GetNotes(userId);

        Assert.AreEqual(3, notes.Count);
    }
    
    [Test]
    public async Task GetNote()
    {
        int userId = 1;
        int noteId = 1;
        
        var note = await _service.GetNote(userId, noteId);

        Assert.IsNotNull(note);
    }
    
    [Test]
    public async Task CreateNote()
    {
        var createNoteRequest = new CreateNoteRequest
        {
            UserId = 1,
            Name = "TestName",
            Description = "testText"
        };

        await _service.TryAddNote(createNoteRequest);
        
        var notes = await _service.GetNotes(createNoteRequest.UserId);

        Assert.AreEqual(4, notes.Count);
    }
    
    [Test]
    public async Task UpdateNote()
    {
        var updateNoteRequest = new UpdateNoteRequest
        {
            Id = 3,
            UserId = 1,
            Name = "TestName",
            Description = "testText",
            IsDone = true
        };

        await _service.TryUpdateNote(updateNoteRequest);
        
        var note = await _service.GetNote(updateNoteRequest.UserId, updateNoteRequest.Id);

        Assert.IsNotNull(note);
        Assert.AreEqual(note.Name, "TestName");
        Assert.AreEqual(note.Description, "testText");
        Assert.AreEqual(note.IsDone, true);
    }
    
    [Test]
    public async Task DeleteNote()
    {
        var deleteNoteRequest = new DeleteNoteRequest
        {
            Id = 2,
            UserId = 1
        };
        
        await _service.TryDeleteNote(deleteNoteRequest);
        
        var notes = await _service.GetNotes(deleteNoteRequest.UserId);
        var note = notes.FirstOrDefault(n => n.Id == 2);

        Assert.IsNull(note);
    }
}