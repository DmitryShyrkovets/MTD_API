using AutoMapper;
using Models.DbModels;
using Services.ViewModels;

namespace Mapper;

public class MappingNote : Profile
{
    public MappingNote()
    {
        CreateMap<Note, NoteModel>();
        CreateMap<NoteModel, Note>();
    }
}