using Microsoft.Data.SqlClient;
using System.Data;

namespace UludagGroup.Models.Contexts
{
    public class Context
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public Context(IConfiguration configuration)
        {
            _configuration = configuration;
#if DEBUG
            _connectionString = _configuration.GetConnectionString("DebugConnection");
#else
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
#endif
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
