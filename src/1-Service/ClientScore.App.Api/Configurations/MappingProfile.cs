using AutoMapper;
using ClientScore.App.Domain.Models;
using ClientScore.App.Domain.ViewModels;

namespace ClientScore.App.Api.Configurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClienteViewModel, Cliente>().ReverseMap();
        CreateMap<EnderecoViewModel, Endereco>().ReverseMap();
    }
}
