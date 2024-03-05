﻿using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;

namespace HealthCare.MVC.Mappers
{
    public class AgentMapper : Profile
    {
        public AgentMapper()
        {
            CreateMap<Agent, AgentCreateModel>().ReverseMap();
            CreateMap<Agent, AgentViewModel>().ReverseMap();
        }
    }
}
