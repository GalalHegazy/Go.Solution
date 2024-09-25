using AutoMapper;
using Go.APIs.Dtos;
using Go.APIs.Errors;
using Go.Core.Entities.Basket;
using Go.Core.Repositories.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Go.APIs.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository
                               ,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet/*("id")*/]  // api/Basket?id= //That's only  EndPoint Use Method Get
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            return Ok( basket is null ? new CustomerBasket(id)  : basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdateBasket(CustomerBasketDto basketDto)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basketDto);

            var CreateOrUpdate = await _basketRepository.UpdateBasketAsync(mappedBasket);

            if (CreateOrUpdate is null) return BadRequest(new APIResponce(400));

            return Ok(CreateOrUpdate);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }


    }
}
