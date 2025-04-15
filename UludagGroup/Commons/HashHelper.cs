using System.Security.Cryptography;
using System.Text;

namespace UludagGroup.Commons
{
    public class HashHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string _saltString;
        public HashHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _saltString = _configuration["SaltString:salt"];
        }
        public string HashPassword(string password)
        {
            // Parolayı tuzlayarak hash'le
            var hashedPassword = HashPasswordWithSalt(password, _saltString);

            return hashedPassword;
        }
        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Kullanıcının girdiği parolayı tuzlayarak hash'le
            var hashedPasswordToCheck = HashPasswordWithSalt(password, _saltString);

            // Hash'lenmiş parolaları karşılaştır
            return hashedPasswordToCheck == hashedPassword;
        }
        private string HashPasswordWithSalt(string password, string salt)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash);
            }
        }
    }
}
