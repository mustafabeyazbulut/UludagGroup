using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.LogoViewModels;

namespace UludagGroup.Repositories.LogoRepositories
{
    public class LogoRepository : BaseRepository, ILogoRepository
    {
        public LogoRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<ResponseViewModel<bool>> AddAsync(CreateLogoViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                INSERT INTO Logos (
                                    Title, 
                                    ImageUrl
                                ) 
                                VALUES (
                                    @Title, 
                                    @ImageUrl
                                )";
                var parameters = new DynamicParameters();
                parameters.Add("@Title", model.Title);
                parameters.Add("@ImageUrl", model.ImageUrl);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni Logo başarıyla eklendi." : "Logo eklenemedi.";
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
        public async Task<ResponseViewModel<LogoViewModel>> GetActiveAsync()
        {
            var response = new ResponseViewModel<LogoViewModel>();
            try
            {
                string query = "SELECT TOP 1 * FROM Logos WHERE IsActive = 1";  // Aktif olan ilk logoyu getir
                using (var connection = _context.CreateConnection())
                {
                    var value = await connection.QueryFirstOrDefaultAsync<LogoViewModel>(query);
                    if (value != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Aktif logo başarıyla getirildi.";
                        response.Data = value;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Logo Bulunamadı";
                        response.Message = "Aktif bir logo bulunamadı.";
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
        public async Task<ResponseViewModel<List<LogoViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<LogoViewModel>>();
            try
            {
                string query = "SELECT * FROM Logos WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<LogoViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif logolar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<LogoViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<LogoViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<LogoViewModel>>();
            try
            {
                string query = "SELECT * FROM Logos";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<LogoViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Logolar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<LogoViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<LogoViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<LogoViewModel>();
            try
            {
                string query = "SELECT * FROM Logos WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<LogoViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Logo başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili logo bulunamadı.";
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
                string query = "DELETE FROM Logos WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "Logo başarıyla silindi." : "Logo silinemedi.";
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
                    var queryResetAll = "UPDATE Logos SET IsActive = 0";
                    var querySetOne = "UPDATE Logos SET IsActive = @IsActive WHERE Id = @Id";
                    if (isActive)
                    {
                        await connection.ExecuteAsync(queryResetAll);
                    }
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { IsActive = isActive ? 1 : 0, Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Logo seçildi." : "Belirtilen Logo bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateLogoViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                UPDATE Logos 
                                SET 
                                    Title = @Title, 
                                    ImageUrl = @ImageUrl 
                                WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@Title", model.Title);
                parameters.Add("@ImageUrl", model.ImageUrl);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Logo güncellendi." : "Logo bulunamadı veya güncellenemedi.";
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
