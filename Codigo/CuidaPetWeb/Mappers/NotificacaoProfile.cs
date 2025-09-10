using AutoMapper;
using Core;
using CuidaPetWeb.Models;

namespace CuidaPetWeb.Mappers
{
    public class NotificacaoProfile : Profile
    {
        public NotificacaoProfile()
        {
            CreateMap<NotificacaoViewModel, Notificacao>().ReverseMap();
        }
    }
}
