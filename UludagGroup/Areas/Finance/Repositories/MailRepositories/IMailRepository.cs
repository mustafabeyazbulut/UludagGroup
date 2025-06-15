using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.MailViewModels;

namespace UludagGroup.Areas.Finance.Repositories.MailRepositories
{
    public interface IMailRepository : IGenericRepository<MailViewModel, CreateMailViewModel, UpdateMailViewModel, MailViewModel>
    {
    }
}
