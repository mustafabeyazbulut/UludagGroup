using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.ReferenceViewModels;

namespace UludagGroup.Repositories.ReferenceRepositories
{
    public class ReferenceRepository : BaseRepository, IReferenceRepository
    {
        public ReferenceRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<ResponseViewModel<bool>> AddAsync(CreateReferenceViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                INSERT INTO BusinessReferences (
                                    CompanyName, 
                                    ImageUrl,
                                    Description
                                ) 
                                VALUES (
                                    @CompanyName, 
                                    @ImageUrl,
                                    @Description
                                )";
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyName", model.CompanyName);
                parameters.Add("@ImageUrl", model.ImageUrl);
                parameters.Add("@Description", model.Description);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni Reference başarıyla eklendi." : "Reference eklenemedi.";
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
        public async Task<ResponseViewModel<List<ReferenceViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<ReferenceViewModel>>();
            try
            {
                string query = "SELECT * FROM BusinessReferences WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<ReferenceViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif Referencelar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<ReferenceViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<ReferenceViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<ReferenceViewModel>>();
            try
            {
                string query = "SELECT * FROM BusinessReferences";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<ReferenceViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Referencelar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<ReferenceViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<ReferenceViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<ReferenceViewModel>();
            try
            {
                string query = "SELECT * FROM BusinessReferences WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<ReferenceViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Reference başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili Reference bulunamadı.";
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
                string query = "DELETE FROM BusinessReferences WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "Reference başarıyla silindi." : "Reference silinemedi.";
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
                    var querySetOne = "UPDATE BusinessReferences SET IsActive = @IsActive WHERE Id = @Id";
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { IsActive = isActive ? 1 : 0, Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Reference seçildi." : "Belirtilen Reference bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateReferenceViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                UPDATE BusinessReferences 
                                SET 
                                    CompanyName = @CompanyName, 
                                    ImageUrl = @ImageUrl, 
                                    Description = @Description 
                                WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@CompanyName", model.CompanyName);
                parameters.Add("@ImageUrl", model.ImageUrl);
                parameters.Add("@Description", model.Description);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Reference güncellendi." : "Reference bulunamadı veya güncellenemedi.";
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
