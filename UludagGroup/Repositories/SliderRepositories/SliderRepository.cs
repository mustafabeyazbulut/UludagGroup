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

        public async Task<ResponseViewModel<bool>> AddSliderAsync(CreateSliderViewModels model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"INSERT INTO Slider (StrongText, NormalText, ContentText, ButtonText, ButtonLink, ImageUrl, Active)
                         VALUES (@StrongText, @NormalText, @ContentText, @ButtonText, @ButtonLink, @ImageUrl, @Active)";
                var parameters = new DynamicParameters();
                parameters.Add("@StrongText", model.StrongText);
                parameters.Add("@NormalText", model.NormalText);
                parameters.Add("@ContentText", model.ContentText);
                parameters.Add("@ButtonText", model.ButtonText);
                parameters.Add("@ButtonLink", model.ButtonLink);
                parameters.Add("@ImageUrl", model.ImageUrl);
                parameters.Add("@Active", model.Active);
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
        public async Task<ResponseViewModel<List<SliderViewModel>>> GetAllSliderAsync()
        {
            var response = new ResponseViewModel<List<SliderViewModel>>();
            try
            {
                string query = "SELECT * FROM Slider";
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
        public async Task<ResponseViewModel<List<SliderViewModel>>> GetAllActiveSliderAsync()
        {
            var response = new ResponseViewModel<List<SliderViewModel>>();
            try
            {
                string query = "SELECT * FROM Slider WHERE Active = 1";  // Active değeri true olanları getir
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

        public async Task<ResponseViewModel<SliderViewModel>> GetSliderAsync(int id)
        {
            var response = new ResponseViewModel<SliderViewModel>();
            try
            {
                string query = "SELECT * FROM Slider WHERE Id = @Id";
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
        public async Task<ResponseViewModel<bool>> RemoveSliderAsync(int id)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "DELETE FROM Slider WHERE Id = @Id";
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
        public async Task<ResponseViewModel<bool>> UpdateSliderAsync(UpdateSliderViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "Update Slider Set StrongText=@StrongText, NormalText=@NormalText, " +
                " ContentText=@ContentText, ButtonText=@ButtonText, ButtonLink=@ButtonLink," +
                " ImageUrl=@ImageUrl, Active=@Active " +
                " where Id=@Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@StrongText", model.StrongText);
                parameters.Add("@NormalText", model.NormalText);
                parameters.Add("@ContentText", model.ContentText);
                parameters.Add("@ButtonText", model.ButtonText);
                parameters.Add("@ButtonLink", model.ButtonLink);
                parameters.Add("@ImageUrl", model.ImageUrl);
                parameters.Add("@Active", model.Active);
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
