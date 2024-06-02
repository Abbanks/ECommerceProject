using ECommerceProject.Web.Models;
using ECommerceProject.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static ECommerceProject.Web.Utility.StaticDetail;

namespace ECommerceProject.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            try
            {
                if (requestDto == null)
                    throw new ArgumentNullException(nameof(requestDto));

                using var client = _httpClientFactory.CreateClient("ECommerceAPI");
                var message = new HttpRequestMessage
                {
                    RequestUri = new Uri(requestDto.Url),
                    Method = HttpMethod.Get // Default to GET if not specified
                };

                message.Headers.Add("Accept", "application/json");

                if (requestDto.ApiType != ApiType.GET)
                {
                    message.Method = requestDto.ApiType switch
                    {
                        ApiType.POST => HttpMethod.Post,
                        ApiType.PUT => HttpMethod.Put,
                        ApiType.DELETE => HttpMethod.Delete,
                        _ => HttpMethod.Get, // Default to GET if not recognized
                    };

                    if (requestDto.Data != null)
                    {
                        var json = JsonConvert.SerializeObject(requestDto.Data);
                        message.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }
                }

                using var response = await client.SendAsync(message);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new ResponseDto { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Unauthorized:
                        return new ResponseDto { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.Forbidden:
                        return new ResponseDto { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.InternalServerError:
                        return new ResponseDto { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<ResponseDto>(content);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
