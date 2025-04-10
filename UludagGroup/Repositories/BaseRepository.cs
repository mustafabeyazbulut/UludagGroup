using UludagGroup.Models.Contexts;

namespace UludagGroup.Repositories
{
    public class BaseRepository
    {
        protected readonly Context _context;
        public BaseRepository(Context context)
        {
            _context = context;
        }
    }
}
