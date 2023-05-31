using AutoMapper;
using Models.DbModels;
using Services.Models.Note;
using Services.Models.Note.Requests;

namespace Mapper;

public class MappingNote : Profile
{
    public MappingNote()
    {
        CreateMap<Note, NoteModel>();
        CreateMap<NoteModel, Note>();

        CreateMap<CreateNoteRequest, Note>()
            .ForMember(dest => dest.Done, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<UpdateNoteRequest, Note>();
        
        CreateMap<DeleteNoteRequest, Note>();
    }
}