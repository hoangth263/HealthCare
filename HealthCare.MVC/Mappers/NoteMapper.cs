using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;

namespace HealthCare.MVC.Mappers
{
    public class NoteMapper : Profile
    {
        public NoteMapper()
        {
            CreateMap<Note, NoteCreateModel>().ReverseMap();
            CreateMap<Note, NoteUpdateModel>().ReverseMap();
        }
    }
}
