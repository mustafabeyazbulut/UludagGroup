using UludagGroup.Models.Contexts;

namespace UludagGroup.Repositories
{
    public class BaseRepository
    {
        protected readonly Context _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public BaseRepository(Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
