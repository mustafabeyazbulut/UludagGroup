using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.FaviconViewModels;

namespace UludagGroup.Repositories.FaviconRepositories
{
    public class FaviconRepository : BaseRepository, IFaviconRepository
    {
        public FaviconRepository(Context context) : base(context)
        {
        }
        public async Task<ResponseViewModel<bool>> AddAsync(CreateFaviconViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"INSERT INTO Favicons (Title, ImageUrl)
                         VALUES ( @Title, @ImageUrl)";
                var parameters = new DynamicParameters();
                parameters.Add("@Title", model.Title);
                parameters.Add("@ImageUrl", model.ImageUrl);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni Favicon başarıyla eklendi." : "Favicon eklenemedi.";
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
        public async Task<ResponseViewModel<FaviconViewModel>> GetActiveAsync()
        {
            var response = new ResponseViewModel<FaviconViewModel>();
            try
            {
                string query = "SELECT TOP 1 * FROM Favicons WHERE IsActive = 1";  // Aktif olan ilk Faviconyu getir
                using (var connection = _context.CreateConnection())
                {
                    var value = await connection.QueryFirstOrDefaultAsync<FaviconViewModel>(query);
                    if (value != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Aktif Favicon başarıyla getirildi.";
                        response.Data = value;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Favicon Bulunamadı";
                        response.Message = "Aktif bir Favicon bulunamadı.";
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
        public async Task<ResponseViewModel<List<FaviconViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<FaviconViewModel>>();
            try
            {
                string query = "SELECT * FROM Favicons WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<FaviconViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif Favcionlar, başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<FaviconViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<FaviconViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<FaviconViewModel>>();
            try
            {
                string query = "SELECT * FROM Favicons";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<FaviconViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Faviconlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<FaviconViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<FaviconViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<FaviconViewModel>();
            try
            {
                string query = "SELECT * FROM Favicons WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<FaviconViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Favicon başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili favicon bulunamadı.";
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
                string query = "DELETE FROM Favicons WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "Favicon başarıyla silindi." : "Favicon silinemedi.";
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
                    var queryResetAll = "UPDATE Favicons SET IsActive = 0";
                    var querySetOne = "UPDATE Favicons SET IsActive = 1 WHERE Id = @Id";
                    await connection.ExecuteAsync(queryResetAll);
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Favicon seçildi." : "Belirtilen Favicon bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateFaviconViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "Update Favicons Set Title=@Title, ImageUrl=@ImageUrl where Id=@Id ";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@Title", model.Title);
                parameters.Add("@ImageUrl", model.ImageUrl);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Favicon güncellendi." : "Favicon bulunamadı veya güncellenemedi.";
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
