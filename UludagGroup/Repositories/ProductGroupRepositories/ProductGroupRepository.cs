using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.ProductGroupViewModels;

namespace UludagGroup.Repositories.ProductGroupRepositories
{
    public class ProductGroupRepository : BaseRepository, IProductGroupRepository
    {
        public ProductGroupRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<ResponseViewModel<bool>> AddAsync(CreateProductGroupViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                INSERT INTO ProductGroups (
                                    Name
                                ) 
                                VALUES (
                                    @Name
                                )";
                var parameters = new DynamicParameters();
                parameters.Add("@Name", model.Name);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni ProductGroup başarıyla eklendi." : "ProductGroup eklenemedi.";
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
        public async Task<ResponseViewModel<List<ProductGroupViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<ProductGroupViewModel>>();
            try
            {
                string query = "SELECT * FROM ProductGroups WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<ProductGroupViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif ProductGrouplar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<ProductGroupViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<ProductGroupViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<ProductGroupViewModel>>();
            try
            {
                string query = "SELECT * FROM ProductGroups";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<ProductGroupViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "ProductGrouplar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<ProductGroupViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<ProductGroupViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<ProductGroupViewModel>();
            try
            {
                string query = "SELECT * FROM ProductGroups WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<ProductGroupViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "ProductGroup başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili ProductGroup bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> RemoveAsync(int id)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "DELETE FROM ProductGroups WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "ProductGroup başarıyla silindi." : "ProductGroup silinemedi.";
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
                    var querySetOne = "UPDATE ProductGroups SET IsActive = @IsActive WHERE Id = @Id";
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { IsActive = isActive ? 1 : 0, Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "ProductGroup seçildi." : "Belirtilen ProductGroup bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateProductGroupViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                UPDATE ProductGroups 
                                SET 
                                    Name = @Name 
                                WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@Name", model.Name);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "ProductGroup güncellendi." : "ProductGroup bulunamadı veya güncellenemedi.";
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
