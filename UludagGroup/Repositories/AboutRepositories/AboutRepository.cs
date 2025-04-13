using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.AboutViewModels;
using UludagGroup.ViewModels.AboutViewModels;

namespace UludagGroup.Repositories.AboutRepositories
{
    public class AboutRepository : BaseRepository, IAboutRepository
    {
        public AboutRepository(Context context) : base(context)
        {
        }
        public async Task<ResponseViewModel<bool>> AddAsync(CreateAboutViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                INSERT INTO Abouts (
                                    MainTitle, 
                                    Paragraph1Image,
                                    Paragraph1Title,
                                    Paragraph1Text,
                                    Paragraph2Image,
                                    Paragraph2Title,
                                    Paragraph2Text,
                                    LeftImage1,
                                    LeftImage2,
                                    LeftImage3,
                                    LeftImage4
                                ) 
                                VALUES (
                                    @MainTitle, 
                                    @Paragraph1Image,
                                    @Paragraph1Title,
                                    @Paragraph1Text,
                                    @Paragraph2Image,
                                    @Paragraph2Title,
                                    @Paragraph2Text,
                                    @LeftImage1,
                                    @LeftImage2,
                                    @LeftImage3,
                                    @LeftImage4
                                )";

                var parameters = new DynamicParameters();
                parameters.Add("@MainTitle", model.MainTitle);
                parameters.Add("@Paragraph1Image", model.Paragraph1Image);
                parameters.Add("@Paragraph1Title", model.Paragraph1Title);
                parameters.Add("@Paragraph1Text", model.Paragraph1Text);
                parameters.Add("@Paragraph2Image", model.Paragraph2Image);
                parameters.Add("@Paragraph2Title", model.Paragraph2Title);
                parameters.Add("@Paragraph2Text", model.Paragraph2Text);
                parameters.Add("@LeftImage1", model.LeftImage1);
                parameters.Add("@LeftImage2", model.LeftImage2);
                parameters.Add("@LeftImage3", model.LeftImage3);
                parameters.Add("@LeftImage4", model.LeftImage4);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni About başarıyla eklendi." : "About eklenemedi.";
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
        public async Task<ResponseViewModel<AboutViewModel>> GetActiveAsync()
        {
            var response = new ResponseViewModel<AboutViewModel>();
            try
            {
                string query = "SELECT TOP 1 * FROM Abouts WHERE IsActive = 1";  // Aktif olan ilk Aboutyu getir
                using (var connection = _context.CreateConnection())
                {
                    var value = await connection.QueryFirstOrDefaultAsync<AboutViewModel>(query);
                    if (value != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Aktif About başarıyla getirildi.";
                        response.Data = value;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "About Bulunamadı";
                        response.Message = "Aktif bir About bulunamadı.";
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
        public async Task<ResponseViewModel<List<AboutViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<AboutViewModel>>();
            try
            {
                string query = "SELECT * FROM Abouts WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<AboutViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif Aboutlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<AboutViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<AboutViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<AboutViewModel>>();
            try
            {
                string query = "SELECT * FROM Abouts";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<AboutViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aboutlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<AboutViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<AboutViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<AboutViewModel>();
            try
            {
                string query = "SELECT * FROM Abouts WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<AboutViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "About başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili About bulunamadı.";
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
                string query = "DELETE FROM Abouts WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "About başarıyla silindi." : "About silinemedi.";
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
                    var queryResetAll = "UPDATE Abouts SET IsActive = 0";
                    var querySetOne = "UPDATE Abouts SET IsActive = 1 WHERE Id = @Id";
                    await connection.ExecuteAsync(queryResetAll);
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "About seçildi." : "Belirtilen About bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateAboutViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "UPDATE Abouts SET " +
                                 "MainTitle = @MainTitle, " +
                                 "Paragraph1Image = @Paragraph1Image, " +
                                 "Paragraph1Title = @Paragraph1Title, " +
                                 "Paragraph1Text = @Paragraph1Text, " +
                                 "Paragraph2Image = @Paragraph2Image, " +
                                 "Paragraph2Title = @Paragraph2Title, " +
                                 "Paragraph2Text = @Paragraph2Text, " +
                                 "LeftImage1 = @LeftImage1, " +
                                 "LeftImage2 = @LeftImage2, " +
                                 "LeftImage3 = @LeftImage3, " +
                                 "LeftImage4 = @LeftImage4 " +
                                 "WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@MainTitle", model.MainTitle);
                parameters.Add("@Paragraph1Image", model.Paragraph1Image);
                parameters.Add("@Paragraph1Title", model.Paragraph1Title);
                parameters.Add("@Paragraph1Text", model.Paragraph1Text);
                parameters.Add("@Paragraph2Image", model.Paragraph2Image);
                parameters.Add("@Paragraph2Title", model.Paragraph2Title);
                parameters.Add("@Paragraph2Text", model.Paragraph2Text);
                parameters.Add("@LeftImage1", model.LeftImage1);
                parameters.Add("@LeftImage2", model.LeftImage2);
                parameters.Add("@LeftImage3", model.LeftImage3);
                parameters.Add("@LeftImage4", model.LeftImage4);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "About güncellendi." : "About bulunamadı veya güncellenemedi.";
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
