using CookComputing.XmlRpc;
using Shared.Structs;

namespace Application.Interfaces
{
    /// <summary>
    ///  This is an XML-RPC interface to expose the server's APIs.  Used by the standard ServerCommandProxy class below.
    /// </summary>
	public interface IServerCommandProxy : IXmlRpcProxy
	{

		[XmlRpcMethod("api.isUserExists")]
		bool IsUserExists(string authToken, string username);

		[XmlRpcMethod("api.getUserOverdraftMode")]
		string GetUserOverdraftMode(string authToken, string username);

		[XmlRpcMethod("api.setUserOverdraftMode")]
		void SetUserOverdraftMode(string authToken, string username, string mode);

		[XmlRpcMethod("api.getUserAccountBalance")]
		double GetUserAccountBalance(string authToken, string username, string accountName);


		[XmlRpcMethod("api.getUserProperty")]
		string GetUserProperty(string authToken, string username, string propertyName);

		[XmlRpcMethod("api.getUserProperties")]
		string[] GetUserProperties(string authToken, string username, string[] propertyNames);

		[XmlRpcMethod("api.setUserProperty")]
		void SetUserProperty(string authToken, string username, string propertyName, string propertyValue);

		[XmlRpcMethod("api.setUserProperties")]
		void SetUserProperties(string authToken, string username, string[][] propertyNamesAndValues);

		[XmlRpcMethod("api.adjustUserAccountBalance")]
		void AdjustUserAccountBalance(string authToken, string username, double adjustment, string comment,
			string accountName);

		[XmlRpcMethod("api.adjustUserAccountBalanceByCardNumber")]
		bool AdjustUserAccountBalanceByCardNumber(string authToken, string cardNumber, double adjustment, string comment);

		[XmlRpcMethod("api.adjustUserAccountBalanceByCardNumber")]
		bool AdjustUserAccountBalanceByCardNumber(string authToken, string cardNumber, double adjustment, string comment, string accountName);

		[XmlRpcMethod("api.adjustUserAccountBalanceIfAvailable")]
		bool AdjustUserAccountBalanceIfAvailable(string authToken, string username, double adjustment, string comment, string accountName);

		[XmlRpcMethod("api.adjustUserAccountBalanceIfAvailableLeaveRemaining")]
		bool AdjustUserAccountBalanceIfAvailableLeaveRemaining(string authToken, string username, double adjustment,
															   double leaveRemaining, string comment, string accountName);

		[XmlRpcMethod("api.adjustUserAccountBalanceByGroup")]
		void AdjustUserAccountBalanceByGroup(string authToken, string group, double adjustment, string comment, string accountName);

		[XmlRpcMethod("api.adjustUserAccountBalanceByGroupUpTo")]
		void AdjustUserAccountBalanceByGroup(string authToken, string group, double adjustment, double limit,
											 string comment, string accountName);

		[XmlRpcMethod("api.setUserAccountBalance")]
		void SetUserAccountBalance(string authToken, string username, double balance, string comment, string accountName);

		[XmlRpcMethod("api.setUserAccountBalanceByGroup")]
		void SetUserAccountBalanceByGroup(string authToken, string group, double balance, string comment, string accountName);

		[XmlRpcMethod("api.resetUserCounts")]
		void ResetUserCounts(string authToken, string username, string resetBy);

		[XmlRpcMethod("api.reapplyInitialUserSettings")]
		void ReapplyInitialUserSettings(string authToken, string username);

		[XmlRpcMethod("api.disablePrintingForUser")]
		void DisablePrintingForUser(string authToken, string username, int disableMins);

		[XmlRpcMethod("api.addNewUser")]
		void AddNewUser(string authToken, string username);

		[XmlRpcMethod("api.renameUserAccount")]
		void RenameUserAccount(string authToken, string currentUserName, string newUserName);

		[XmlRpcMethod("api.deleteExistingUser")]
		void DeleteExistingUser(string authToken, string username, bool redactUserData = false);

		[XmlRpcMethod("api.addNewInternalUser")]
		void AddNewInternalUser(string authToken, string username, string password, string fullName, string email, string cardId, string pin, bool sendEmail = false);

