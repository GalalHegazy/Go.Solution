using AutoMapper;
using Go.APIs.Dtos;
using Go.APIs.Dtos.AccountDtos;
using Go.APIs.Dtos.OrderDtos;
using Go.Core.Entities.Basket;
using Go.Core.Entities.Identity;
using Go.Core.Entities.Order_Aggregate;
using Go.Core.Entities.Product;

namespace Go.APIs.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                      .ForMember(D => D.Brand      ,O => O.MapFrom(P=>P.Brand.Name))
                      .ForMember(D => D.Category   ,O => O.MapFrom(P=>P.Category.Name))
                      .ForMember(D => D.PictureUrl ,O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemsDto, BasketItems>();

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<ShippingAddressDto,ShippingAddress>();

            // To Map DeliveryMethod in OrderToReturnDto 
            CreateMap<Order,OrderToReturnDto>()
                     .ForMember(D => D.DeliveryMethod    , O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                     .ForMember(D => D.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            // To Map OrderItems To OrderItemsDto in OrderToReturnDto 
            CreateMap<OrderItems, OrderItemsDto>()
                     .ForMember(D => D.ProductId  , I => I.MapFrom(S => S.Product.ProductId))
                     .ForMember(D => D.ProductName, I => I.MapFrom(S => S.Product.ProductName))
                     .ForMember(D => D.PictureUrl , I => I.MapFrom(S => S.Product.PictureUrl))
                     .ForMember(D => D.PictureUrl , I => I.MapFrom<OrderForProductPictureUrlResolver>());

        }
    }
}
