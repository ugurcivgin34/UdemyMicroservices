using FreeCourse.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{
    /// <summary>
    /// Identity sunucusunda kaynak sahibi şifre doğrulaması için özelleştirilmiş bir doğrulayıcı.
    /// </summary>
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        // Kullanıcı işlemleri için UserManager servisi
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Bir instance oluştururken gerekli bağımlılıkları enjekte eder.
        /// </summary>
        /// <param name="userManager">Kullanıcı işlemleri için UserManager servisi.</param>
        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Kullanıcının kimliğini doğrulamak için bu metod kullanılır.
        /// </summary>
        /// <param name="context">Doğrulama bağlamı, kullanıcı adı ve şifre içerir.</param>
        /// <returns>Task of type void.</returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            // Kullanıcı adı ile kullanıcıyı bulmaya çalış
            var existUser = await _userManager.FindByEmailAsync(context.UserName);

            // Kullanıcı yoksa hata mesajı döndür
            if (existUser == null)
            {
                var errors = new Dictionary<string, object>
            {
                { "errors", new List<string> { "Email veya şifreniz yanlış" } }
            };
                context.Result.CustomResponse = errors;

                return;
            }

            // Kullanıcının şifresini doğrula
            var passwordCheck = await _userManager.CheckPasswordAsync(existUser, context.Password);

            // Şifre yanlışsa hata mesajı döndür
            if (passwordCheck == false)
            {
                var errors = new Dictionary<string, object> // Hata mesajını bir sözlük içerisinde döndürüyoruz
            {
                { "errors", new List<string> { "Email veya şifreniz yanlış" } }
            };
                context.Result.CustomResponse = errors;

                return;
            }

            // Doğrulama başarılıysa, sonucu döndür
            context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }

}
