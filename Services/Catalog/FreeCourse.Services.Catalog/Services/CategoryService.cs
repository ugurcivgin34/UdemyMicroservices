using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services
{
    /// <summary>
    /// MongoDB'de kategorilerle ilgili işlemleri gerçekleştiren servis.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        /// <summary>
        /// CategoryService sınıfının yeni bir örneğini oluşturur.
        /// </summary>
        /// <param name="mapper">Objeler arası dönüşüm için AutoMapper nesnesi.</param>
        /// <param name="databaseSettings">Veritabanı ayarları.</param>
        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        /// <summary>
        /// Tüm kategorileri döndürür.
        /// </summary>
        /// <returns>CategoryDto listesi içeren bir yanıt.</returns>
        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync(); // category => true tüm kategorileri seçer.
            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }

        /// <summary>
        /// Yeni bir kategori oluşturur.
        /// </summary>
        /// <param name="category">Oluşturulacak kategori nesnesi.</param>
        /// <returns>Oluşturulan kategoriye dair CategoryDto içeren bir yanıt.</returns>
        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryCollection.InsertOneAsync(category);
            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }

        /// <summary>
        /// Belirli bir ID'ye sahip kategoriyi döndürür.
        /// </summary>
        /// <param name="id">Aranan kategorinin ID'si.</param>
        /// <returns>ID'ye karşılık gelen CategoryDto içeren bir yanıt ya da hata mesajı.</returns>
        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                return Response<CategoryDto>.Fail("Category not found", 404);
            }

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
    }

}
