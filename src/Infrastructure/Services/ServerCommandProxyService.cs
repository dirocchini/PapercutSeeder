using System;
using System.Collections.Generic;
using System.Linq;
using Application.Interfaces;
using CookComputing.XmlRpc;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Options;
using Shared.Structs;

namespace Infrastructure.Services
{
    /// <summary>
    ///  A proxy designed to wrap XML-RCP calls the Application Server's XML-RPC API commands. This class requires the .NET
    ///  XML-RPC Library available from https://www.nuget.org/packages/Kveer.XmlRPC/ (.NET Core) or
    ///  https://www.nuget.org/packages/xmlrpcnet/ (.NET Foundation)
    /// </summary>
    public class ServerCommandProxyService : IServerCommandOperations
    {
        private readonly ILogger<ServerCommandProxyService> _logger;
        private readonly PapercutOptions _papercutOptions;
        private readonly IServerCommandProxy _proxy;
        private readonly string _authToken;

        /// <summary>
        ///  The constructor.
        /// </summary>
        ///
        /// <param name="server">
        ///  The name or IP address of the server hosting the Application Server. The server should be configured
        ///  to allow XML-RPC connections from the host running this proxy class. Localhost is generally accepted
        ///  by default.
        /// </param>
        /// <param name="port">
        ///  The port the Application Server is listening on. This is port 9191 on a default install.
        /// </param>
        /// <param name="authToken">
        ///  The authentication token as a string. All RPC calls must pass through an authentication token.
        ///  This should be the value defined in the advanced config key auth.webservices.auth-token.
        /// </param>
        public ServerCommandProxyService(IOptions<PapercutOptions> papercutOptions, ILogger<ServerCommandProxyService> logger)
        {
            _logger = logger;
            _papercutOptions = papercutOptions.Value;
            // this is the XML-RPC-v2 form:
            //_proxy = XmlRpcProxyGen.Create<IServerCommandProxy>();
            // XML-RPC-v1 uses the non-generic form... hopefully it will be around for a while
            _proxy = (IServerCommandProxy)XmlRpcProxyGen.Create(typeof(IServerCommandProxy));

            _proxy.Url = "http://" + _papercutOptions.Server + ":" + _papercutOptions.Port + "/rpc/api/xmlrpc";
            _authToken = _papercutOptions.Token;
        }
        #region [  defaults  ]
        /// <summary>
        ///  Test to see if a user associated with "username" exists in the system.
        /// </summary>
        ///
        /// <param name="username">
        ///  The username to test.
        /// </param>
        /// <returns>
        ///  Returns true if the user exists in the system, else returns false.
        /// </returns>
        private bool IsUserExists(string username)
        {
            return _proxy.IsUserExists(_authToken, username);
        }

        /// This is the legacy method name
        private bool UserExists(string username)
        {
            return _proxy.IsUserExists(_authToken, username);
        }

        /// <summary>
        ///  Gets a user's current account balance.
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user.
        /// </param>
        /// <param name="accountName">
        ///  Optional name of the user's personal account. If blank, the total balance is returned.
        /// </param>
        /// <returns>
        ///  The value of the user's account.
        /// </returns>
        private double GetUserAccountBalance(string username, string accountName)
        {
            return _proxy.GetUserAccountBalance(_authToken, username, accountName);
        }

        /// <summary>
        ///  Get the user's overdraft mode
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user.
        /// </param>
        /// <returns>
        ///  the user's overdraft mode ('individual' or 'default
        /// </returns>
        private string GetUserOverdraftMode(string username)
        {
            return _proxy.GetUserOverdraftMode(_authToken, username);
        }

        /// <summary>
        ///  Set the user's overdraft mode
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user.
        /// </param>
        /// <param name="mode">
        ///  mode - the overdraft mode to use ('individual' or 'default')
        /// </param>
        private void SetUserOverdraftMode(string username, string mode)
        {
            _proxy.SetUserOverdraftMode(_authToken, username, mode);
        }

        /// <summary>Gets a user property.</summary>
        ///
        /// <param name="username">The name of the user.</param>
        /// <param name="propertyName">
        /// The name of the property to get.  The following list of property names can also be set using
        /// <see cref="SetUserProperty(string,string,string)" />:
        ///<list type="bullet">
        /// <item><description><c>balance</c>: the user's balance, unformatted, e.g. "1234.56".</description></item>
        /// <item><description><c>primary-card-number</c></description></item>
        /// <item><description><c>secondary-card-number</c></description></item>
        /// <item><description><c>card-pin</c></description></item>
        /// <item><description><c>department</c></description></item>
        /// <item><description>
        ///  <c>disabled-net</c>: <c>true</c> if the user's 'net access is disabled, otherwise <c>false</c>
        /// </description></item>
        /// <item><description>
        ///  <c>disabled-print</c>: <c>true</c> if the user's printing is disabled, otherwise <c>false</c>
        /// </description></item>
        /// <item><description><c>email</c></description></item>
        /// <item><description><c>full-name</c></description></item>
        /// <item><description>
        ///  <c>internal</c>: <c>true</c> if this is an internal user, otherwise <c>false</c>
        /// </description></item>
        /// <item><description><c>notes</c></description></item>
        /// <item><description><c>office</c></description></item>
        /// <item><description>
        ///  <c>print-stats.job-count</c>: total number of print jobs from this user, unformatted, e.g. "1234"
        /// </description></item>
        /// <item><description>
        ///  <c>print-stats.page-count</c>: total number of pages printed by this user, unformatted, e.g. "1234"
        /// </description></item>
        /// <item><description>
        ///  <c>net-stats.data-mb</c>: total 'net MB used by this user, unformatted, e.g. "1234.56"
        /// </description></item>
        /// <item><description>
        ///  <c>net-stats.time-hours</c>: total 'net hours used by this user, unformatted, e.g. "1234.56"
        /// </description></item>
        /// <item><description>
        ///  <c>restricted</c>: <c>true</c> if this user's printing is restricted, <c>false</c> if they are unrestricted.
        /// </description></item>
        ///</list>
        /// The following options are "read only", i.e. cannot be set using <see cref="SetUserProperty(string,string,string)" />:
        ///<list type="bullet">
        /// <item><description>
        ///  <c>account-selection.mode</c>: the user's account selection mode.  One of the following:
        ///  <list type="bullet">
        ///   <item><description><c>AUTO_CHARGE_TO_PERSONAL_ACCOUNT</c></description></item>
        ///   <item><description><c>CHARGE_TO_PERSONAL_ACCOUNT_WITH_CONFIRMATION</c></description></item>
        ///   <item><description><c>AUTO_CHARGE_TO_SHARED</c></description></item>
        ///   <item><description><c>SHOW_ACCOUNT_SELECTION_POPUP</c></description></item>
        ///   <item><description><c>SHOW_ADVANCED_ACCOUNT_SELECTION_POPUP</c></description></item>
        ///   <item><description><c>SHOW_MANAGER_MODE_POPUP</c></description></item>
        ///  </list>
        /// </description></item>
        /// <item><description>
        ///  <c>account-selection.can-charge-personal</c>: <c>true</c> if the user's account selection settings allow them
        ///  to charge jobs to their personal account, otherwise <c>false</c>.
        /// </description></item>
        /// <item><description>
        ///  <c>account-selection.can-charge-shared-from-list</c>: <c>true</c> if the user's account selection settings
        ///  allow them to select a shared account to charge from a list of shared accounts, otherwise <c>false</c>.
        /// </description></item>
        /// <item><description>
        ///  <c>account-selection.can-charge-shared-by-pin</c>: <c>true</c> if the user's account selection settings allow
        ///  them to charge a shared account by PIN or code, otherwise <c>false</c>.
        /// </description></item>
        ///</list>
        /// </param>
        /// <returns>The value of the requested property.</returns>
        ///
        /// <see cref="SetUserProperty(string,string,string)" />
        private string GetUserProperty(string username, string propertyName)
        {
            return _proxy.GetUserProperty(_authToken, username, propertyName);
        }

        /// <summary>
        ///  Get multiple user properties at once (to save multiple calls).
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user.
        /// </param>
        /// <param name="propertyNames">
        ///  The names of the properties to get.  See <see cref="GetUserProperty(string,string)" /> for valid property names.
        /// </param>
        /// <returns>
        ///  The property values (in the same order as given in <paramref param="propertyNames" />).
        /// </returns>
        ///
        /// <see cref="GetUserProperty(string,string)" />
        /// <see cref="SetUserProperties(string,string[][])" />
        private string[] GetUserProperties(string username, string[] propertyNames)
        {
            return _proxy.GetUserProperties(_authToken, username, propertyNames);
        }

