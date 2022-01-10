using System.DirectoryServices;

namespace kairosApp.Domain.Services
{
    public interface IActiveDirectoryService
    {
        string GetCurrentDomainPath();
        void GetAUser(string userName);
        DirectorySearcher BuildUserSearcher(DirectoryEntry de);
        void GetAdditionalUserInfo();

        string Login(string userName, string password);
        bool ChangePassword(string userName, string password);
        bool ResetPassword(string userName, string password);

    }
}
