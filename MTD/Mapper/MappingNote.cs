using AutoMapper;
using Models.DbModels;
using Services.DtoModels;

namespace Mapper;

public class MappingNote : Profile
{
    public MappingNote()
    {
        CreateMap<Note, NoteDto>();
        CreateMap<NoteDto, Note>();
    }
}