        /// <summary>
        ///  Sets a user property.
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user.
        /// </param>
        /// <param name="propertyName">
        ///  The name of the property to set.  Valid options include: balance, card-number, card-pin, department,
        ///  disabled-net, disabled-print, email, full-name, notes, office, password, print-stats.job-count,
        ///  print-stats.page-count, net-stats.data-mb, net-stats.time-hours, restricted.
        /// </param>
        /// <param name="propertyValue">
        ///  The value of the property to set.
        /// </param>
        ///
        /// <see cref="GetUserProperty(string,string)" />
        private void SetUserProperty(string username, string propertyName, string propertyValue)
        {
            _proxy.SetUserProperty(_authToken, username, propertyName, propertyValue);
        }

        /// <summary>
        ///  Set multiple user properties at once (to save multiple calls).
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user.
        /// </param>
        /// <param name="propertyNamesAndValues">
        ///  The list of property names and values to set. E.g. [["balance", "1.20"], ["office", "East Wing"]].  See
        ///  <see cref="SetUserProperty(string,string,string)" /> for valid property names.
        /// </param>
        ///
        /// <see cref="GetUserProperties(string,string[])" />
        /// <see cref="SetUserProperty(string,string,string)" />
        private void SetUserProperties(string username, string[][] propertyNamesAndValues)
        {
            _proxy.SetUserProperties(_authToken, username, propertyNamesAndValues);
        }

        /// <summary>
        ///  Adjust a user's account balance by an adjustment amount. An adjustment bay be positive (add to the user's
        ///  account) or negative (subtract from the account).
        /// </summary>
        ///
        /// <param name="username">
        ///  The username associated with the user who's account is to be adjusted.
        /// </param>
        /// <param name="adjustment">
        ///  The adjustment amount. Positive to add credit and negative to subtract.
        /// </param>
        /// <param name="comment">
        ///  A user defined comment to associated with the transaction. This may be a null string.
        /// </param>
        /// <param name="accountName">
        ///  Optional name of the user's personal account. If blank, the built-in default account is used.
        /// </param>
        private void AdjustUserAccountBalance(string username, double adjustment, string comment, string accountName)
        {
            _proxy.AdjustUserAccountBalance(_authToken, username, adjustment, comment, accountName);
        }

        /// <summary>
        /// Adjust a user's account balance by an adjustment amount (if there is credit available).   This can be used
        /// to perform atomic account adjustments, without needed to check the user's balance first. An adjustment may
        /// be positive (add to the user's account) or negative (subtract from the account).
        /// </summary>
        ///
        /// <param name="username">
        ///  The username associated with the user who's account is to be adjusted.
        /// </param>
        /// <param name="adjustment">
        ///  The adjustment amount. Positive to add credit and negative to subtract.
        /// </param>
        /// <param name="comment">
        ///  A user defined comment to associated with the transaction. This may be a null string.
        /// </param>
        /// <param name="accountName">
        ///  Optional name of the user's personal account. If blank, the built-in default account is used.
        /// </param>
        /// <returns>
        ///  True if the transaction was performed, and false if the user did not have enough balance available.
        /// </returns>
        private bool AdjustUserAccountBalanceIfAvailable(string username, double adjustment, string comment, string accountName)
        {
            return _proxy.AdjustUserAccountBalanceIfAvailable(_authToken, username, adjustment, comment, accountName);
        }

        /// <summary>
        /// Adjust a user's account balance by an adjustment amount (if there is credit available to leave the specified
        /// amount still available in the account).   This can be used to perform atomic account adjustments, without
        /// need to check the user's balance first. An adjustment may be positive (add to the user's account)
        /// or negative (subtract from the account).
        /// </summary>
        ///
        /// <param name="username">
        ///  The username associated with the user who's account is to be adjusted.
        /// </param>
        /// <param name="adjustment">
        ///  The adjustment amount. Positive to add credit and negative to subtract.
        /// </param>
        /// <param name="leaveRemaining">
        ///  The amount to leave in the account when deductions are done.
        /// </param>
        /// <param name="comment">
        ///  A user defined comment to associated with the transaction. This may be a null string.
        /// </param>
        /// <param name="accountName">
        ///  Optional name of the user's personal account. If blank, the built-in default account is used.
        /// </param>
        /// <returns>
        ///  True if the transaction was performed, and false if the user did not have enough balance available.
        /// </returns>
        private bool AdjustUserAccountBalanceIfAvailableLeaveRemaining(string username, double adjustment, double leaveRemaining, string comment, string accountName)
        {
            return _proxy.AdjustUserAccountBalanceIfAvailableLeaveRemaining(_authToken, username, adjustment, leaveRemaining, comment, accountName);
        }

        /// <summary>
        /// Adjust a user's account balance.  User lookup is by card number.
        /// </summary>
        /// <param name="cardNumber">
        /// The card number associated with the user who's account is to be adjusted.
        /// </param>
        /// <param name="adjustment">
        /// The adjustment amount.  Positive to add credit and negative to subtract.
        /// </param>
        /// <param name="comment">
        /// A user defined comment to be associated with the transaction.  This may be a null string.
        /// </param>
        /// <returns>
        /// True if successful, false if not (e.g. no users found for the supplied card number).
        /// </returns>
        private bool AdjustUserAccountBalanceByCardNumber(string cardNumber, double adjustment, string comment)
        {
            return _proxy.AdjustUserAccountBalanceByCardNumber(_authToken, cardNumber, adjustment, comment);
        }

        /// <summary>
        /// Adjust a user's account balance.  User lookup is by card number.
        /// </summary>
        /// <param name="cardNumber">
        /// The card number associated with the user who's account is to be adjusted.
        /// </param>
        /// <param name="adjustment">
        /// The adjustment amount.  Positive to add credit and negative to subtract.
        /// </param>
        /// <param name="comment">
        /// A user defined comment to be associated with the transaction.  This may be a null string.
        /// </param>
        /// <param name="accountName">
        /// Optional name of the user's personal account.  If blank, the built-in default account is used.  If multiple
        /// personal accounts is enabled the account name must be provided.
        /// </param>
        /// <returns>
        /// True if successful, FALSE if not (eg. no users found for the supplied card number)
        /// </returns>
        private bool AdjustUserAccountBalanceByCardNumber(string cardNumber, double adjustment, string comment,
            string accountName)
        {
            return _proxy.AdjustUserAccountBalanceByCardNumber(_authToken, cardNumber, adjustment, comment, accountName);
        }

        /// <summary>
        ///  Adjust the account balance of all users in a group by an adjustment amount. An adjustment may be positive
        ///  (add to the user's account) or negative (subtract from the account).
        /// </summary>
        ///
        /// <param name="group">
        ///  The group for which all users' accounts are to be adjusted.
        /// </param>
        /// <param name="adjustment">
        ///  The adjustment amount. Positive to add credit and negative to subtract.
        /// </param>
        /// <param name="comment">
        ///  A user defined comment to be associated with the transaction. This may be a null string.
        /// </param>
        /// <param name="accountName">
        ///  Optional name of the user's personal account. If blank, the built-in default account is used.
        /// </param>
        private void AdjustUserAccountBalanceByGroup(string group, double adjustment, string comment, string accountName)
        {
            _proxy.AdjustUserAccountBalanceByGroup(_authToken, group, adjustment, comment, accountName);
        }

        /// <summary>
        ///  Adjust the account balance of all users in a group by an adjustment amount. An adjustment may be positive
        ///  (add to the user's account) or negative (subtract from the account).
        /// </summary>
        ///
        /// <param name="group">
        ///  The group for which all users' accounts are to be adjusted.
        /// </param>
        /// <param name="adjustment">
        ///  The adjustment amount. Positive to add credit and negative to subtract.
        /// </param>
        /// <param name="limit">
        ///  Only add balance up to this limit.
        /// </param>
        /// <param name="comment">
        ///  A user defined comment to be associated with the transaction. This may be a null string.
        /// </param>
        /// <param name="accountName">
        ///  Optional name of the user's personal account. If blank, the built-in default account is used.
        /// </param>
        private void AdjustUserAccountBalanceByGroup(string group, double adjustment, double limit, string comment, string accountName)
        {
            _proxy.AdjustUserAccountBalanceByGroup(_authToken, group, adjustment, limit, comment, accountName);
        }

        /// <summary>
        ///  Set the balance on a user's account to a set value. This is conducted as a transaction.
        /// </summary>
        ///
        /// <param name="username">
        ///  The username associated with the user who's account is to be set.
        /// </param>
        /// <param name="balance">
        ///  The balance to set the account to.
        /// </param>
        /// <param name="comment">
        ///  A user defined comment to associate with the transaction. This may be a null string.
        /// </param>
        /// <param name="accountName">
        ///  Optional name of the user's personal account. If blank, the built-in default account is used.
        /// </param>
        private void SetUserAccountBalance(string username, double balance, string comment, string accountName)
        {
            _proxy.SetUserAccountBalance(_authToken, username, balance, comment, accountName);
        }

