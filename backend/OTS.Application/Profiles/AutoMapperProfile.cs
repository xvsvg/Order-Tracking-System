using AutoMapper;
using OTS.Application.Dto;
using OTS.Domain.Domain.Core.Implementations;

namespace OTS.Application.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Courier, CourierDto>();
        CreateMap<Customer, CourierDto>();
        CreateMap<CourierDto, CourierDto>();
        CreateMap<Order, OrderDto>();
    }
}