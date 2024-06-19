using AutoMapper;
using ECommerceProject.Services.ProductAPI.Models;
using ECommerceProject.Services.ProductAPI.Models.Dto;
using ECommerceProject.Services.ProductAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProject.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductAPIController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("products")]
        public async Task<ActionResult<ResponseDto>> GetAllProducts()
        {
            var response = new ResponseDto();
            try
            {
                var products = await _productRepository.GetAllAsync();
                response.Result = _mapper.Map<List<ProductDtoResponse>>(products);
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ResponseDto>> GetProductById(int productId)
        {
            var response = new ResponseDto();
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Product not found";
                    return NotFound(response);
                }

                response.Result = _mapper.Map<ProductDtoResponse>(product);
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> CreateProduct([FromBody] ProductDto productDto)
        {
            var response = new ResponseDto();
            try
            {
                var product = _mapper.Map<Product>(productDto);
                var isSuccess = await _productRepository.AddAsync(product);
                if (!isSuccess)
                {
                    response.IsSuccess = false;
                    response.Message = "Failed to create product";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                response.Result = _mapper.Map<ProductDtoResponse>(product);
                response.Message = "Product created successfully";
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> UpdateProduct([FromBody] ProductDtoResponse productDto)
        {
            var response = new ResponseDto();
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(productDto.ProductId);
                if (existingProduct == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Product not found";
                    return NotFound(response);
                }

                _mapper.Map(productDto, existingProduct);

                var isSuccess = await _productRepository.UpdateAsync(existingProduct);
                if (!isSuccess)
                {
                    response.IsSuccess = false;
                    response.Message = "Failed to update product";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                response.Result = _mapper.Map<ProductDtoResponse>(existingProduct);
                response.Message = "Product updated successfully";
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpDelete("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> DeleteProduct(int productId)
        {
            var response = new ResponseDto();
            try
            {
                var isSuccess = await _productRepository.DeleteAsync(productId);
                if (!isSuccess)
                {
                    response.IsSuccess = false;
                    response.Message = "Failed to delete product";
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }

                response.Message = "Product deleted successfully";
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
