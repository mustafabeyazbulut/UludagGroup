using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.SliderViewModels;

namespace UludagGroup.Repositories.SliderRepositories
{
    public class SliderRepository : BaseRepository, ISliderRepository
    {
        public SliderRepository(Context context) : base(context)
        {
        }

        public async Task<ResponseViewModel<bool>> AddAsync(CreateSliderViewModels model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"INSERT INTO Sliders (StrongText, NormalText, ContentText, ButtonText, ButtonLink, ImageUrl, IsFirst)
                         VALUES (@StrongText, @NormalText, @ContentText, @ButtonText, @ButtonLink, @ImageUrl, @IsFirst)";
                var parameters = new DynamicParameters();
                parameters.Add("@StrongText", model.StrongText);
                parameters.Add("@NormalText", model.NormalText);
                parameters.Add("@ContentText", model.ContentText);
                parameters.Add("@ButtonText", model.ButtonText);
                parameters.Add("@ButtonLink", model.ButtonLink);
                parameters.Add("@ImageUrl", model.ImageUrl);
                parameters.Add("@IsFirst", model.IsFirst);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni slider başarıyla eklendi." : "Slider eklenemedi.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message =  ex.Message;
            }
            return response;
        }
      
        public async Task<ResponseViewModel<List<SliderViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<SliderViewModel>>();
            try
            {
                string query = "SELECT * FROM Sliders WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<SliderViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif sliderlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<SliderViewModel>();  
            }
            return response;
        }
        public async Task<ResponseViewModel<List<SliderViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<SliderViewModel>>();
            try
            {
                string query = "SELECT * FROM Sliders";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<SliderViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Sliderlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<SliderViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<SliderViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<SliderViewModel>();
            try
            {
                string query = "SELECT * FROM Sliders WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<SliderViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Slider başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili slider bulunamadı.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message =  ex.Message;
            }
            return response;
        }
        public async Task<ResponseViewModel<bool>> RemoveAsync(int id)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "DELETE FROM Sliders WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "Slider başarıyla silindi." : "Slider silinemedi.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message =  ex.Message;
            }
            return response;
        }
        public async Task<ResponseViewModel<bool>> SetFirstAsync(int id)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var queryResetAll = "UPDATE Sliders SET IsFirst = 0";
                    var querySetOne = "UPDATE Sliders SET IsFirst = 1 WHERE Id = @Id";
                    await connection.ExecuteAsync(queryResetAll);
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Slider seçildi." : "Belirtilen slider bulunamadı.";
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
                    // Belirtilen slider'ın Active durumunu güncelleme
                    var querySetActive = "UPDATE Sliders SET IsActive = @IsActive WHERE Id = @Id";

                    var affectedRows = await connection.ExecuteAsync(querySetActive, new { IsActive = isActive ? 1 : 0, Id = id });

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Slider durumu güncellendi." : "Belirtilen slider bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateSliderViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "Update Sliders Set StrongText=@StrongText, NormalText=@NormalText, " +
                " ContentText=@ContentText, ButtonText=@ButtonText, ButtonLink=@ButtonLink," +
                " ImageUrl=@ImageUrl, IsFirst=@IsFirst " +
                " where Id=@Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@StrongText", model.StrongText);
                parameters.Add("@NormalText", model.NormalText);
                parameters.Add("@ContentText", model.ContentText);
                parameters.Add("@ButtonText", model.ButtonText);
                parameters.Add("@ButtonLink", model.ButtonLink);
                parameters.Add("@ImageUrl", model.ImageUrl);
                parameters.Add("@IsFirst", model.IsFirst);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Slider güncellendi." : "Slider bulunamadı veya güncellenemedi.";
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
