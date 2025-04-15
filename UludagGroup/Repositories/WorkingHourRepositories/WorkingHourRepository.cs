using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.WorkingHourViewModels;

namespace UludagGroup.Repositories.WorkingHourRepositories
{
    public class WorkingHourRepository : BaseRepository, IWorkingHourRepository
    {
        public WorkingHourRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<ResponseViewModel<bool>> AddAsync(CreateWorkingHourViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                INSERT INTO WorkingHours (
                                    DayOfWeek, 
                                    TimeRange
                                ) 
                                VALUES (
                                    @DayOfWeek, 
                                    @TimeRange
                                )";
                var parameters = new DynamicParameters();
                parameters.Add("@DayOfWeek", model.DayOfWeek);
                parameters.Add("@TimeRange", model.TimeRange);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni WorkingHour başarıyla eklendi." : "WorkingHour eklenemedi.";
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
        public async Task<ResponseViewModel<List<WorkingHourViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<WorkingHourViewModel>>();
            try
            {
                string query = "SELECT * FROM WorkingHours WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<WorkingHourViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif WorkingHourlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<WorkingHourViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<WorkingHourViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<WorkingHourViewModel>>();
            try
            {
                string query = "SELECT * FROM WorkingHours";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<WorkingHourViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "WorkingHourlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<WorkingHourViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<WorkingHourViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<WorkingHourViewModel>();
            try
            {
                string query = "SELECT * FROM WorkingHours WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<WorkingHourViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "WorkingHour başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili WorkingHour bulunamadı.";
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
                string query = "DELETE FROM WorkingHours WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "WorkingHour başarıyla silindi." : "WorkingHour silinemedi.";
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
                    var querySetOne = "UPDATE WorkingHours SET IsActive = @IsActive WHERE Id = @Id";
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { IsActive = isActive ? 1 : 0, Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "WorkingHour seçildi." : "Belirtilen WorkingHour bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateWorkingHourViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                UPDATE WorkingHours 
                                SET 
                                    DayOfWeek = @DayOfWeek, 
                                    TimeRange = @TimeRange
                                WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@DayOfWeek", model.DayOfWeek);
                parameters.Add("@TimeRange", model.TimeRange);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "WorkingHour güncellendi." : "WorkingHour bulunamadı veya güncellenemedi.";
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
