using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.SocialMediaViewModels;

namespace UludagGroup.Repositories.SocialMediaRepositories
{
    public class SocialMediaRepository : BaseRepository, ISocialMediaRepository
    {
        public SocialMediaRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<ResponseViewModel<bool>> AddAsync(CreateSocialMediaViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                INSERT INTO SocialMedia (
                                    Twitter, 
                                    Facebook,
                                    Youtube,
                                    Linkedin
                                ) 
                                VALUES (
                                    @Twitter, 
                                    @Facebook,
                                    @Youtube,
                                    @Linkedin
                                )";

                var parameters = new DynamicParameters();
                parameters.Add("@Twitter", model.Twitter);
                parameters.Add("@Facebook", model.Facebook);
                parameters.Add("@Youtube", model.Youtube);
                parameters.Add("@Linkedin", model.Linkedin);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni SocialMedia başarıyla eklendi." : "SocialMedia eklenemedi.";
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
        public async Task<ResponseViewModel<SocialMediaViewModel>> GetActiveAsync()
        {
            var response = new ResponseViewModel<SocialMediaViewModel>();
            try
            {
                string query = "SELECT TOP 1 * FROM SocialMedia WHERE IsActive = 1"; 
                using (var connection = _context.CreateConnection())
                {
                    var value = await connection.QueryFirstOrDefaultAsync<SocialMediaViewModel>(query);
                    if (value != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Aktif SocialMedia başarıyla getirildi.";
                        response.Data = value;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "SocialMedia Bulunamadı";
                        response.Message = "Aktif bir SocialMedia bulunamadı.";
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
        public async Task<ResponseViewModel<List<SocialMediaViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<SocialMediaViewModel>>();
            try
            {
                string query = "SELECT * FROM SocialMedia WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<SocialMediaViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif SocialMedialar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<SocialMediaViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<SocialMediaViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<SocialMediaViewModel>>();
            try
            {
                string query = "SELECT * FROM SocialMedia";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<SocialMediaViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "SocialMedialar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<SocialMediaViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<SocialMediaViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<SocialMediaViewModel>();
            try
            {
                string query = "SELECT * FROM SocialMedia WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<SocialMediaViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "SocialMedia başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili SocialMedia bulunamadı.";
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
                string query = "DELETE FROM SocialMedias WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "SocialMedia başarıyla silindi." : "SocialMedia silinemedi.";
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
                    var queryResetAll = "UPDATE SocialMedia SET IsActive = 0";
                    var querySetOne = "UPDATE SocialMedia SET IsActive = @IsActive WHERE Id = @Id";
                    if (isActive)
                    {
                        await connection.ExecuteAsync(queryResetAll);
                    }
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { Id = id, IsActive = isActive });

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0
                        ? $"SocialMedia {(isActive ? "aktif" : "pasif")} hale getirildi."
                        : "Belirtilen SocialMedia bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateSocialMediaViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "UPDATE SocialMedia SET " +
                                 "Twitter = @Twitter, " +
                                 "Facebook = @Facebook, " +
                                 "Youtube = @Youtube, " +
                                 "Linkedin = @Linkedin " +
                                 "WHERE Id = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@Twitter", model.Twitter);
                parameters.Add("@Facebook", model.Facebook);
                parameters.Add("@Youtube", model.Youtube);
                parameters.Add("@Linkedin", model.Linkedin);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "SocialMedia güncellendi." : "SocialMedia bulunamadı veya güncellenemedi.";
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
