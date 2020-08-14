using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IServerCommandOperations
    {
        List<string> GetAllUsers();
        List<string> GetAllPrinters();
        List<string> GetAllSharedAccounts();
        void SaveLog(string job);
        void AddNewUser(string login);
        void SetUserProperty(string login, string propertyName,string propertyValue);
        void SetUserProperties(string login, string[][] propertyNamesAndValues);
        void AddFakeUsersToPapercut(List<string> departments, List<string> offices, int maxUsersToCreate);
    }
}
