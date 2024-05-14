using ECommerceProject.Web.Models;

namespace ECommerceProject.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);
    }
}
