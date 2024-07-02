using AutoMapper;
using Clientes.Domain.Models;
using Clientes.Infra.RabbitMQ.Models.cs;

namespace Clientes.Infra.RabbitMQ.Mappings
{
    public class MessageMapper : Profile
    {
        public MessageMapper()
        {
            CreateMap<Cliente, Message>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(x => x.Cpf, opt => opt.MapFrom(src => src.Cpf))
                .ForMember(x => x.Nascimento, opt => opt.MapFrom(src => src.Nascimento))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
