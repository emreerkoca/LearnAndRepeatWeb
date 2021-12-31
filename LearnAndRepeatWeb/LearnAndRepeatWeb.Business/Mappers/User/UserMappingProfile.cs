using AutoMapper;
using LearnAndRepeatWeb.Contracts.Responses.User;
using LearnAndRepeatWeb.Infrastructure.Entities.User;

namespace LearnAndRepeatWeb.Business.Mappers.User
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserModel, PostUserResponse>();
            CreateMap<UserTokenModel, UserTokenResponse>();
        }
    }
}