		[XmlRpcMethod("api.exportUserDataHistory")]
		void ExportUserDataHistory(string authToken, string username, string saveLocation);

		[XmlRpcMethod("api.lookUpUserNameByIDNo")]
		string LookUpUserNameByIDNo(string authToken, string idNo);

		[XmlRpcMethod("api.lookUpUserNameByCardNo")]
		string LookUpUserNameByCardNo(string authToken, string cardNo);


		[XmlRpcMethod("api.lookUpUserNameByEmail")]
		string LookUpUserNameByEmail(string authToken, string email);

		[XmlRpcMethod("api.lookUpUserNameBySecondaryUserName")]
		string LookUpUserNameBySecondaryUserName(string authToken, string secondaryUserName);

		[XmlRpcMethod("api.lookUpUsersByFullName")]
		string[] LookUpUsersByFullName(string authToken, string fullName);

		[XmlRpcMethod("api.addUserToGroup")]
		void AddUserToGroup(string authToken, string username, string groupName);

		[XmlRpcMethod("api.removeUserFromGroup")]
		void RemoveUserFromGroup(string authToken, string username, string groupName);

		[XmlRpcMethod("api.addAdminAccessUser")]
		void AddAdminAccessUser(string authToken, string username);

		[XmlRpcMethod("api.removeAdminAccessUser")]
		void RemoveAdminAccessUser(string authToken, string username);

		[XmlRpcMethod("api.addAdminAccessGroup")]
		void AddAdminAccessGroup(string authToken, string groupName);

		[XmlRpcMethod("api.removeAdminAccessGroup")]
		void RemoveAdminAccessGroup(string authToken, string groupName);

		[XmlRpcMethod("api.setUserAccountSelectionAutoSelectSharedAccount")]
		void SetUserAccountSelectionAutoSelectSharedAccount(string authToken, string username, string accountName,
																bool chargeToPersonal);

		[XmlRpcMethod("api.setUserAccountSelectionAutoChargePersonal")]
		void SetUserAccountSelectionAutoChargePersonal(string authToken, string username, bool withPopupConfirmation);

		[XmlRpcMethod("api.setUserAccountSelectionStandardPopup")]
		void SetUserAccountSelectionStandardPopup(string authToken, string username, bool allowPersonal,
												  bool allowListSelection, bool allowPinCode,
												  bool allowPrintingAsOtherUser,
												  bool chargeToPersonalWhenSharedSelected);

		[XmlRpcMethod("api.setUserAccountSelectionStandardPopup")]
		void SetUserAccountSelectionStandardPopup(string authToken, string username, bool allowPersonal,
												  bool allowListSelection, bool allowPinCode,
												  bool allowPrintingAsOtherUser,
												  bool chargeToPersonalWhenSharedSelected,
												  string defaultSharedAccount);


		[XmlRpcMethod("api.setUserAccountSelectionAdvancedPopup")]
		void SetUserAccountSelectionAdvancedPopup(string authToken, string userName, bool allowPersonal,
				bool chargeToPersonalWhenSharedSelected, string defaultSharedAccount);

		[XmlRpcMethod("api.listUserAccounts")]
		string[] ListUserAccounts(string authToken, int offset, int limit);

		[XmlRpcMethod("api.listSharedAccounts")]
		string[] ListSharedAccounts(string authToken, int offset, int limit);

		[XmlRpcMethod("api.getTotalUsers")]
		int GetTotalUsers(string authToken);

		[XmlRpcMethod("api.listUserSharedAccounts")]
		string[] ListUserSharedAccounts(string authToken, string username, int offset, int limit, bool ignoreAccountMode);

		[XmlRpcMethod("api.listUserSharedAccounts")]
		string[] ListUserSharedAccounts(string authToken, string username, int offset, int limit);

		[XmlRpcMethod("api.isSharedAccountExists")]
		bool SharedAccountExists(string authToken, string accountName);

		[XmlRpcMethod("api.getSharedAccountAccountBalance")]
		double GetSharedAccountAccountBalance(string authToken, string sharedAccountName);

		[XmlRpcMethod("api.getSharedAccountOverdraftMode")]
		string GetSharedAccountOverdraftMode(string authToken, string sharedAccountName);

