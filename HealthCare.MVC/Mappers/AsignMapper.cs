using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;

namespace HealthCare.MVC.Mappers
{
    public class AsignMapper : Profile
    {
        public AsignMapper()
        {
            CreateMap<Asign, AsignCreateModel>().ReverseMap();
            CreateMap<Asign, AsignUpdateModel>().ReverseMap();
            CreateMap<Asign, AsignDetailsModel>().ReverseMap();
        }
    }
}
