using Microsoft.AspNetCore.Http;

namespace FreeCourse.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService // ISharedIdentityService arayüzünü implemente eder
    {
        private IHttpContextAccessor _httpContextAccessor; // HttpContextAccessor, HttpContext nesnesine erişmemizi sağlar
        
        public SharedIdentityService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value; // sub kullanıcının kimliğidir
    }
}