		[XmlRpcMethod("api.setSharedAccountOverdraftMode")]
		void SetSharedAccountOverdraftMode(string authToken, string sharedAccountName, string mode);

		[XmlRpcMethod("api.disableSharedAccount")]
		void DisableSharedAccount(string authToken, string accountName, int disableMins);

		[XmlRpcMethod("api.getSharedAccountProperty")]
		string GetSharedAccountProperty(string authToken, string sharedAccountName, string propertyName);

		[XmlRpcMethod("api.getSharedAccountProperties")]
		string[] GetSharedAccountProperties(string authToken, string sharedAccountName, string[] propertyNames);

		[XmlRpcMethod("api.setSharedAccountProperty")]
		void SetSharedAccountProperty(string authToken, string sharedAccountName, string propertyName,
									  string propertyValue);

		[XmlRpcMethod("api.setSharedAccountProperties")]
		void SetSharedAccountProperties(string authToken, string sharedAccountName, string[][] propertyNamesAndValues);

		[XmlRpcMethod("api.adjustSharedAccountAccountBalance")]
		void AdjustSharedAccountAccountBalance(string authToken, string accountName, double adjustment, string comment);

		[XmlRpcMethod("api.setSharedAccountAccountBalance")]
		void SetSharedAccountAccountBalance(string authToken, string accountName, double balance, string comment);

		[XmlRpcMethod("api.addNewSharedAccount")]
		void AddNewSharedAccount(string authToken, string sharedAccountName);

		[XmlRpcMethod("api.renameSharedAccount")]
		void RenameSharedAccount(string authToken, string currentSharedAccountName, string newSharedAccountName);

		[XmlRpcMethod("api.deleteExistingSharedAccount")]
		void DeleteExistingSharedAccount(string authToken, string sharedAccountName);

		[XmlRpcMethod("api.addSharedAccountAccessUser")]
		void AddSharedAccountAccessUser(string authToken, string sharedAccountName, string username);

		[XmlRpcMethod("api.addSharedAccountAccessGroup")]
		void AddSharedAccountAccessGroup(string authToken, string sharedAccountName, string groupName);

		[XmlRpcMethod("api.removeSharedAccountAccessUser")]
		void RemoveSharedAccountAccessUser(string authToken, string sharedAccountName, string username);

		[XmlRpcMethod("api.removeSharedAccountAccessGroup")]
		void RemoveSharedAccountAccessGroup(string authToken, string sharedAccountName, string groupName);

		[XmlRpcMethod("api.getPrinterProperty")]
		string GetPrinterProperty(string authToken, string serverName, string printerName, string propertyName);

		[XmlRpcMethod("api.getPrinterProperties")]
		string[] GetPrinterProperties(string authToken, string serverName, string printerName, string[] propertyNames);

		[XmlRpcMethod("api.setPrinterProperty")]
		void SetPrinterProperty(string authToken, string serverName, string printerName, string propertyName,
								string propertyValue);

		[XmlRpcMethod("api.setPrinterProperties")]
		bool SetPrinterProperties(string authToken, string serverName, string printerName, string[,] propertyNamesAndValues);

		[XmlRpcMethod("api.applyDeviceSettings")]
		bool ApplyDeviceSettings(string authToken, string deviceName);

		[XmlRpcMethod("api.listPrinters")]
		string[] ListPrinters(string authToken, int offset, int limit);

		[XmlRpcMethod("api.resetPrinterCounts")]
		void ResetPrinterCounts(string authToken, string serverName, string printerName, string resetBy);

		[XmlRpcMethod("api.addPrinterGroup")]
		void AddPrinterGroup(string authToken, string serverName, string printerName, string printerGroupName);

		[XmlRpcMethod("api.setPrinterGroups")]
		void SetPrinterGroups(string authToken, string serverName, string printerName, string printerGroupNames);

		[XmlRpcMethod("api.disablePrinter")]
		void DisablePrinter(string authToken, string serverName, string printerName, int disableMins);

		[XmlRpcMethod("api.deletePrinter")]
		void DeletePrinter(string authToken, string serverName, string printerName);

		[XmlRpcMethod("api.renamePrinter")]
		void RenamePrinter(string authToken, string serverName, string printerName, string newServerName,
						   string newPrinterName);