        /// <summary>
        ///  Set the balance for each member of a group to the given value.
        /// </summary>
        ///
        /// <param name="group">
        ///  The group for which all users' balance is to be set.
        /// </param>
        /// <param name="balance">
        ///  The value to set all users' balance to.
        /// </param>
        /// <param name="comment">
        ///  A user defined comment to associate with the transaction. This may be a null string.
        /// </param>
        /// <param name="accountName">
        ///  Optional name of the user's personal account. If blank, the built-in default account is used.
        /// </param>
        private void SetUserAccountBalanceByGroup(string group, double balance, string comment, string accountName)
        {
            _proxy.SetUserAccountBalanceByGroup(_authToken, group, balance, comment, accountName);
        }

        /// <summary>
        ///  Reset the counts (pages and job counts) associated with a user account.
        /// </summary>
        ///
        /// <param name="username">
        ///  The username associated with the user who's counts are to be reset.
        /// </param>
        /// <param name="resetBy">
        ///  The name of the user/script/process reseting the counts.
        /// </param>
        private void ResetUserCounts(string username, string resetBy)
        {
            _proxy.ResetUserCounts(_authToken, username, resetBy);
        }

        /// <summary>
        ///  Re-applies initial user settings on the given user. These initial settings are based on group membership.
        /// </summary>
        ///
        /// <param name="username">
        ///  The user's username
        /// </param>
        private void ReapplyInitialUserSettings(string username)
        {
            _proxy.ReapplyInitialUserSettings(_authToken, username);
        }

        /// <summary>
        ///  Disable printing for a user for a specified period of time.
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user to disable printing for.
        /// </param>
        /// <param name="disableMins">
        ///  The number of minutes to disable printing for the user. If the value is -1 the printer will be disabled for all
        ///  time until re-enabled.
        /// </param>
        private void DisablePrintingForUser(string username, int disableMins)
        {
            _proxy.DisablePrintingForUser(_authToken, username, disableMins);
        }

        /// <summary>
        ///  Trigger the process of adding a new user account. Assuming the user exists in the OS/Network/Domain user
        ///  directory, the account will be created with the correct initial settings as defined by the rules setup in the
        ///  admin interface under the Group's section.
        ///
        ///  Calling this method is equivalent to triggering the "new user" event when a new user performs printing for the
        ///  first time.
        /// </summary>
        ///
        /// <param name="username">
        ///  The username of the user to add.
        /// </param>
        private void AddNewUser(string username)
        {
            _proxy.AddNewUser(_authToken, username);
        }

        /// <summary>
        ///  Rename a user account.  Useful when the user has been renamed in the domain / directory, so that usage history
        ///  can be maintained for the new username.  This should be performed in conjunction with a rename of the user in
        ///  the domain / user directory, as all future usage and authentication will need to use the new username.
        /// </summary>
        ///
        /// <param name="currentUserName">
        ///  The username of the user to rename.
        /// </param>
        /// <param name="newUserName">
        ///  The user's new username.
        /// </param>
        private void RenameUserAccount(string currentUserName, string newUserName)
        {
            _proxy.RenameUserAccount(_authToken, currentUserName, newUserName);
        }

        /// <summary>
        /// Delete/remove an existing user from the user list. Use this method with care.  Calling this will
        /// permanently delete the user account from the user list (print and transaction history records remain).
        /// </summary>
        ///
        /// <param name="username">
        /// 	The username of the user to delete/remove.
        /// </param>
        /// <param name="redactUserData">
        ///  	If true, in addition to deletion permanently redact user data (default false)
        /// </param>
        private void DeleteExistingUser(string username, bool redactUserData = false)
        {
            _proxy.DeleteExistingUser(_authToken, username, redactUserData);
        }

        /// <summary>
        /// Creates and sets up a new internal user account.  The (unique) username and password are required at a minimum.
        /// The other properties are optional and will be used if not blank.  Properties may also be set after creation
        /// using <see cref="SetUserProperty(string,string,string)" /> or <see cref="SetUserProperties(string,string[][])" />.
        /// </summary>
        ///
        /// <param name="userName">
        ///  (required) A unique username.  An exception is thrown if the username already exists.
        /// </param>
        /// <param name="password">
        ///  (required) The user's password.
        /// </param>
        /// <param name="fullName">
        ///  (optional) The full name of the user.
        /// </param>
        /// <param name="email">
        ///  (optional) The email address of the user.
        /// </param>
        /// <param name="cardId">
        ///  (optional) The card/identity number of the user.
        /// </param>
        /// <param name="pin">
        ///  The card/id pin.
        /// </param>
        /// <param name="sendEmail">
        ///  Whether or not we want to send a confirmation email to the created user
        /// </param>
        private void AddNewInternalUser(string userName, string password, string fullName, string email, string cardId,
            string pin, bool sendEmail = false)
        {
            _proxy.AddNewInternalUser(_authToken, userName, password, fullName, email, cardId, pin, sendEmail);
        }

        /// <summary>
        ///  Export user data based on a set of predefined CSV reports (The owner of these files will be the system account running the PaperCut process)
        /// </summary>
        /// <param name="userName">
        ///  The user name of interest
        /// </param>
        /// <param name="saveLocation">
        ///  Location on the PaperCut MF/NG application server to export CSV reports to.
        ///  The system account running the PaperCut process must have write permissions to this location.
        /// </param>
        private void ExportUserDataHistory(string userName, string saveLocation)
        {
            _proxy.ExportUserDataHistory(_authToken, userName, saveLocation);
        }

        /// <summary>
        ///  Looks up the user with the given user id number and returns their user name.  If no match was found an empty
        ///  string is returned.
        /// </summary>
        ///
        /// <param name="idNo">
        ///  The user id number to look up.
        /// </param>
        /// <returns>
        ///  The matching user name, or an empty string if there was no match.
        /// </returns>
        private string LookUpUserNameByIDNo(string idNo)
        {
            return _proxy.LookUpUserNameByIDNo(_authToken, idNo);
        }

        /// <summary>
        ///  Looks up the user with the given user card number and returns their user name.  If no match was found an empty
        ///  string is returned.
        /// </summary>
        ///
        /// <param name="cardNo">
        ///  The user card number to look up.
        /// </param>
        /// <returns>
        ///  The matching user name, or an empty string if there was no match.
        /// </returns>
        private string LookUpUserNameByCardNo(string cardNo)
        {
            return _proxy.LookUpUserNameByCardNo(_authToken, cardNo);
        }


        /// <summary>
        ///  Looks up the user with the primary email address and returns their user name.  If no match was found an empty
        ///  string is returned.
        /// </summary>
        ///
        /// <param name="email">
        ///  The user's primary email to look up.
        /// </param>
        /// <returns>
        ///  The matching user name, or an empty string if there was no match.
        /// </returns>
        private string LookUpUserNameByEmail(string email)
        {
            return _proxy.LookUpUserNameByEmail(_authToken, email);
        }

        /// <summary>
        ///  Looks up the user with the given alias (secondary username) and returns their user name.  If no match was found an empty
        ///  string is returned.
        /// </summary>
        ///
        /// <param name="secondaryUserName">
        ///  The user alias to look up.
        /// </param>
        /// <returns>
        ///  The matching user name, or an empty string if there was no match.
        /// </returns>
        private string LookUpUserNameBySecondaryUserName(string secondaryUserName)
        {
            return _proxy.LookUpUserNameBySecondaryUserName(_authToken, secondaryUserName);
        }
        /// <summary>
        ///  Looks up all the users with the given full names and returns their user names.  If no match was found an empty
        ///  string is returned.
        /// </summary>
        ///
        /// <param name="fullName">
        ///  The user full name to look up. Note: Full names don't have to be unique
        /// </param>
        /// <returns>
        ///  A list of matching usernames, or an empty string if there was no match.
        /// </returns>
        private string[] LookUpUsersByFullName(string fullName)
        {
            return _proxy.LookUpUsersByFullName(_authToken, fullName);
        }


        /// <summary>
        ///  Adds the user to the specified group
        /// </summary>
        /// <param name="username">
        ///  The user name
        /// </param>
        /// <param name="groupName">
        ///  The name of the group.
        /// </param>
        private void AddUserToGroup(string username, string groupName)
        {
            _proxy.AddUserToGroup(_authToken, username, groupName);
        }

        /// <summary>
        ///  Removes the user from the specified group.
        /// </summary>
        ///
        /// <param name="username">
        ///  The user name
        /// </param>
        /// <param name="groupName">
        ///  The name of the group.
        /// </param>
        private void RemoveUserFromGroup(string username, string groupName)
        {
            _proxy.RemoveUserFromGroup(_authToken, username, groupName);
        }


