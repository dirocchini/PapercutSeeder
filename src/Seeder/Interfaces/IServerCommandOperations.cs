using System;
using System.Collections.Generic;
using System.Text;

namespace Seeder.Interfaces
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



    }
}
