﻿using ECommerceProject.Web.Service.IService;
using ECommerceProject.Web.Utility;

namespace ECommerceProject.Web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetail.TokenCookie);
        }

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticDetail.TokenCookie, out token);
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetail.TokenCookie, token);

        }
    }
}