        /// <summary>
        ///  Adds a user as an admin with default admin rights.
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user.
        /// </param>
        private void AddAdminAccessUser(string username)
        {
            _proxy.AddAdminAccessUser(_authToken, username);
        }

        /// <summary>
        ///  Removes an admin user from the list of admins.
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user.
        /// </param>
        private void RemoveAdminAccessUser(string username)
        {
            _proxy.RemoveAdminAccessUser(_authToken, username);
        }

        /// <summary>
        ///  Adds a group as an admin group with default admin rights.
        /// </summary>
        ///
        /// <param name="groupName">
        ///  The name of the group.
        /// </param>
        private void AddAdminAccessGroup(string groupName)
        {
            _proxy.AddAdminAccessGroup(_authToken, groupName);
        }

        /// <summary>
        ///  Removes a group from the list of admin groups.
        /// </summary>
        ///
        /// <param name="groupName">
        ///  The name of the group.
        /// </param>
        private void RemoveAdminAccessGroup(string groupName)
        {
            _proxy.RemoveAdminAccessGroup(_authToken, groupName);
        }

        /// <summary>
        /// List all user accounts (sorted by username) starting at 'offset' and ending at 'limit'.
        /// This can be used to enumerate all user accounts in 'pages'.  When retrieving a list of all user accounts, the
        /// recommended page size / limit is 1000.  Batching in groups of 1000 ensures efficient transfer and
        /// processing.
        /// E.g.:
        ///   listUserAccounts(0, 1000) - returns users 0 through 999
        ///   listUserAccounts(1000, 1000) - returns users 1000 through 1999
        ///   listUserAccounts(2000, 1000) - returns users 2000 through 2999
        /// </summary>
        ///
        /// <param name="offset">
        ///  The 0-index offset in the list of users to return.  I.e. 0 is the first user, 1 is the second, etc.
        /// </param>
        /// <param name="limit">
        ///  The number of accounts to return in this batch.  Recommended: 1000.
        /// </param>
        /// <returns>
        ///  An array of user names.
        /// </returns>
        private string[] ListUserAccounts(int offset, int limit)
        {
            return _proxy.ListUserAccounts(_authToken, offset, limit);
        }

        /// <summary>
        /// List all shared accounts (sorted by account name) starting at <code>offset</code> and ending at <code>limit</code>.
        /// This can be used to enumerate all shared accounts in 'pages'.  When retrieving a list of all shared accounts, the
        /// recommended page size / limit is <code>1000</code>.  Batching in groups of 1000 ensures efficient transfer and
        /// processing.
        /// E.g.:
        ///   listSharedAccounts(0, 1000) - returns accounts 0 through 999
        ///   listSharedAccounts(1000, 1000) - returns accounts 1000 through 1999
        ///   listSharedAccounts(2000, 1000) - returns accounts 2000 through 2999
        /// </summary>
        ///
        /// <param name="offset">
        ///  The 0-index offset in the list of accounts to return.  I.e. 0 is the first account, 1 is the second, etc.
        /// </param>
        /// <param name="limit">
        ///  The number of users to return in this batch.  Recommended: 1000.
        /// </param>
        /// <returns>
        ///  An array of shared accounts names.
        /// </returns>
        private string[] ListSharedAccounts(int offset, int limit)
        {
            return _proxy.ListSharedAccounts(_authToken, offset, limit);
        }


        /// <summary>
        /// Get the count of all users in the system.
        /// </summary>
        ///
        /// <returns>
        ///  Numeric total of all user accounts.
        /// </returns>
        private int GetTotalUsers()
        {
            return _proxy.GetTotalUsers(_authToken);
        }

        /// <summary>
        /// List all shared accounts (sorted by account name) that the user has access to, starting at <code>offset</code>
        /// and listing only <code>limit</code> accounts. This can be used to enumerate all shared accounts in 'pages'.
        /// When retrieving a list of all shared accounts, the recommended page size / limit is <code>1000</code>.
        /// Batching in groups of 1000 ensures efficient transfer and processing.
        /// E.g.:
        ///  listUserSharedAccounts("user", 0, 1000) - returns accounts 0 through 999
        ///  listUserSharedAccounts("user", 1000, 1000) - returns accounts 1000 through 1999
        ///  listUserSharedAccounts("user", 2000, 1000) - returns accounts 2000 through 2999
        /// </summary>
        ///
        /// <param name="username">
        ///  The user's name.
        /// </param>
        /// <param name="offset">
        ///  The 0-index offset in the list of accounts to return.  I.e. 0 is the first account, 1 is the second, etc.
        /// </param>
        /// <param name="limit">
        ///  The number of accounts to return in this batch.  Recommended: 1000.
        /// </param>
        /// <param name="ignoreAccountMode">
        ///  If true, list accounts regardless of current shared account mode.
        /// </param>
        /// <returns>
        ///  An array of shared accounts names the user has access to.
        /// </returns>
        private string[] ListUserSharedAccounts(string username, int offset, int limit, bool ignoreAccountMode = false)
        {
            return _proxy.ListUserSharedAccounts(_authToken, username, offset, limit, ignoreAccountMode);
        }

        /// <summary>
        ///  Test to see if a shared account exists.
        /// </summary>
        ///
        /// <param name="accountName">
        ///  The name of the shared account.
        /// </param>
        /// <returns>
        ///  Return true if the shared account exists, else false.
        /// </returns>
        private bool SharedAccountExists(string accountName)
        {
            return _proxy.SharedAccountExists(_authToken, accountName);
        }

        /// <summary>
        ///  Gets a shared account's current balance.
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account.
        /// </param>
        /// <returns>
        ///  The value of the account's current balance.
        /// </returns>
        private double GetSharedAccountAccountBalance(string sharedAccountName)
        {
            return _proxy.GetSharedAccountAccountBalance(_authToken, sharedAccountName);
        }


        /// <summary>
        ///  Get the shared account's overdraft mode
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account.
        /// </param>
        /// <returns>
        ///  the shared account's overdraft mode ('individual' or 'default')
        /// </returns>
        private string GetSharedAccountOverdraftMode(string sharedAccountName)
        {
            return _proxy.GetSharedAccountOverdraftMode(_authToken, sharedAccountName);
        }

        /// <summary>
        ///  Set the shared account's overdraft mode
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account.
        /// </param>
        /// <param name="mode">
        ///  the shared account's new overdraft mode ('individual' or 'default')
        /// </param>
        private void SetSharedAccountOverdraftMode(string sharedAccountName, string mode)
        {
            _proxy.SetSharedAccountOverdraftMode(_authToken, sharedAccountName, mode);
        }

        /// <summary>
        ///  Disable shared account for a specified period of time.
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account to disable.
        /// </param>
        /// <param name="disableMins">
        ///  The number of minutes to disable the shared account for. If the value is -1 the shared account will be disabled permanently
        ///  until re-enabled.
        /// </param>
        private void DisableSharedAccount(string sharedAccountName, int disableMins)
        {
            _proxy.DisableSharedAccount(_authToken, sharedAccountName, disableMins);
        }

        /// <summary>
        ///  Gets a shared account property.
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account.
        /// </param>
        /// <param name="propertyName">
        ///  The name of the property to get.  Valid options include: access-groups, access-users, account-id, balance,
        ///  comment-option, disabled, invoice-option, notes, overdraft-amount, pin, restricted.
        /// </param>
        /// <returns>
        ///  The value of the requested property.
        /// </returns>
        ///
        /// <see cref="SetSharedAccountProperty(string,string,string)" />
        private string GetSharedAccountProperty(string sharedAccountName, string propertyName)
        {
            return _proxy.GetSharedAccountProperty(_authToken, sharedAccountName, propertyName);
        }

        /// <summary>
        ///  Get multiple shared account properties at once (to save multiple calls).
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The shared account name.
        /// </param>
        /// <param name="propertyNames">
        ///  The names of the properties to get.  See <see cref="GetSharedAccountProperty(string,string)" /> for valid property names.
        /// </param>
        /// <returns>
        ///  The property values (in the same order as given in <paramref param="propertyNames" />.
        /// </returns>
        ///
        /// <see cref="GetSharedAccountProperty(string,string)" />
        /// <see cref="SetSharedAccountProperties(string,string[][])" />
        private string[] GetSharedAccountProperties(string sharedAccountName, string[] propertyNames)
        {
            return _proxy.GetSharedAccountProperties(_authToken, sharedAccountName, propertyNames);
        }

