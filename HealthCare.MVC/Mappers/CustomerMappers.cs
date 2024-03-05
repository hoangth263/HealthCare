using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;

namespace HealthCare.MVC.Mappers
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<Customer, CustomerCreateModel>().ReverseMap();
        }
    }
}
