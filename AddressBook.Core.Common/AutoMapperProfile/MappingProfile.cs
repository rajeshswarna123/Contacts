using AddressBook.Core.Models;
using AddressBook.Core.Models.Request;
using AutoMapper;

namespace AddressBook.Core.Common.AutoMapperProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginRequest, Entities.User>();
            CreateMap<RegisterRequest, Entities.User>();
            CreateMap<ContactRequest, Entities.Contact>();

            CreateMap<User, Entities.User>().ReverseMap();
            CreateMap<Contact, Entities.Contact>().ReverseMap();

        }
    }
}