        /// <summary>
        ///  Sets a shared account property.
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account.
        /// </param>
        /// <param name="propertyName">
        ///  The name of the property to set.  See <see cref="GetSharedAccountProperty(string,string)" /> for valid property names.
        /// </param>
        /// <param name="propertyValue">
        ///  The value of the property to set.
        /// </param>
        ///
        /// <see cref="GetSharedAccountProperty(string,string)" />
        private void SetSharedAccountProperty(string sharedAccountName, string propertyName, string propertyValue)
        {
            _proxy.SetSharedAccountProperty(_authToken, sharedAccountName, propertyName, propertyValue);
        }

        /// <summary>
        ///  Set multiple shared account properties at once (to save multiple calls).
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The shared account name.
        /// </param>
        /// <param name="propertyNamesAndValues">
        ///  The list of property names and values to set. E.g. [["balance", "1.20"], ["invoice-option", "ALWAYS_INVOICE"]].
        ///  See <see cref="SetSharedAccountProperty(string,string,string)" /> for valid property names.
        /// </param>
        ///
        /// <see cref="GetSharedAccountProperties(string,string[])" />
        /// <see cref="SetSharedAccountProperty(string,string,string)" />
        private void SetSharedAccountProperties(string sharedAccountName, string[][] propertyNamesAndValues)
        {
            _proxy.SetSharedAccountProperties(_authToken, sharedAccountName, propertyNamesAndValues);
        }

        /// <summary>
        ///  Adjust a shared account's account balance by an adjustment amount. An adjustment bay be positive (add to the
        ///  account) or negative (subtract from the account).
        /// </summary>
        ///
        /// <param name="accountName">
        ///  The full name of the shared account to adjust.
        /// </param>
        /// <param name="adjustment">
        ///  The adjustment amount. Positive to add credit and negative to subtract.
        /// </param>
        /// <param name="comment">
        ///  A user defined comment to associated with the transaction. This may be a null string.
        /// </param>
        private void AdjustSharedAccountAccountBalance(string accountName, double adjustment, string comment)
        {
            _proxy.AdjustSharedAccountAccountBalance(_authToken, accountName, adjustment, comment);
        }

        /// <summary>
        ///  Set a shared account's account balance.
        /// </summary>
        ///
        /// <param name="accountName">
        ///  The name of the account to be adjusted.
        /// </param>
        /// <param name="balance">
        ///  The balance to set (positive or negative).
        /// </param>
        /// <param name="comment">
        ///  The comment to be associated with the transaction.
        /// </param>
        private void SetSharedAccountAccountBalance(string accountName, double balance, string comment)
        {
            _proxy.SetSharedAccountAccountBalance(_authToken, accountName, balance, comment);
        }

        /// <summary>
        ///  Create a new shared account with the given name.
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account to create. Use a '\' to denote a subaccount, e.g.: 'parent\sub'
        /// </param>
        private void AddNewSharedAccount(string sharedAccountName)
        {
            _proxy.AddNewSharedAccount(_authToken, sharedAccountName);
        }

        /// <summary>
        ///  Rename an existing shared account.
        /// </summary>
        ///
        /// <param name="currentSharedAccountName">
        ///  The name of the shared account to rename. Use a '\' to denote a subaccount. e.g.: 'parent\sub'
        /// </param>
        /// <param name="newSharedAccountName">
        /// The new shared account name.
        /// </param>
        private void RenameSharedAccount(string currentSharedAccountName, string newSharedAccountName)
        {
            _proxy.RenameSharedAccount(_authToken, currentSharedAccountName, newSharedAccountName);
        }

        /// <summary>
        ///  Delete a shared account from the system.  Use this method with care.  Deleting a shared account will
        ///  permanently delete it from the shared account list (print history records will remain).
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account to delete.
        /// </param>
        private void DeleteExistingSharedAccount(string sharedAccountName)
        {
            _proxy.DeleteExistingSharedAccount(_authToken, sharedAccountName);
        }

        /// <summary>
        ///  Allow the given user access to the given shared account without using a pin.
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account to allow access to.
        /// </param>
        /// <param name="username">
        ///  The name of the user to give access to.
        /// </param>
        private void AddSharedAccountAccessUser(string sharedAccountName, string username)
        {
            _proxy.AddSharedAccountAccessUser(_authToken, sharedAccountName, username);
        }

        /// <summary>
        ///  Allow the given group access to the given shared account without using a pin.
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account to allow access to.
        /// </param>
        /// <param name="groupName">
        ///  The name of the group to give access to.
        /// </param>
        private void AddSharedAccountAccessGroup(string sharedAccountName, string groupName)
        {
            _proxy.AddSharedAccountAccessGroup(_authToken, sharedAccountName, groupName);
        }

        /// <summary>
        ///  Revoke the given user's access to the given shared account.
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account to revoke access to.
        /// </param>
        /// <param name="username">
        ///  The name of the user to revoke access for.
        /// </param>
        private void RemoveSharedAccountAccessUser(string sharedAccountName, string username)
        {
            _proxy.RemoveSharedAccountAccessUser(_authToken, sharedAccountName, username);
        }

        /// <summary>
        ///  Revoke the given group's access to the given shared account.
        /// </summary>
        ///
        /// <param name="sharedAccountName">
        ///  The name of the shared account to revoke access to.
        /// </param>
        /// <param name="groupName">
        ///  The name of the group to revoke access for.
        /// </param>
        private void RemoveSharedAccountAccessGroup(string sharedAccountName, string groupName)
        {
            _proxy.RemoveSharedAccountAccessGroup(_authToken, sharedAccountName, groupName);
        }

        /// <summary>
        ///  Gets a printer property.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The name of the server.
        /// </param>
        /// <param name="printerName">
        ///  The name of the printer.
        /// </param>
        /// <param name="propertyName">
        ///  The name of the property.  Valid options include: cost-model, custom-field-1, custom-field-2, custom-field-3,
        ///  custom-field-4, custom-field-5, custom-field-6, disabled, print-stats.job-count, print-stats.page-count,
        ///  printer-id
        /// </param>
        /// <returns>
        ///  The value of the requested property.
        /// </returns>
        private string GetPrinterProperty(string serverName, string printerName, string propertyName)
        {
            return _proxy.GetPrinterProperty(_authToken, serverName, printerName, propertyName);
        }

        /// <summary>
        ///  Gets a list printer properties.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The name of the server.
        /// </param>
        /// <param name="printerName">
        ///  The name of the printer.
        /// </param>
        /// <param name="propertyNames">
        ///  An array of strings, one for each property name
        /// </param>
        /// <returns>
        ///  An array of requested property values (same order as the requested array).
        /// </returns>
        private string[] GetPrinterProperties(string serverName, string printerName, string[] propertyNames)
        {
            return _proxy.GetPrinterProperties(_authToken, serverName, printerName, propertyNames);
        }

        /// <summary>
        ///  Sets a printer property.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The name of the server.
        /// </param>
        /// <param name="printerName">
        ///  The name of the printer.
        /// </param>
        /// <param name="propertyName">
        ///  The name of the property.  Valid options include: disabled.
        /// </param>
        /// <param name="propertyValue">
        ///  The value of the property to set.
        /// </param>
        private void SetPrinterProperty(string serverName, string printerName, string propertyName, string propertyValue)
        {
            _proxy.SetPrinterProperty(_authToken, serverName, printerName, propertyName, propertyValue);
        }
        /// <summary>
        ///  Sets multiple  printer properties.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The name of the server.
        /// </param>
        /// <param name="printerName">
        ///  The name of the printer.
        /// </param>
        /// <param name="propertyNamesAndValues">
        ///  An array of arrays. Names value pairs
        /// </param>
        private void SetPrinterProperties(string serverName, string printerName, string[,] propertyNamesAndValues)
        {
            _proxy.SetPrinterProperties(_authToken, serverName, printerName, propertyNamesAndValues);
        }


        private bool ApplyDeviceSettings(string deviceName)
        {
            return _proxy.ApplyDeviceSettings(_authToken, deviceName);
        }

        /// <summary>
        /// List all printers (sorted by printer name) starting at 'offset' and ending at 'limit'.
        /// This can be used to enumerate all printers in 'pages'.  When retrieving a list of all printers, the
        /// recommended page size / limit is 1000.  Batching in groups of 1000 ensures efficient transfer and
        /// processing.
        /// E.g.:
        ///   listPrinters(0, 1000) - returns users 0 through 999
        ///   listPrinters(1000, 1000) - returns users 1000 through 1999
        ///   listPrinters(2000, 1000) - returns users 2000 through 2999
        /// </summary>
        ///
        /// <param name="offset">
        ///  The 0-index offset in the list of printers to return.  I.e. 0 is the first printer, 1 is the second, etc.
        /// </param>
        /// <param name="limit">
        ///  The number of printers to return in this batch.  Recommended: 1000.
        /// </param>
        /// <returns>
        ///  An array of printers.
        /// </returns>
        private string[] ListPrinters(int offset, int limit)
        {
            return _proxy.ListPrinters(_authToken, offset, limit);
        }

