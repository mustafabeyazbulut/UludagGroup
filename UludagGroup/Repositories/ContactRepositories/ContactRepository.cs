using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.ContactViewModels;

namespace UludagGroup.Repositories.ContactRepositories
{
    public class ContactRepository : BaseRepository, IContactRepository
    {
        public ContactRepository(Context context) : base(context)
        {
        }
        public async Task<ResponseViewModel<bool>> AddAsync(CreateContactViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                INSERT INTO Contacts (
                                    ContentBody, 
                                    PrimaryEmail,
                                    SecondaryEmail,
                                    PrimaryPhone,
                                    SecondaryPhone,
                                    PrimaryAddress,
                                    SecondaryAddress,
                                    MapUrl
                                ) 
                                VALUES (
                                    @ContentBody, 
                                    @PrimaryEmail,
                                    @SecondaryEmail,
                                    @PrimaryPhone,
                                    @SecondaryPhone,
                                    @PrimaryAddress,
                                    @SecondaryAddress,
                                    @MapUrl
                                )";

                var parameters = new DynamicParameters();
                parameters.Add("@ContentBody", model.ContentBody);
                parameters.Add("@PrimaryEmail", model.PrimaryEmail);
                parameters.Add("@SecondaryEmail", model.SecondaryEmail);
                parameters.Add("@PrimaryPhone", model.PrimaryPhone);
                parameters.Add("@SecondaryPhone", model.SecondaryPhone);
                parameters.Add("@PrimaryAddress", model.PrimaryAddress);
                parameters.Add("@SecondaryAddress", model.SecondaryAddress);
                parameters.Add("@MapUrl", model.MapUrl);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni Contact başarıyla eklendi." : "Contact eklenemedi.";
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
        public async Task<ResponseViewModel<ContactViewModel>> GetActiveAsync()
        {
            var response = new ResponseViewModel<ContactViewModel>();
            try
            {
                string query = "SELECT TOP 1 * FROM Contacts WHERE IsActive = 1";  // Aktif olan ilk Contactyu getir
                using (var connection = _context.CreateConnection())
                {
                    var value = await connection.QueryFirstOrDefaultAsync<ContactViewModel>(query);
                    if (value != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Aktif Contact başarıyla getirildi.";
                        response.Data = value;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Contact Bulunamadı";
                        response.Message = "Aktif bir Contact bulunamadı.";
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
        public async Task<ResponseViewModel<List<ContactViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<ContactViewModel>>();
            try
            {
                string query = "SELECT * FROM Contacts WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<ContactViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif Contactlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<ContactViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<ContactViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<ContactViewModel>>();
            try
            {
                string query = "SELECT * FROM Contacts";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<ContactViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Contactlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<ContactViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<ContactViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<ContactViewModel>();
            try
            {
                string query = "SELECT * FROM Contacts WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<ContactViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Contact başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili Contact bulunamadı.";
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
                string query = "DELETE FROM Contacts WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "Contact başarıyla silindi." : "Contact silinemedi.";
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
                    var queryResetAll = "UPDATE Contacts SET IsActive = 0";
                    var querySetOne = "UPDATE Contacts SET IsActive = 1 WHERE Id = @Id";
                    await connection.ExecuteAsync(queryResetAll);
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Contact seçildi." : "Belirtilen Contact bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateContactViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "UPDATE Contacts SET " +
                                 "ContentBody = @ContentBody, " +
                                 "PrimaryEmail = @PrimaryEmail, " +
                                 "SecondaryEmail = @SecondaryEmail, " +
                                 "PrimaryPhone = @PrimaryPhone, " +
                                 "SecondaryPhone = @SecondaryPhone, " +
                                 "PrimaryAddress = @PrimaryAddress, " +
                                 "SecondaryAddress = @SecondaryAddress, " +
                                 "MapUrl = @MapUrl " +
                                 "WHERE Id = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@ContentBody", model.ContentBody);
                parameters.Add("@PrimaryEmail", model.PrimaryEmail);
                parameters.Add("@SecondaryEmail", model.SecondaryEmail);
                parameters.Add("@PrimaryPhone", model.PrimaryPhone);
                parameters.Add("@SecondaryPhone", model.SecondaryPhone);
                parameters.Add("@PrimaryAddress", model.PrimaryAddress);
                parameters.Add("@SecondaryAddress", model.SecondaryAddress);
                parameters.Add("@MapUrl", model.MapUrl);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Contact güncellendi." : "Contact bulunamadı veya güncellenemedi.";
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
