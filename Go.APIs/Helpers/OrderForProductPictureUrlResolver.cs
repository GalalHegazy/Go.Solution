using AutoMapper;
using Go.APIs.Dtos.OrderDtos;
using Go.Core.Entities.Order_Aggregate;
using Go.Core.Entities.Product;

namespace Go.APIs.Helpers
{
    public class OrderForProductPictureUrlResolver : IValueResolver<OrderItems, OrderItemsDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderForProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItems source, OrderItemsDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["ApiPictureUrl"]}/{source.Product.PictureUrl}";

            return string.Empty;
        }
    }
}