        /// <summary>
        ///  Reset the counts (pages and job counts) associated with a printer.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The name of the server hosting the printer.
        /// </param>
        /// <param name="printerName">
        ///  The printer's name.
        /// </param>
        /// <param name="resetBy">
        ///  The name of the user/script/process resetting the counts.
        /// </param>
        ///
        private void ResetPrinterCounts(string serverName, string printerName, string resetBy)
        {
            _proxy.ResetPrinterCounts(_authToken, serverName, printerName, resetBy);
        }

        /// <summary>
        ///  Disable a printer for select period of time.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The name of the server hosting the printer.
        /// </param>
        /// <param name="printerName">
        ///  The printer's name.
        /// </param>
        /// <param name="disableMins">
        ///  The number of minutes to disable the printer. If the value is -1 the printer will be disabled for all
        ///  time until re-enabled.
        /// </param>
        private void DisablePrinter(string serverName, string printerName, int disableMins)
        {
            _proxy.DisablePrinter(_authToken, serverName, printerName, disableMins);
        }

        /// <summary>
        ///  Delete a printer.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The name of the server hosting the printer.
        /// </param>
        /// <param name="printerName">
        ///  The printer's name.
        /// </param>
        private void DeletePrinter(string serverName, string printerName)
        {
            _proxy.DeletePrinter(_authToken, serverName, printerName);
        }

        /// <summary>
        ///  Rename a printer.  This can be useful after migrating a print queue or print server (i.e. the printer retains
        ///  its history and settings under the new name).  Note that in some cases case sensitivity is important, so care
        ///  should be taken to enter the name exactly as it appears in the OS.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The existing printer's server name.
        /// </param>
        /// <param name="printerName">
        ///  The existing printer's queue name.
        /// </param>
        /// <param name="newServerName">
        ///  The new printer's server name.
        /// </param>
        /// <param name="newPrinterName">
        ///  The new printer's queue name.
        /// </param>
        private void RenamePrinter(string serverName, string printerName, string newServerName, string newPrinterName)
        {
            _proxy.RenamePrinter(_authToken, serverName, printerName, newServerName, newPrinterName);
        }

        /// <summary>
        ///  Add the group to the printer access group list.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The existing printer's server name.
        /// </param>
        /// <param name="printerName">
        ///  The existing printer's queue name.
        /// </param>
        /// <param name="groupName">
        ///  The name of the group that needs to be added to the printer group restrictions list
        /// </param>
        private void AddPrinterAccessGroup(string serverName, string printerName, string groupName)
        {
            _proxy.AddPrinterAccessGroup(_authToken, serverName, printerName, groupName);
        }

        /// <summary>
        ///  Removes the group from the printer access group list.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The existing printer's server name.
        /// </param>
        /// <param name="printerName">
        ///  The existing printer's queue name.
        /// </param>
        /// <param name="groupName">
        ///  The name of the group that needs to be removed from the list of groups allowed to print to this printer.
        /// </param>
        private void RemovePrinterAccessGroup(string serverName, string printerName, string groupName)
        {
            _proxy.RemovePrinterAccessGroup(_authToken, serverName, printerName, groupName);
        }

        /// <summary>
        ///  Method to set a simple single page cost using the Simple Charging Model.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The name of the server.
        /// </param>
        /// <param name="printerName">
        ///  The name of the printer.
        /// </param>
        /// <param name="costPerPage">
        ///  The cost per page (simple charging model)
        /// </param>
        private void SetPrinterCostSimple(string serverName, string printerName, double costPerPage)
        {
            _proxy.SetPrinterCostSimple(_authToken, serverName, printerName, costPerPage);
        }

        /// <summary>
        ///  Get the page cost if, and only if, the printer is using the Simple Charging Model.
        /// </summary>
        ///
        /// <param name="serverName">
        ///  The name of the server.
        /// </param>
        /// <param name="printerName">
        ///  The name of the printer.
        /// </param>
        /// <returns>
        ///  The default page cost. On failure an exception is thrown.
        /// </returns>
        private double GetPrinterCostSimple(string serverName, string printerName)
        {
            return _proxy.GetPrinterCostSimple(_authToken, serverName, printerName);
        }

        /// <summary>
        ///  Add a new group to system's group list.  The caller is responsible for ensuring that the supplied group name is
        ///  valid and exists in the linked user directory source.  The status of this method may be monitored with calls to
        ///  <code>getTaskStatus()</code>.
        /// </summary>
        ///
        /// <param name="groupName">
        ///  The name of the new group to add. The group should already exist in the network user directory.
        /// </param>
        private void AddNewGroup(string groupName)
        {
            _proxy.AddNewGroup(_authToken, groupName);
        }

        /// <summary>
        ///  Syncs an existing group with the configured directory server, updates the group membership.
        /// </summary>
        ///
        /// <param name="groupName">
        ///  The name of the new group to sync. The group should already exist in the network user directory.
        /// </param>
        /// <returns>
        ///  <code>True</code> if successful.  On failure an exception is thrown.
        /// </returns>
        private bool SyncGroup(string groupName)
        {
            return _proxy.SyncGroup(_authToken, groupName);
        }

        /// <summary>
        ///  Removes the user group.
        /// </summary>
        ///
        /// <param name="groupName">
        ///  The name of the group that needs to be deleted.
        /// </param>
        private void RemoveGroup(string groupName)
        {
            _proxy.RemoveGroup(_authToken, groupName);
        }

        /// <summary>
        /// List all user groups (sorted by groupname) starting at 'offset' and ending at 'limit'.
        /// This can be used to enumerate all groups in 'pages'.  When retrieving a list of all groups, the
        /// recommended page size / limit is 1000.  Batching in groups of 1000 ensures efficient transfer and
        /// processing.
        /// E.g.:
        ///   listUserGroups(0, 1000) - returns users 0 through 999
        ///   listUserGroups(1000, 1000) - returns users 1000 through 1999
        ///   listUserGroups(2000, 1000) - returns users 2000 through 2999
        /// </summary>
        ///
        /// <param name="offset">
        ///  The 0-index offset in the list of groups to return.  I.e. 0 is the first group, 1 is the second, etc.
        /// </param>
        /// <param name="limit">
        ///  The number of groups to return in this batch.  Recommended: 1000.
        /// </param>
        /// <returns>
        ///  An array of user groups.
        /// </returns>
        private string[] ListUserGroups(int offset, int limit)
        {
            return _proxy.ListUserGroups(_authToken, offset, limit);
        }

        /// <summary>
        /// Retrive all groups a user is a member of.
        /// </summary>
        /// <param name="userName">The username to look up</param>
        /// <returns>An array of Group Names the user belongs to</returns>
        private string[] GetUserGroups(string userName)
        {
            return _proxy.GetUserGroups(_authToken, userName);
        }

        /// <summary>
        /// Retrive users in group.
        /// </summary>
        /// <param name="groupName">The group to look up</param>
        /// <param name="offset"> The 0-index offset in the list of users to return.  I.e. 0 is the first users, 1 is the second, etc</param>
        /// <param name="batchSize">The number of users to return in this batch.  Recommended: 1000.</param>
        /// <returns>An array of User Names in the group</returns>
        private string[] GetGroupMembers(string groupName, int offset, int batchSize)
        {
            return _proxy.GetGroupMembers(_authToken, groupName, offset, batchSize);
        }

        /// <summary>
        ///  Test to see if a group associated with groupname exists in the system.
        /// </summary>
        ///
        /// <param name="groupName">
        ///  The groupname to test.
        /// </param>
        /// <returns>
        ///  Returns true if the group exists in the system, else returns false.
        /// </returns>
        private bool GroupExists(string groupName)
        {
            return _proxy.GroupExists(_authToken, groupName);
        }

        /// <summary>
        ///  Set the group quota allocation settings on a given group.
        /// </summary>
        ///
        /// <param name="groupName">
        ///  The name of the group.
        /// </param>
        /// <param name="quotaAmount">
        ///  The quota amount to set.
        /// </param>
        /// <param name="period">
        ///  The schedule period (one of either NONE, DAILY, WEEKLY or MONTHLY);
        /// </param>
        /// <param name="quotaMaxAccumulation">
        ///  The maximum quota accumulation.
        /// </param>
        private void SetGroupQuota(string groupName, double quotaAmount, string period, double quotaMaxAccumulation)
        {
            _proxy.SetGroupQuota(_authToken, groupName, quotaAmount, period, quotaMaxAccumulation);
        }

