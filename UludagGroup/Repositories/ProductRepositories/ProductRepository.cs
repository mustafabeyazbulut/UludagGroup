using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.ProductViewModels;

namespace UludagGroup.Repositories.ProductRepositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<ResponseViewModel<bool>> AddAsync(CreateProductViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                INSERT INTO Products (
                                    Name, 
                                    ShortDescription,
                                    LongDescription,
                                    Price,
                                    ImageUrl,
                                    Rating,
                                    PGroup
                                ) 
                                VALUES (
                                    @Name, 
                                    @ShortDescription,
                                    @LongDescription,
                                    @Price,
                                    @ImageUrl,
                                    @Rating,
                                    @PGroup     
                                )";
                var parameters = new DynamicParameters();
                parameters.Add("@Name", model.Name);
                parameters.Add("@ShortDescription", model.ShortDescription);
                parameters.Add("@LongDescription", model.LongDescription);
                parameters.Add("@Price", model.Price);
                parameters.Add("@ImageUrl", model.ImageUrl);
                parameters.Add("@Rating", model.Rating);
                parameters.Add("@PGroup", model.PGroup);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni Product başarıyla eklendi." : "Product eklenemedi.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ResponseViewModel<List<ProductViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<ProductViewModel>>();
            try
            {
                string query = @"
                                SELECT p.*, pg.Name as PGroupText  
                                FROM Products p
                                JOIN ProductGroups pg ON p.PGroup = pg.Id
                                WHERE p.IsActive = 1";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<ProductViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif Productlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<ProductViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<ProductViewModel>>> GetAllActiveFeaturedAsync()
        {
            var response = new ResponseViewModel<List<ProductViewModel>>();
            try
            {
                string query = @"SELECT p.*, pg.Name as PGroupText  
                                FROM Products p
                                JOIN ProductGroups pg ON p.PGroup = pg.Id
                                WHERE p.IsActive = 1 AND p.IsFeatured = 1";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<ProductViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif Productlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<ProductViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<ProductViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<ProductViewModel>>();
            try
            {
                string query = @"SELECT p.*, pg.Name as PGroupText  
                                FROM Products p
                                JOIN ProductGroups pg ON p.PGroup = pg.Id";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<ProductViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Productlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<ProductViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<ProductViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<ProductViewModel>();
            try
            {
                string query = @"SELECT p.*, pg.Name as PGroupText  
                                FROM Products p
                                JOIN ProductGroups pg ON p.PGroup = pg.Id
                                WHERE p.Id = @Id
                                ";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<ProductViewModel>(query, parameters);
                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Product başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili Product bulunamadı.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseViewModel<List<CategoryWithProductViewModel>>> GetProductsGroupedByCategoryAsync()
        {
            var response = new ResponseViewModel<List<CategoryWithProductViewModel>>();
            try
            {
                string query = @"
                                SELECT DISTINCT pg.Id AS PGroup, pg.Name as PGroupText
                                FROM Products p
                                JOIN ProductGroups pg ON p.PGroup = pg.Id
                                WHERE p.IsActive = 1
                                ORDER BY pg.Name";  // Ürün gruplarına göre sıralama

                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<CategoryWithProductViewModel>(query);


                    // values boşsa, yeni bir liste oluşturuyoruz. Eğer boş değilse gelen veriyi kullanıyoruz.
                    var categoryList = values?.ToList() ?? new List<CategoryWithProductViewModel>();

                    // Her durumda "Tüm Kategoriler" ekliyoruz.
                    var allCategories = new CategoryWithProductViewModel
                    {
                        PGroup = 0,
                        PGroupText = "Tüm Kategoriler",
                    };

                    // "Tüm Kategoriler" kategorisini listenin başına ekliyoruz
                    categoryList.Insert(0, allCategories);

                    response.Data = categoryList;
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Ürünler başarıyla kategorilere göre gruplanarak getirildi.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ResponseViewModel<bool>> RemoveAsync(int id)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "DELETE FROM Products WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "Product başarıyla silindi." : "Product silinemedi.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ResponseViewModel<List<ProductViewModel>>> SearchProductsAsync(SearchProductViewModel model)
        {
            var response = new ResponseViewModel<List<ProductViewModel>>();

            try
            {
                string query = @"
                        SELECT p.*, pg.Name as PGroupText  
                        FROM Products p
                        JOIN ProductGroups pg ON p.PGroup = pg.Id
                        WHERE p.IsActive = 1";

                // Eğer CategoryId sıfır değilse, sorguyu kategorize ediyoruz
                if (model.CategoryId != 0)
                {
                    query += " AND p.PGroup = @CategoryId";
                }

                // Eğer ProductName verilmişse, adı içeriyorsa sorguya ekliyoruz
                if (!string.IsNullOrEmpty(model.ProductName))
                {
                    query += " AND p.Name LIKE @ProductName";
                }

                // Veritabanı bağlantısını oluşturuyoruz
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<ProductViewModel>(query, new
                    {
                        CategoryId = model.CategoryId,
                        ProductName = "%" + model.ProductName + "%"
                    });

                    // Gelen veriyi listeliyoruz. Eğer sonuç yoksa, boş bir liste dönecek.
                    var productList = values?.ToList() ?? new List<ProductViewModel>();

                    response.Data = productList;
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Ürünler başarıyla filtrelenerek getirildi.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
            }

            return response;
        }
        public async Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var query = "UPDATE Products SET IsActive = @IsActive WHERE Id = @Id";
                    var affectedRows = await connection.ExecuteAsync(query, new { Id = id, IsActive = isActive });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0
                        ? (isActive ? "Ürün aktif hale getirildi." : "Ürün pasif hale getirildi.")
                        : "Belirtilen ürün bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ResponseViewModel<bool>> SetFeaturedStatusAsync(int id, bool isFeatured)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var query = "UPDATE Products SET IsFeatured = @IsFeatured WHERE Id = @Id";
                    var affectedRows = await connection.ExecuteAsync(query, new { Id = id, IsFeatured = isFeatured });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0
                        ? (isFeatured ? "Ürün öne çıkarıldı." : "Ürün öne çıkanlardan kaldırıldı.")
                        : "Belirtilen ürün bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateProductViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                UPDATE Products 
                                SET 
                                    Name = @Name, 
                                    ShortDescription = @ShortDescription, 
                                    LongDescription = @LongDescription, 
                                    Price = @Price, 
                                    ImageUrl = @ImageUrl, 
                                    Rating = @Rating,
                                    PGroup = @PGroup
                                WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@Name", model.Name);
                parameters.Add("@ShortDescription", model.ShortDescription);
                parameters.Add("@LongDescription", model.LongDescription);
                parameters.Add("@ImageUrl", model.ImageUrl);
                parameters.Add("@Price", model.Price);
                parameters.Add("@Rating", model.Rating);
                parameters.Add("@PGroup", model.PGroup);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Product güncellendi." : "Product bulunamadı veya güncellenemedi.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