		[XmlRpcMethod("api.addPrinterAccessGroup")]
		void AddPrinterAccessGroup(string authToken, string serverName, string printerName, string groupName);

		[XmlRpcMethod("api.removePrinterAccessGroup")]
		void RemovePrinterAccessGroup(string authToken, string serverName, string printerName, string groupName);

		[XmlRpcMethod("api.setPrinterCostSimple")]
		void SetPrinterCostSimple(string authToken, string serverName, string printerName, double costPerPage);

		[XmlRpcMethod("api.getPrinterCostSimple")]
		double GetPrinterCostSimple(string authToken, string serverName, string printerName);

		[XmlRpcMethod("api.addNewGroup")]
		void AddNewGroup(string authToken, string groupName);

		[XmlRpcMethod("api.syncGroup")]
		bool SyncGroup(string authToken, string groupName);

		[XmlRpcMethod("api.removeGroup")]
		void RemoveGroup(string authToken, string groupName);

		[XmlRpcMethod("api.listUserGroups")]
		string[] ListUserGroups(string authToken, int offset, int limit);

		[XmlRpcMethod("api.getUserGroups")]
		string[] GetUserGroups(string authToken, string userName);

		[XmlRpcMethod("api.getGroupMembers")]
		string[] GetGroupMembers(string authToken, string groupName, int offset, int batchSize);

		[XmlRpcMethod("api.isGroupExists")]
		bool GroupExists(string authToken, string groupName);

		[XmlRpcMethod("api.setGroupQuota")]
		void SetGroupQuota(string authToken, string groupName, double quotaAmount, string period,
						   double quotaMaxAccumulation);

		[XmlRpcMethod("api.getGroupQuota")]
		GetGroupQuotaResponse GetGroupQuota(string authToken, string groupName);

		[XmlRpcMethod("api.useCard")]
		string UseCard(string authToken, string username, string cardNumber);

		[XmlRpcMethod("api.getTaskStatus")]
		GetTaskStatusResponse GetTaskStatus();

		[XmlRpcMethod("api.performOnlineBackup")]
		void PerformOnlineBackup(string authToken);

		[XmlRpcMethod("api.performGroupSync")]
		void PerformGroupSync(string authToken);

		[XmlRpcMethod("api.performUserAndGroupSync")]
		void PerformUserAndGroupSync(string authToken);

		[XmlRpcMethod("api.performUserAndGroupSyncAdvanced")]
		void PerformUserAndGroupSyncAdvanced(string authToken, bool deleteNonExistentUsers, bool updateUserDetails);

		[XmlRpcMethod("api.addNewUsers")]
		void AddNewUsers(string authToken);

		[XmlRpcMethod("api.batchImportSharedAccounts")]
		string BatchImportSharedAccounts(string authToken, string importFile, bool test, bool deleteNonExistentAccounts);

		[XmlRpcMethod("api.batchImportUsers")]
		void BatchImportUsers(string authToken, string importFile, bool createNewUsers);

		[XmlRpcMethod("api.batchImportInternalUsers")]
		void BatchImportInternalUsers(string authToken, string importFile, bool overwriteExistingPasswords,
									  bool overwriteExistingPINs);

		[XmlRpcMethod("api.batchImportUserCardIdNumbers")]
		void BatchImportUserCardIdNumbers(string authToken, string importFile, bool overwriteExistingPINs);

		[XmlRpcMethod("api.getConfigValue")]
		string GetConfigValue(string authToken, string configName);

		[XmlRpcMethod("api.setConfigValue")]
		void SetConfigValue(string authToken, string configName, string configValue);

		[XmlRpcMethod("api.processJob")]
		void ProcessJob(string authToken, string jobDetails);

		[XmlRpcMethod("api.changeInternalAdminPassword")]
		bool ChangeInternalAdminPassword(string authToken, string newPassword);

		[XmlRpcMethod("api.changeInternalAdminPassword")]
		bool GenerateScheduledReport(string authToken, string reportTitle, string saveLocation);

		[XmlRpcMethod("api.generateAdHocReport")]
		bool GenerateAdHocReport(string authToken, string reportType, string dataParams, string exportTypeExt, string reportTitle, string saveLocation);
	}
}
