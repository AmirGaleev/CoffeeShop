using Microsoft.AspNetCore.Http;

namespace CoffeeShop.Services
{
    public class VisitCounterService : IVisitCounterService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string VisitCookieName = "UserVisits";
        private const int CookieExpiryDays = 365;

        public VisitCounterService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetVisitCount()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext.Request.Cookies.TryGetValue(VisitCookieName, out string visitCountStr))
            {
                return int.TryParse(visitCountStr, out int count) ? count : 0;
            }
            return 0;
        }

        public void IncrementVisitCount()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var currentCount = GetVisitCount();
            var newCount = currentCount + 1;

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(CookieExpiryDays),
                HttpOnly = true,
                IsEssential = true,
                Secure = true
            };

            httpContext.Response.Cookies.Append(VisitCookieName, newCount.ToString(), cookieOptions);
        }
    }
}