        /// <summary>
        ///  Get the group quota allocation settings on a given group.
        /// </summary>
        ///
        /// <param name="groupName">
        ///  The name of the group.
        /// </param>
        /// <returns>
        ///  A struct containing the quota amount, quota period and max accumulation amount.
        /// </returns>
        private GetGroupQuotaResponse GetGroupQuota(string groupName)
        {
            return _proxy.GetGroupQuota(_authToken, groupName);
        }

        /// <summary>
        ///  Apply the value of a card to a user's account.
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user with the account to credit.
        /// </param>
        /// <param name="cardNumber">
        ///  The number of the card to use.
        /// </param>
        /// <returns>
        ///  A string indicating the outcome, such as SUCCESS, UNKNOWN_USER, INVALID_CARD_NUMBER, CARD_IS_USED or
        ///  CARD_HAS_EXPIRED.
        /// </returns>
        private string UseCard(string username, string cardNumber)
        {
            return _proxy.UseCard(_authToken, username, cardNumber);
        }


        ///<summary>
        ///Return the status (completed flag and any status message text) associated with a long running task such as
        /// a sync operation started by the performGroupSync API.
        /// </summary>
        ///
        /// <returns>
        ///struct providing information about the current or latest task started via this API.
        /// </returns>

        private GetTaskStatusResponse GetTaskStatus()
        {
            return _proxy.GetTaskStatus();
        }

        /// <summary>
        ///  Instigate an online backup. This process is equivalent to pressing the manual backup button in the web based
        ///  admin interface. The data is expected into the server/data/backups directory as a timestamped, zipped XML file.
        /// </summary>
        ///
        private void PerformOnlineBackup()
        {
            _proxy.PerformOnlineBackup(_authToken);
        }

        /// <summary>
        ///  Start the process of synchronizing the system's group membership with the OS/Network/Domain's group membership.
        ///  The call to this method will start the synchronization process. The operation will commence and complete in the
        ///  background.
        /// </summary>
        ///
        private void PerformGroupSync()
        {
            _proxy.PerformGroupSync(_authToken);
        }

        /// <summary>
        ///  Start a full user and group synchronization. This is equivalent to pressing on the "Synchronize Now" button in
        ///  the admin user interface. The behaviour of the sync process, such as deleting old users, is determined by the
        ///  current system settings as defined in the admin interface. A call to this method will commence the sync process
        ///  and the operation will complete in the background.
        /// </summary>
        ///
        private void PerformUserAndGroupSync()
        {
            _proxy.PerformUserAndGroupSync(_authToken);
        }

        /// <summary>
        ///  An advanced version of the user and group synchronization process providing control over the sync behaviour. A
        ///  call to this method will commence the sync process and the operation will complete in the background.
        /// </summary>
        ///
        /// <param name="deleteNonExistentUsers">
        ///  If set to <code>True</code>, old users will be deleted.
        /// </param>
        /// <param name="updateUserDetails">
        ///  If set to <code>True</code>, user details such as full-name, email, etc. will be synced with the
        ///  underlying OS/Network/Domain user directory.
        /// </param>
        ///
        private void PerformUserAndGroupSyncAdvanced(bool deleteNonExistentUsers, bool updateUserDetails)
        {
            _proxy.PerformUserAndGroupSyncAdvanced(_authToken, deleteNonExistentUsers, updateUserDetails);
        }

        /// <summary>
        ///  Calling this method will start a specialized user and group synchronization process optimized for tracking down
        ///  adding any new users that exist in the OS/Network/Domain user directory and not in the system. Any existing user
        ///  accounts will not be modified. A group synchronization will only be performed if new users are actually added to
        ///  the system.
        /// </summary>
        ///
        private void AddNewUsers()
        {
            _proxy.AddNewUsers(_authToken);
        }

        /// <summary>
        ///  Import the shared accounts contained in the given TSV import file.
        /// </summary>
        ///
        /// <param name="importFile">
        ///  The import file location relative to the application server.
        /// </param>
        /// <param name="test">
        ///  If true, perform a test only. The printed statistics will show what would have occurred if testing wasn't
        ///  enabled. No accounts will be modified.
        /// </param>
        /// <param name="deleteNonExistentAccounts">
        ///  If true, accounts that do not exist in the import file but exist in the system will be deleted.  If false, they
        ///  will be ignored.
        /// </param>
        ///
        private string BatchImportSharedAccounts(string importFile, bool test, bool deleteNonExistentAccounts)
        {
            return _proxy.BatchImportSharedAccounts(_authToken, importFile, test, deleteNonExistentAccounts);
        }

        /// <summary>
        ///  Import the users contained in the given tab-delimited import file.
        /// </summary>
        ///
        /// <param name="importFile">
        ///  The import file location relative to the application server.
        /// </param>
        /// <param name="createNewUsers">
        ///  If true, users only existing in the import file will be newly created, otherwise ignored
        /// </param>
        ///
        private void BatchImportUsers(string importFile, bool createNewUsers)
        {
            _proxy.BatchImportUsers(_authToken, importFile, createNewUsers);
        }

        /// <summary>
        ///  Import the internal users contained in the given tab-delimited import file.
        /// </summary>
        ///
        /// <param name="importFile">
        ///  The import file location relative to the application server.
        /// </param>
        /// <param name="overwriteExistingPasswords">
        ///  True to overwrite existing user passwords, false to only update un-set passwords.
        /// </param>
        /// <param name="overwriteExistingPINs">
        ///  True to overwrite existing user PINs, false to only update un-set PINs.
        /// </param>
        ///
        private void BatchImportInternalUsers(string importFile, bool overwriteExistingPasswords,
            bool overwriteExistingPINs)
        {
            _proxy.BatchImportInternalUsers(_authToken, importFile, overwriteExistingPasswords, overwriteExistingPINs);
        }

        /// <summary>
        ///  Import the user card/ID numbers and PINs contained in the given tab-delimited import file.
        /// </summary>
        ///
        /// <param name="importFile">
        ///  The import file location relative to the application server.
        /// </param>
        /// <param name="overwriteExistingPINs">
        ///  If true, users with a PIN already defined will have it overwritten by the PIN in the import file, if specified.
        ///  If false, the existing PIN will not be overwritten.
        /// </param>
        ///
        private void BatchImportUserCardIdNumbers(string importFile, bool overwriteExistingPINs)
        {
            _proxy.BatchImportUserCardIdNumbers(_authToken, importFile, overwriteExistingPINs);
        }

        /// <summary>
        ///  Get the config value from the server.
        /// </summary>
        ///
        /// <param name="configName">
        ///  The name of the config value to retrieve.
        /// </param>
        /// <returns>
        ///  The config value.  If the config value does not exist a blank string is returned.
        /// </returns>
        ///
        private string GetConfigValue(string configName)
        {
            return _proxy.GetConfigValue(_authToken, configName);
        }

        /// <summary>
        ///  Set the config value from the server.
        ///  NOTE: Take care updating config values.  You may cause serious problems which can only be fixed by
        ///        reinstallation of the application. Use the setConfigValue API at your own risk.
        /// </summary>
        ///
        /// <param name="configName">
        ///  The name of the config value to set.
        /// </param>
        /// <param name="configValue">
        ///  The value to set.
        /// </param>
        /// <returns>
        ///  The config value.  If the config value does not exist a blank string is returned.
        /// </returns>
        ///
        private void SetConfigValue(string configName, string configValue)
        {
            _proxy.SetConfigValue(_authToken, configName, configValue);
        }

        /// <summary>
        ///  Takes the details of a job and logs and charges as if it were a "real" job.  Jobs processed via this method are
        ///  not susceptible to filters, pop-ups, hold/release queues etc., they are simply logged.  See the user manual
        ///  section "Importing Job Details" for more information and the format of jobDetails.
        /// </summary>
        ///
        /// <param name="jobDetails">
        ///  The job details (a comma separated list of name-value pairs with an equals sign as the name-value delimiter).
        /// </param>
        ///
        private void ProcessJob(string jobDetails)
        {
            _proxy.ProcessJob(_authToken, jobDetails);
        }

        /// <summary>
        ///  Change the internal admin password.
        /// </summary>
        ///
        /// <param name="newPassword">
        ///  The new password.  Cannot be blank.
        /// </param>
        /// <returns>
        ///  True if the password was successfully changed.
        /// </returns>
        ///
        private bool ChangeInternalAdminPassword(string newPassword)
        {
            return _proxy.ChangeInternalAdminPassword(_authToken, newPassword);
        }

        /// <summary>
        ///  Set the user to Auto Charge to Personal
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user with the account to credit.
        /// </param>
        /// <param name="withPopupConfirmation">
        ///	  If a popup confirmation is to be used (Optional. Defaults to false)
        ///	</param>
        ///
        private void SetUserAccountSelectionAutoChargePersonal(string username, bool withPopupConfirmation = false)
        {
            _proxy.SetUserAccountSelectionAutoChargePersonal(_authToken, username, withPopupConfirmation);
        }

