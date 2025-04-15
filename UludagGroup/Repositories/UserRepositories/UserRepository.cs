using Dapper;
using Microsoft.AspNetCore.Authentication;
using System.Reflection;
using System.Security.Claims;
using UludagGroup.Commons;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;
using UludagGroup.ViewModels.UserViewModels;

namespace UludagGroup.Repositories.UserRepositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly HashHelper _hashHelper;
        public UserRepository(Context context, IHttpContextAccessor httpContextAccessor, HashHelper hashHelper) : base(context, httpContextAccessor)
        {
            _hashHelper = hashHelper;
        }
        public async Task<ResponseViewModel<bool>> AddAsync(CreateUserViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = @"
                                INSERT INTO Users (
                                    Email, 
                                    Password,
                                    FullName
                                ) 
                                VALUES (
                                    @Email, 
                                    @Password,
                                    @FullName
                                )";

                var parameters = new DynamicParameters();
                parameters.Add("@Email", model.Email);
                parameters.Add("@Password", _hashHelper.HashPassword(model.Password));
                parameters.Add("@FullName", model.FullName);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);

                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Ekleme Başarısız";
                    response.Message = affectedRows > 0 ? "Yeni User başarıyla eklendi." : "User eklenemedi.";
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
        public async Task<ResponseViewModel<UserViewModel>> AdminAuthAsync(string email, string password, bool rememberMe)
        {
            var response = new ResponseViewModel<UserViewModel>();
            try
            {
                string query = "SELECT * FROM Users WHERE email = @email and IsAdminPage=1";
                var parameters = new DynamicParameters();
                parameters.Add("@email", email);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<UserViewModel>(query, parameters);
                    if (values == null)
                    {
                        throw new Exception("Kullanıcı ya da parola hatalıdır.");
                    }

                    if (!_hashHelper.VerifyPassword(password, values.Password))
                    {
                        throw new Exception("Kullanıcı ya da parola hatalıdır.");
                    }
                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, values.Id.ToString()),
                                new Claim(ClaimTypes.Email, values.Email),
                                new Claim(ClaimTypes.Name, values.FullName),
                                new Claim(ClaimTypes.Role, "Admin") // Rol eklersen
                            };

                    var identity = new ClaimsIdentity(claims, "AdminScheme");
                    var principal = new ClaimsPrincipal(identity);

                    if (_httpContextAccessor.HttpContext != null)
                    {
                        await _httpContextAccessor.HttpContext.SignInAsync("AdminScheme", principal);
                    }
                    else
                    {
                        throw new InvalidOperationException("HttpContext mevcut değil, oturum başlatılamadı.");
                    }
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "User başarıyla bulundu.";
                    response.Data = values;
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
        public async Task<ResponseViewModel<UserViewModel>> FinanceAuthAsync(string email, string password, bool rememberMe)
        {
            var response = new ResponseViewModel<UserViewModel>();
            try
            {
                string query = "SELECT * FROM Users WHERE email = @email and IsFinancePage=1";
                var parameters = new DynamicParameters();
                parameters.Add("@email", email);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<UserViewModel>(query, parameters);
                    if (values == null)
                    {
                        throw new Exception("Kullanıcı ya da parola hatalıdır.");
                    }
                    if (!_hashHelper.VerifyPassword(password, values.Password))
                    {
                        throw new Exception("Kullanıcı ya da parola hatalıdır.");
                    }
                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, values.Id.ToString()),
                                new Claim(ClaimTypes.Email, values.Email),
                                new Claim(ClaimTypes.Name, values.FullName),
                                new Claim(ClaimTypes.Role, "Finance") // Rol eklersen
                            };

                    var identity = new ClaimsIdentity(claims, "FinanceScheme");
                    var principal = new ClaimsPrincipal(identity);

                    if (_httpContextAccessor.HttpContext != null)
                    {
                        await _httpContextAccessor.HttpContext.SignInAsync("FinanceScheme", principal);
                    }
                    else
                    {
                        throw new InvalidOperationException("HttpContext mevcut değil, oturum başlatılamadı.");
                    }

                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "User başarıyla bulundu.";
                    response.Data = values;

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
        public async Task<ResponseViewModel<List<UserViewModel>>> GetAllActiveAsync()
        {
            var response = new ResponseViewModel<List<UserViewModel>>();
            try
            {
                string query = "SELECT * FROM Users WHERE IsActive = 1";  // Active değeri true olanları getir
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<UserViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif Userlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<UserViewModel>();
            }
            return response;
        }
        public async Task<ResponseViewModel<List<UserViewModel>>> GetAllAsync()
        {
            var response = new ResponseViewModel<List<UserViewModel>>();
            try
            {
                string query = "SELECT * FROM Users";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<UserViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Userlar başarıyla getirildi.";
                    response.Data = values.ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
                response.Data = new List<UserViewModel>();  // Boş bir liste döndürülüyor.
            }
            return response;
        }
        public async Task<ResponseViewModel<UserViewModel>> GetAsync(int id)
        {
            var response = new ResponseViewModel<UserViewModel>();
            try
            {
                string query = "SELECT * FROM Users WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<UserViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "User başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili User bulunamadı.";
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
        public async Task<ResponseViewModel<UserViewModel>> GetAsync(string email)
        {
            var response = new ResponseViewModel<UserViewModel>();
            try
            {
                string query = "SELECT * FROM Users WHERE email = @email";
                var parameters = new DynamicParameters();
                parameters.Add("@email", email);
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryFirstOrDefaultAsync<UserViewModel>(query, parameters);

                    if (values != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "User başarıyla bulundu.";
                        response.Data = values;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Silinmiş veya Bulunamayan Kayıt";
                        response.Message = "Veritabanında belirtilen ID ile ilişkili User bulunamadı.";
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
                string query = "DELETE FROM Users WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Silme Başarısız";
                    response.Message = affectedRows > 0 ? "User başarıyla silindi." : "User silinemedi.";
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
                    var querySetOne = "UPDATE Users SET IsActive = @IsActive WHERE Id = @Id";
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { IsActive = isActive ? 1 : 0, Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "User seçildi." : "Belirtilen User bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> SetAdminPageAsync(int id, bool isAdmin)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var querySetOne = "UPDATE Users SET IsAdminPage = @IsAdminPage WHERE Id = @Id";
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { IsAdminPage = isAdmin ? 1 : 0, Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "User seçildi." : "Belirtilen User bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> SetFinancePageAsync(int id, bool isFinance)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var querySetOne = "UPDATE Users SET IsFinancePage = @IsFinancePage WHERE Id = @Id";
                    var affectedRows = await connection.ExecuteAsync(querySetOne, new { IsFinancePage = isFinance ? 1 : 0, Id = id });
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "User seçildi." : "Belirtilen User bulunamadı.";
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
        public async Task<ResponseViewModel<bool>> UpdateAsync(UpdateUserViewModel model)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "UPDATE Users SET " +
                                 "Email = @Email, " +
                                 "FullName = @FullName " +
                                 "WHERE Id = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", model.Id);
                parameters.Add("@Email", model.Email);
                parameters.Add("@FullName", model.FullName);
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "User güncellendi." : "User bulunamadı veya güncellenemedi.";
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
        public async Task<ResponseViewModel<bool>> UpdatePasswordAsync(int id, string password)
        {
            var response = new ResponseViewModel<bool>();
            try
            {
                string query = "UPDATE Users SET " +
                                 "Password = @Password " +
                                 "WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@Password", _hashHelper.HashPassword(password));
                using (var connection = _context.CreateConnection())
                {
                    var affectedRows = await connection.ExecuteAsync(query, parameters);
                    response.Status = affectedRows > 0;
                    response.Title = affectedRows > 0 ? "Başarılı" : "Güncelleme Başarısız";
                    response.Message = affectedRows > 0 ? "Parola güncellendi." : "User bulunamadı veya Parola güncellenemedi.";
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
