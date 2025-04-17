using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.OurServiceViewModels;

namespace UludagGroup.Repositories.OurServiceRepositories
{
    public class OurServiceRepository : BaseRepository, IOurServiceRepository
    {
        public OurServiceRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<ResponseViewModel<bool>> AddAsync(CreateOurServiceViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                INSERT INTO OurServices (
                                    Title, 
                                    Paragraph1,
                                    Paragraph2,
                                    ImageUrl
                                ) 
                                VALUES (
                                    @Title, 
                                    @Paragraph1,
                                    @Paragraph2,
                                    @ImageUrl
                                )";
                var parameters = new DynamicParameters();
                parameters.Add("@Title", model.Title);
                parameters.Add("@Paragraph1", model.Paragraph1);
                parameters.Add("@Paragraph2", model.Paragraph2);
                parameters.Add("@ImageUrl", model.ImageUrl);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni OurService başarıyla eklendi." : "OurService eklenemedi.";
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
        public async Task<ResponseViewModel<List<OurServiceViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<OurServiceViewModel>>();
            try
            {
                string query = "SELECT * FROM OurServices WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<OurServiceViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif OurServicelar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<OurServiceViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<OurServiceViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<OurServiceViewModel>>();
            try
            {
                string query = "SELECT * FROM OurServices";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<OurServiceViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "OurServicelar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<OurServiceViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<OurServiceViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<OurServiceViewModel>();
            try
            {
                string query = "SELECT * FROM OurServices WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<OurServiceViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "OurService başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili OurService bulunamadı.";
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
                string query = "DELETE FROM OurServices WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "OurService başarıyla silindi." : "OurService silinemedi.";
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
                    var querySetOne = "UPDATE OurServices SET IsActive = @IsActive WHERE Id = @Id";
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { IsActive = isActive ? 1 : 0, Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "OurService seçildi." : "Belirtilen OurService bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateOurServiceViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                UPDATE OurServices 
                                SET 
                                    Title = @Title, 
                                    Paragraph1 = @Paragraph1,
                                    Paragraph2 = @Paragraph2, 
                                    ImageUrl = @ImageUrl  
                                WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@Title", model.Title);
                parameters.Add("@Paragraph1", model.Paragraph1);
                parameters.Add("@Paragraph2", model.Paragraph2);
                parameters.Add("@ImageUrl", model.ImageUrl);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "OurService güncellendi." : "OurService bulunamadı veya güncellenemedi.";
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
