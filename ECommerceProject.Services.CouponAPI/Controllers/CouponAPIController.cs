using AutoMapper;
using ECommerceProject.Services.CouponAPI.Models;
using ECommerceProject.Services.CouponAPI.Models.Dto;
using ECommerceProject.Services.CouponAPI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProject.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public CouponAPIController(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet("all")]
        public async Task<ResponseDto> GetAllCoupons()
        {
            try
            {
                var coupons = await _couponRepository.GetAllAsync();
                _response.Result = _mapper.Map<List<CouponDtoResponse>>(coupons);
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpGet("couponId")]
        public async Task<ResponseDto> GetCouponById(int couponId)
        {
            try
            {
                var coupon = await _couponRepository.GetByIdAsync(couponId);

                if (coupon == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Coupon not found";
                }
                _response.Result = _mapper.Map<CouponDtoResponse>(coupon);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return _response;
        }

        [HttpGet("couponCode")]
        public async Task<ResponseDto> GetCouponByCode(string couponCode)
        {
            try
            {
                var coupon = await _couponRepository.GetByCodeAsync(couponCode);

                if (coupon == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Coupon not found";
                }
                _response.Result = _mapper.Map<CouponDtoResponse>(coupon);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return _response;
        }

        [HttpPost("create-coupon")]
        public async Task<ResponseDto> CreateCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                await _couponRepository.AddAsync(coupon);
                _response.Result = _mapper.Map<CouponDtoResponse>(coupon);
               
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return _response;

        }

        [HttpPut("update-coupon")]
        public async Task<ResponseDto> UpdateCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                await _couponRepository.UpdateAsync(coupon);
                _response.Result = _mapper.Map<CouponDtoResponse>(coupon);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return _response;
        }

        [HttpDelete("delete/{id}")]
        public async Task<ResponseDto> DeleteCoupon(int id)
        {
            try
            {
                await _couponRepository.DeleteAsync(id);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return _response;
        }

    }
}
