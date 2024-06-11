using AutoMapper;
using ECommerceProject.Services.CouponAPI.Models;
using ECommerceProject.Services.CouponAPI.Models.Dto;
using ECommerceProject.Services.CouponAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProject.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response = new ResponseDto();

        public CouponAPIController(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<ResponseDto>> GetAllCoupons()
        {
            try
            {
                var coupons = await _couponRepository.GetAllAsync();
                _response.Result = _mapper.Map<List<CouponDtoResponse>>(coupons);
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("{couponId}")]
        public async Task<ActionResult<ResponseDto>> GetCouponById(int couponId)
        {
            try
            {
                var coupon = await _couponRepository.GetByIdAsync(couponId);

                if (coupon == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Coupon not found";
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<CouponDtoResponse>(coupon);
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpGet("code/{couponCode}")]
        public async Task<ActionResult<ResponseDto>> GetCouponByCode(string couponCode)
        {
            try
            {
                var coupon = await _couponRepository.GetByCodeAsync(couponCode);

                if (coupon == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Coupon not found";
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<CouponDtoResponse>(coupon);
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpPost("create-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> CreateCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                await _couponRepository.AddAsync(coupon);
                _response.Result = _mapper.Map<CouponDtoResponse>(coupon);
                _response.IsSuccess = true;
                return CreatedAtAction(nameof(GetCouponById), new { couponId = coupon.CouponId }, _response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpPut("update-coupon")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> UpdateCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                await _couponRepository.UpdateAsync(coupon);
                _response.Result = _mapper.Map<CouponDtoResponse>(coupon);
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
                return StatusCode(500, _response);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> DeleteCoupon(int id)
        {
            try
            {
                var coupon = await _couponRepository.GetByIdAsync(id);

                if (coupon == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Coupon not found";
                    return NotFound(_response);
                }

                await _couponRepository.DeleteAsync(id);
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
                return StatusCode(500, _response);
            }
        }
    }
}
