using AutoMapper;
using MainApp.BLL.Entities;
using MainApp.BLL.Models;

namespace MainApp.Web.Profiles
{
    public class ApplicationUserToViewUserProfile : Profile
    {
        public ApplicationUserToViewUserProfile()
        {
            CreateMap<ApplicationUser, UserView>()
                //.ForMember(d => d.UserRole, o => o.MapFrom(s => $"admin"))
                ;
        }
    }
}
