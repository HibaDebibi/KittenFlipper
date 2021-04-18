using AutoMapper;
using KittenFlipper.Entitites;
using KittenFlipper.Models.User;

namespace KittenFlipper.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}
