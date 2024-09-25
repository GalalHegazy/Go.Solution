using AdminDashBoard.ViewModels.Products;
using AdminDashBoard.ViewModels.Roles;
using AdminDashBoard.ViewModels.Users;
using AutoMapper;
using Go.Core.Entities.Identity;
using Go.Core.Entities.Product;
using Microsoft.AspNetCore.Identity;

namespace AdminDashBoard.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