        /// <summary>
        ///  Set the user to Auto Charge to a Single Shared Account
        /// </summary>
        ///
        /// <param name="username">
        ///  The name of the user with the account to credit.
        /// </param>
        ///
        /// <param name="accountName">
        ///  The name of the shared account.
        /// </param>
        ///
        /// <param name="chargeToPersonal">
        ///  Transactions should be primarily charged to the user, not the account
        /// </param>
        ///
        private void SetUserAccountSelectionAutoSelectSharedAccount(string username, string accountName, bool chargeToPersonal)
        {
            _proxy.SetUserAccountSelectionAutoSelectSharedAccount(_authToken, username, accountName, chargeToPersonal);
        }

        /// <summary>
        ///  Set the user to select an acccount from the popup list of approved accounts
        /// </summary>
        /// <param name="username">
        ///  The name of the user with the account to credit.
        /// </param>
        /// <param name="allowPersonal">
        ///  Allows the user to allocate the transaction to their personal account
        /// </param>
        /// <param name="allowListSelection">
        ///  Sets the popup behavior to present a list of approved accounts to the user
        /// </param>
        /// <param name="allowPinCode">
        ///  Sets the popup behavior to allow the user to enter a PIN code to identify an account
        /// </param>
        /// <param name="allowPrintingAsOtherUser">
        ///  Sets the popup behavior to allow the user to supply alternate credentials as the billable user for the job
        /// </param>
        /// <param name="chargeToPersonalWhenSharedSelected">
        ///  When a shared account is selected the user should be charged for the job with a record of the transaction
        ///  attributed to the account
        /// </param>
        private void SetUserAccountSelectionStandardPopup(string username, bool allowPersonal, bool allowListSelection,
            bool allowPinCode, bool allowPrintingAsOtherUser, bool chargeToPersonalWhenSharedSelected)
        {
            _proxy.SetUserAccountSelectionStandardPopup(_authToken, username, allowPersonal, allowListSelection,
                allowPinCode, allowPrintingAsOtherUser, chargeToPersonalWhenSharedSelected);
        }

        /// <summary>
        ///  Set the user to select an acccount from the popup list of approved accounts
        /// </summary>
        /// <param name="username">
        ///  The name of the user with the account to credit.
        /// </param>
        /// <param name="allowPersonal">
        ///  Allows the user to allocate the transaction to their personal account
        /// </param>
        /// <param name="allowListSelection">
        ///  Sets the popup behavior to present a list of approved accounts to the user
        /// </param>
        /// <param name="allowPinCode">
        ///  Sets the popup behavior to allow the user to enter a PIN code to identify an account
        /// </param>
        /// <param name="allowPrintingAsOtherUser">
        ///  Sets the popup behavior to allow the user to supply alternate credentials as the billable user for the job
        /// </param>
        /// <param name="chargeToPersonalWhenSharedSelected">
        ///  When a shared account is selected the user should be charged for the job with a record of the transaction
        ///  attributed to the account
        /// </param>
        /// <param name="defaultSharedAccount">
        ///  The default shared account (blank for none)
        /// </param>
        private void SetUserAccountSelectionStandardPopup(string username, bool allowPersonal, bool allowListSelection,
            bool allowPinCode, bool allowPrintingAsOtherUser, bool chargeToPersonalWhenSharedSelected,
            string defaultSharedAccount)
        {
            _proxy.SetUserAccountSelectionStandardPopup(_authToken, username, allowPersonal, allowListSelection,
                allowPinCode, allowPrintingAsOtherUser, chargeToPersonalWhenSharedSelected, defaultSharedAccount);
        }

        /// <summary>
        ///   Change a user's account selection setting to use the advanced account selection pop-up.
        /// </summary>
        ///
        /// <param name="userName">
        ///   The user's username
        /// </param>
        /// <param name="allowPersonal">
        ///    Allow user to charge to personal account
        /// </param>
        /// <param name="chargeToPersonalWhenSharedSelected">
        ///   true if charge to personal and allocate to shared account.
        /// </param>
        /// <param name="defaultSharedAccount">
        ///   The default shared account (optional)
        /// </param>
        private void SetUserAccountSelectionAdvancedPopup(string userName, bool allowPersonal,
            bool chargeToPersonalWhenSharedSelected, string defaultSharedAccount = "")
        {
            _proxy.SetUserAccountSelectionAdvancedPopup(_authToken, userName, allowPersonal, chargeToPersonalWhenSharedSelected, defaultSharedAccount);
        }

        /// <summary>
        ///		Generate a specified scheduled report
        /// </summary>
        ///
        /// <param name="reportTitle">
        ///		the title of the report
        /// </param>
        /// <param name="saveLocation">
        ///	 the location on the server to save the report to
        ///	</param>
        private bool GenerateScheduledReport(string reportTitle, string saveLocation)
        {
            return _proxy.GenerateScheduledReport(_authToken, reportTitle, saveLocation);
        }

        /// <summary>
        /// 	Generates an AdHoc report
        /// </summary>
        /// <param name="reportType">
        ///		The type of report
        ///	</param>
        /// <param name="dataParams">
        ///		The data parameters for the report
        ///	</param>
        /// <param name="exportTypeExt">
        ///		The export format
        ///	</param>
        /// <param name="reportTitle">
        ///		The prefix of the report title
        ///	</param>
        /// <param name="saveLocation">
        ///		A file path of where to save the report on the server
        ///	</param>
        private bool GenerateAdHocReport(string reportType, string dataParams, string exportTypeExt, string reportTitle, string saveLocation)
        {
            return _proxy.GenerateAdHocReport(_authToken, reportType, dataParams, exportTypeExt, reportTitle, saveLocation);
        }
        #endregion [  defaults  ]
        public List<string> GetAllUsers()
        {
            try
            {
                return ListUserAccounts(0, int.MaxValue).ToList();
            }
            catch (Exception e)
            {
                LoggerExtensions.LogError(_logger, "Error getting users. Returning empty list. Error details {error}", e.ToString());
                return new List<string>();
            }
        }

        public void AddFakeUsersToPapercut(List<string> departments, List<string> offices, int maxUsersToCreate)
        {
            for (var i = 0; i < maxUsersToCreate; i++)
            {
                var user = new User().GetFakeUser(departments, offices);

                LoggerExtensions.LogInformation(_logger, "   CREATING USER {de} OF {ate} - {user}", i + 1, 50, user.Login);

                AddNewUser(user.Login);

                string[] primaryCarNumber = { "primary-card-number", user.PrimaryCardNumber };
                string[] secondaryCardNumber = { "secondary-card-number", user.SecondaryCardNumber };
                string[] userDepartment = { "department", user.Department };
                string[] email = { "email", user.Email };
                string[] fullName = { "full-name", user.FullName };
                string[] userNameAlias = { "username-alias", user.UserNameAlias };
                string[] notes = { "notes", user.Notes };
                string[] userOffice = { "office", user.Office };
                string[] restricted = { "restricted", "FALSE" };
                string[] home = { "home", user.Home };
                string[] cardPin = { "card-pin", user.Pin };

                SetUserProperties(user.Login,
                    new string[][]
                    {
                        primaryCarNumber, secondaryCardNumber, userDepartment, email, fullName, userNameAlias, notes,
                        userOffice, restricted, home, cardPin
                    });
            }
        }

        List<string> IServerCommandOperations.GetAllPrinters()
        {
            try
            {
                return ListPrinters(0, int.MaxValue).ToList(); 
            }
            catch (Exception e)
            {
                LoggerExtensions.LogError(_logger, "Error getting printers. Returning empty list. Error details {error}", e.ToString());
                return new List<string>();
            }
        }

        List<string> IServerCommandOperations.GetAllSharedAccounts()
        {
            try
            {
                return ListSharedAccounts(0, int.MaxValue).ToList();
            }
            catch (Exception e)
            {
                LoggerExtensions.LogError(_logger, "Error getting shared accounts. Returning empty list. Error details {error}", e.ToString());
                return new List<string>();
            }
        }

        void IServerCommandOperations.SaveLog(string job)
        {
            ProcessJob(job);
        }

        void IServerCommandOperations.AddNewUser(string login)
        {
            AddNewUser(login);
        }

        void IServerCommandOperations.SetUserProperty(string login, string propertyName, string propertyValue)
        {
            SetUserProperty(login, propertyName, propertyValue);
        }

        void IServerCommandOperations.SetUserProperties(string login, string[][] propertyNamesAndValues)
        {
            SetUserProperties(login, propertyNamesAndValues);
        }

    }
}