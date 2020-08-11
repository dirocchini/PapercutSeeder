
using Microsoft.Extensions.DependencyInjection;
using Seeder;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = Startup.ConfigureServices();
        serviceProvider.GetService<App>().ProcessJobs();
    }


//private static PaperCut.ServerCommandProxyService _serverProxy;

    //public static void Main()
    //{
    //    var serviceProvider = Startup.ConfigureServices();
    //    serviceProvider.GetService<App>();







    //    // This should be the value defined in the advanced config key auth.webservices.auth-token. Change as appropriate.
    //    const string authToken = "123123";
    //    const string server = "localhost";
    //    const int days = 90;
    //    const int maxDailyJobs = 40;
    //    DateTime startDateTime = DateTime.Now.AddDays(-1 * days);


    //    // Create an instance of the server command proxy class.
    //    _serverProxy = new PaperCut.ServerCommandProxyService(server, 9191, authToken);


    //    List<string> users = new List<string>();
    //    List<string> printers = new List<string>();

    //    users = _serverProxy.ListUserAccounts(0, int.MaxValue).ToList();
    //    printers = _serverProxy.ListPrinters(0, int.MaxValue).ToList();

    //    users = AddUsersIfEmpty(users);
    //    printers = AddPrintersIfEmpty(printers);

    //    bool log = true;
    //    while (log)
    //    {
    //        DateTime now = startDateTime.AddDays(1);

    //        if (now >= DateTime.Now)
    //            log = true;

    //        var jobsQtd = new Random().Next(maxDailyJobs / 2, maxDailyJobs);
    //        var jobIntervalMinutes = 1440 / jobsQtd;
    //        var startDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
    //        for (int i = 0; i < jobsQtd; i++)
    //        {
    //            var u = users[new Random().Next(users.Count)];
    //            var p = printers[new Random().Next(printers.Count)];

    //            var totalPages = new Random().Next(1, 32);
    //            var totalColorPages = new Random().Next(0, totalPages);

    //            startDate = startDate.AddMinutes(jobIntervalMinutes).AddSeconds(new Random().Next(5, 48));


    //            var logToInsert =
    //                $"user={u},"
    //                + $"server=FAKE SERVER,"
    //                + $"printer={(p.Contains(@"\") ? p.Split('\\')[1] : p)},"
    //                + $"time={string.Format("{0:yyyyMMddTHHmmss}", startDate)},"
    //                // + "cost=" 
    //                + $"total-pages=" + FormatString(totalPages.ToString()) + ","
    //                + $"total-color-pages=" + FormatString(totalColorPages.ToString()) + ","
    //                + $"copies=" + FormatString("1") + ","
    //                + $"document-name=" + FormatString("FAKE PRINT LOG") + ","
    //                //+ "duplex=" 
    //                //+ "grayscale=" 
    //                //+ "paper-size-name="
    //                //+ "paper-width-mm=" 
    //                //+ "paper-height-mm="
    //                //+ "document-size-kb=" 
    //                //+ "invoice=" 
    //                + $"comment=inserido pelo robo";// + ","
    //                                                //+ "client-machine=" 
    //                                                //+ "client-ip=" 
    //                                                //+ "shared-account=" 

    //            _serverProxy.ProcessJob(logToInsert);

    //            Console.WriteLine($"inserindo log para a data de {startDate} ");
    //        }

    //        startDateTime = startDate;
    //    }
    //}

    //private static List<string> AddUsersIfEmpty(List<string> users)
    //{
    //    if (users == null || users.Count == 0)
    //    {
    //        users.Add("user.A");
    //        users.Add("user.B");
    //        users.Add("user.C");
    //        users.Add("user.D");
    //        users.Add("user.E");
    //        users.Add("user.F");
    //        users.Add("user.G");
    //        users.Add("user.H");
    //        users.Add("user.I");
    //        users.Add("user.J");
    //        users.Add("user.K");
    //        users.Add("user.L");
    //    }

    //    return users;
    //}

    //private static List<string> AddPrintersIfEmpty(List<string> printers)
    //{
    //    if (printers == null || printers.Count == 0)
    //    {
    //        printers.Add("printer A");
    //        printers.Add("printer B");
    //        printers.Add("printer C");
    //        printers.Add("printer D");
    //        printers.Add("printer E");
    //        printers.Add("printer F");
    //        printers.Add("printer G");
    //    }


    //    return printers;
    //}

    //private static string FormatString(string value)
    //{
    //    if (string.IsNullOrEmpty(value))
    //        return string.Empty;

    //    string ret = string.Empty;
    //    ret = value.Replace(",", "").Replace("\"", "").Trim();

    //    return ret;
    //}

}
