using AutoMapper;
using ECommerceProject.EventBridge;
using ECommerceProject.Services.ShoppingCartAPI.Models;
using ECommerceProject.Services.ShoppingCartAPI.Models.Dto;
using ECommerceProject.Services.ShoppingCartAPI.Repository.IRepository;
using ECommerceProject.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerceProject.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private IProductService _productService;
        private ICouponService _couponService;
        private readonly IEventPublisher _eventPublisher;

        public ShoppingCartAPIController(ICartRepository cartRepository, 
            IMapper mapper, IProductService productService, ICouponService couponService, IEventPublisher eventPublisher)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;
            _eventPublisher = eventPublisher;
        }

        [HttpGet("get_cart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            var _response = new ResponseDto();
            try
            {
                CartDto cart = new CartDto();

                // Fetch CartHeader using repository
                var cartHeader = await _cartRepository.GetAllAsync<CartHeader>()
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (cartHeader != null)
                {
                    cart.CartHeader = _mapper.Map<CartHeaderDto>(cartHeader);

                    // Fetch CartDetails using repository
                    var cartDetails = await _cartRepository.GetAllAsync<CartDetails>()
                        .Where(u => u.CartHeaderId == cartHeader.CartHeaderId)
                        .ToListAsync();

                    cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(cartDetails);

                    // Fetch products using ProductService
                    IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                    foreach (var item in cart.CartDetails)
                    {
                        // Map product to CartDetailsDto
                        item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);

                        if (item.Product != null)
                        {
                            // Calculate CartTotal
                            cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                        }
                        else
                        {
                            // Log missing product for debugging
                            Console.WriteLine($"Product not found for ProductId: {item.ProductId}");
                        }
                    }

                    // Apply coupon if any
                    if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                    {
                        CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                        if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                        {
                            cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                            cart.CartHeader.Discount = coupon.DiscountAmount;
                        }
                    }

                    _response.Result = cart;
                    _response.IsSuccess = true;
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Cart not found for the specified user.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                // Log exception if needed
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            return _response;
        }


        [HttpPost("apply_coupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cartDto)
        {
            var response = new ResponseDto();

            try
            {
                var cartFromDb = await _cartRepository.GetAllAsync<CartHeader>()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);

                if (cartFromDb == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Cart not found for the user.";
                    return response;
                }

                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                await _cartRepository.UpdateAsync(cartFromDb);

                response.IsSuccess = true;
                response.Result = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }


        [HttpPost("email_cart_request")]
        public async Task<object> EmailCartRequest([FromBody] CartDto cartDto)
        {
            var response = new ResponseDto();
            try
            {
                await _eventPublisher.PublishEvent(cartDto, "Cart Sent", "olubanke.eboda.ecommerceweb");
                response.Result = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.ToString();
            }
            return response;
        }


        [HttpPost("upsert_cart")]
        public async Task<ResponseDto> UpsertCart(CartDto cartDto)
        {
            var response = new ResponseDto();
            try
            {
                var cartHeader = await _cartRepository.GetAllAsync<CartHeader>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);

                if (cartHeader == null)
                {
                    // Create header and details
                    var newCartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    await _cartRepository.AddAsync(newCartHeader);

                    var newCartDetail = cartDto.CartDetails.First();
                    newCartDetail.CartHeaderId = newCartHeader.CartHeaderId;
                    await _cartRepository.AddAsync(_mapper.Map<CartDetails>(newCartDetail));

                    response.Message = "Cart created successfully";
                }
                else
                {
                    // If header is not null, check if details have the same product
                    var cartDetail = cartDto.CartDetails.First();
                    var cartDetails = await _cartRepository.GetAllAsync<CartDetails>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.ProductId == cartDetail.ProductId && u.CartHeaderId == cartHeader.CartHeaderId);

                    if (cartDetails == null)
                    {
                        // Create cart details
                        cartDetail.CartHeaderId = cartHeader.CartHeaderId;
                        await _cartRepository.AddAsync(_mapper.Map<CartDetails>(cartDetail));
                        response.Message = "Cart details created successfully";
                    }
                    else
                    {
                        // Update count in cart details
                        cartDetail.Count += cartDetails.Count;
                        cartDetail.CartHeaderId = cartDetails.CartHeaderId;
                        cartDetail.CartDetailsId = cartDetails.CartDetailsId;
                        await _cartRepository.UpdateAsync(_mapper.Map<CartDetails>(cartDetail));
                        response.Message = "Cart updated successfully";
                    }
                }

                response.Result = cartDto;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        [HttpPost("remove_cart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            var response = new ResponseDto();
            try
            {
                var cartDetails = await _cartRepository.GetByIdAsync<CartDetails>(cartDetailsId);
                if (cartDetails == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Cart detail not found";
                    return response;
                }

                var cartDetailsList = _cartRepository.GetAllAsync<CartDetails>()
                    .Where(u => u.CartHeaderId == cartDetails.CartHeaderId).ToList();

                int totalCountofCartItem = cartDetailsList.Count();

                await _cartRepository.DeleteAsync(cartDetails);

                if (totalCountofCartItem == 1)
                {
                    var cartHeader = await _cartRepository.GetByIdAsync<CartHeader>(cartDetails.CartHeaderId);
                    if (cartHeader != null)
                    {
                        await _cartRepository.DeleteAsync(cartHeader);
                    }
                }

                response.Result = true;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.IsSuccess = false;
            }
            return response;
        }




    }
}

