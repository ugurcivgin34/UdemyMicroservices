using Newtonsoft.Json;

namespace FreeCourse.Shared.Dtos
{
    /// <summary>
    /// Genel bir yanıt modeli olan ResponseDto.
    /// </summary>
    /// <typeparam name="T">Yanıtın veri türü.</typeparam>
    public class Response<T>
    {
        /// <summary>
        /// Yanıttaki veri.
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// HTTP durum kodu.
        /// Bu bilgi JSON'a serileştirilirken dahil edilmez.
        /// </summary>
        [JsonIgnore]
        public int StatusCode { get; private set; }

        /// <summary>
        /// Yanıtın başarılı olup olmadığı bilgisini tutar.
        /// Bu bilgi JSON'a serileştirilirken dahil edilmez.
        /// </summary>
        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// Oluşabilecek hataların listesi.
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Başarılı bir yanıt oluşturmak için kullanılan static fabrika metodu.
        /// </summary>
        /// <param name="data">Döndürülecek veri.</param>
        /// <param name="statusCode">HTTP durum kodu.</param>
        /// <returns>Başarılı bir ResponseDto nesnesi.</returns>
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        /// <summary>
        /// Veri olmadan başarılı bir yanıt oluşturmak için kullanılan static fabrika metodu.
        /// </summary>
        /// <param name="statusCode">HTTP durum kodu.</param>
        /// <returns>Başarılı bir ResponseDto nesnesi.</returns>
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        /// <summary>
        /// Başarısız bir yanıt oluşturmak için kullanılan static fabrika metodu.
        /// </summary>
        /// <param name="errors">Hataların listesi.</param>
        /// <param name="statusCode">HTTP durum kodu.</param>
        /// <returns>Başarısız bir ResponseDto nesnesi.</returns>
        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        /// <summary>
        /// Tek bir hatayla başarısız bir yanıt oluşturmak için kullanılan static fabrika metodu.
        /// </summary>
        /// <param name="error">Hata mesajı.</param>
        /// <param name="statusCode">HTTP durum kodu.</param>
        /// <returns>Başarısız bir ResponseDto nesnesi.</returns>
        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessful = false };
        }
    }

}
