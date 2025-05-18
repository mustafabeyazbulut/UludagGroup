using Dapper;
using UludagGroup.Models.Contexts;
using UludagGroup.Repositories;
using UludagGroup.ViewModels;

namespace UludagGroup.Areas.Finance.Repositories.GenericRepositories
{
    public class GenericRepository<TModel, TCreateModel, TUpdateModel, TViewModel> : BaseRepository,
     IGenericRepository<TModel, TCreateModel, TUpdateModel, TViewModel>
     where TModel : class
     where TCreateModel : class
     where TUpdateModel : class
     where TViewModel : class, new()
    {
        public GenericRepository(Context context, IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor) // BaseRepository'nin constructor'ını çağırıyoruz
        {
        }
        // Tüm verileri liste olarak al
        public async Task<ResponseViewModel<List<TViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<TViewModel>>();
            try
            {
                string query = "SELECT * FROM z" + typeof(TModel).Name.Replace("ViewModel",""); // Dinamik tablo adı
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.QueryAsync<TViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Veriler başarıyla getirildi.";
                    response.Data = data.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<TViewModel>();
            }

            return response;
        }
        // Aktif verileri al
        public async Task<ResponseViewModel<List<TViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<TViewModel>>();

            try
            {
                string query = @"
                            SELECT * FROM z" + typeof(TModel).Name.Replace("ViewModel", "") + @"
                            WHERE IsActive = 1";  // Aktif verileri almak için sorgu
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.QueryAsync<TViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif veriler başarıyla getirildi.";
                    response.Data = data.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<TViewModel>();
            }

            return response;
        }
        // Tek bir veriyi al
        public async Task<ResponseViewModel<TViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<TViewModel>();

            try
            {
                string query = "SELECT * FROM z" + typeof(TModel).Name.Replace("ViewModel", "") + " WHERE Id = @Id";
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.QuerySingleOrDefaultAsync<TViewModel>(query, new { Id = id });
                    if (data != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Veri başarıyla getirildi.";
                        response.Data = data;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Bulunamadı";
                        response.Message = "Veri bulunamadı.";
                        response.Data = null;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = null;
            }

            return response;
        }
        // Veriyi ekle
        public async Task<ResponseViewModel<int>> AddAsync(TCreateModel model)
        {
            var response = new ResponseViewModel<int>();

            try
            {
                var properties = typeof(TCreateModel).GetProperties()
                    .Where(p => p.CanRead && p.Name.ToLower() != "id") // ID'yi dışla
                    .ToList();

                var columns = string.Join(", ", properties.Select(p => p.Name));
                var values = string.Join(", ", properties.Select(p => "@" + p.Name));

                string tableName = $"z{typeof(TCreateModel).Name.Replace("Create", "").Replace("ViewModel", "")}";

                // Kimliği döndüren sorgu
                string query = $@"
            INSERT INTO {tableName} ({columns}) 
            VALUES ({values}); 
            SELECT CAST(SCOPE_IDENTITY() as int);";

                using (var connection = _context.CreateConnection())
                {
                    var insertedId = await connection.ExecuteScalarAsync<int>(query, model);
                    response.Status = insertedId > 0;
                    response.Title = "Başarılı";
                    response.Message = insertedId > 0 ? "Veri başarıyla eklendi." : "Veri eklenemedi.";
                    response.Data = insertedId;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = 0;
            }

            return response;
        }

        // Veriyi güncelle
        public async Task<ResponseViewModel<bool>> UpdateAsync(TUpdateModel model)
        {
            var response = new ResponseViewModel<bool>();

            try
            {
                // Modelin tüm özelliklerini al
                var properties = typeof(TUpdateModel).GetProperties()
                    .Where(p => p.CanRead && p.Name != "Id") // "Id" özelliğini hariç tutuyoruz
                    .ToList();

                // SET kısmındaki kolonları ve değerleri oluştur
                var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

                // SQL sorgusunu oluştur
                string query = $"UPDATE z{typeof(TUpdateModel).Name.Replace("Update", "").Replace("ViewModel", "")} SET {setClause} WHERE Id = @Id";

                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync(query, model);
                    response.Status = result > 0;
                    response.Title = "Başarılı";
                    response.Message = result > 0 ? "Veri başarıyla güncellendi." : "Veri güncellenemedi.";
                    response.Data = result > 0;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = false;
            }

            return response;
        }
        // Veriyi sil
        public async Task<ResponseViewModel<bool>> RemoveAsync(int id)
        {
            var response = new ResponseViewModel<bool>();

            try
            {
                string query = "DELETE FROM z" + typeof(TModel).Name.Replace("ViewModel", "") + " WHERE Id = @Id";
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync(query, new { Id = id });
                    response.Status = result > 0;
                    response.Title = "Başarılı";
                    response.Message = result > 0 ? "Veri başarıyla silindi." : "Veri silinemedi.";
                    response.Data = result > 0;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = false;
            }

            return response;
        }
        // Verinin aktiflik durumunu güncelle
        public async Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "UPDATE z" + typeof(TModel).Name.Replace("ViewModel", "") + " SET IsActive = @IsActive WHERE Id = @Id";
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync(query, new { Id = id, IsActive = isActive });
                    response.Status = result > 0;
                    response.Title = "Başarılı";
                    response.Message = result > 0 ? "Aktiflik durumu başarıyla güncellendi." : "Aktiflik durumu güncellenemedi.";
                    response.Data = result > 0;
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = false;
            }
            return response;
        }
    }
}
