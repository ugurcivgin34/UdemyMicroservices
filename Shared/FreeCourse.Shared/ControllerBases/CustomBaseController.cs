using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Shared.ControllerBases
{
    /// <summary>
    /// Özelleştirilmiş temel controller sınıfı.
    /// Bu sınıf, ortak bir ActionResult dönüş metodu sağlamaktadır.
    /// </summary>
    public class CustomBaseController : ControllerBase
    {
        /// <summary>
        /// Verilen bir yanıtı temsil eden bir ActionResult örneği oluşturur.
        /// </summary>
        /// <param name="response">Döndürülecek yanıt.</param>
        /// <returns>İlgili yanıtın status koduyla birlikte ObjectResult örneği.</returns>
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }

}
