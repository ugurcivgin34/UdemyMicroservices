using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    /// <summary>
    /// MongoDB'de kurslarla ilgili işlemleri gerçekleştiren servis.
    /// </summary>
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        /// <summary>
        /// CourseService sınıfının yeni bir örneğini oluşturur.
        /// </summary>
        /// <param name="mapper">Objeler arası dönüşüm için AutoMapper nesnesi.</param>
        /// <param name="databaseSettings">Veritabanı ayarları.</param>
        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm kursları döndürür.
        /// </summary>
        /// <returns>CourseDto listesi içeren bir yanıt.</returns>
        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();
            // Kurslar için kategori bilgilerini tamamlar.
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        /// <summary>
        /// Belirli bir ID'ye sahip kursu döndürür.
        /// </summary>
        /// <param name="id">Aranan kursun ID'si.</param>
        /// <returns>ID'ye karşılık gelen CourseDto içeren bir yanıt ya da hata mesajı.</returns>
        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return Response<CourseDto>.Fail("Course not found", 404);
            }

            course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        /// <summary>
        /// Belirli bir kullanıcıya ait tüm kursları döndürür.
        /// </summary>
        /// <param name="userId">Kursları aranan kullanıcının ID'si.</param>
        /// <returns>UserId'ye karşılık gelen CourseDto listesi içeren bir yanıt.</returns>
        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();
            // Kurslar için kategori bilgilerini tamamlar.
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        /// <summary>
        /// Yeni bir kurs oluşturur.
        /// </summary>
        /// <param name="courseCreateDto">Oluşturulacak kurs bilgileri.</param>
        /// <returns>Oluşturulan kursa dair CourseDto içeren bir yanıt.</returns>
        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedTime = DateTime.Now;

            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        /// <summary>
        /// Bir kursu günceller.
        /// </summary>
        /// <param name="courseUpdateDto">Güncellenecek kurs bilgileri.</param>
        /// <returns>Yanıt olarak işlem durumunu döndürür.</returns>
        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);

            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);

            if (result == null)
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }

            return Response<NoContent>.Success(204);
        }

        /// <summary>
        /// Bir kursu siler.
        /// </summary>
        /// <param name="id">Silinecek kursun ID'si.</param>
        /// <returns>Yanıt olarak işlem durumunu döndürür.</returns>
        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }
        }
    }

}
