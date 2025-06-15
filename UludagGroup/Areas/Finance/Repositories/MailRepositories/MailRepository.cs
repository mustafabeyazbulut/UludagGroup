using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.MailViewModels;
using UludagGroup.Models.Contexts;

namespace UludagGroup.Areas.Finance.Repositories.MailRepositories
{
    public class MailRepository : GenericRepository<MailViewModel, CreateMailViewModel, UpdateMailViewModel, MailViewModel>,
        IMailRepository
    {
        public MailRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }
}
