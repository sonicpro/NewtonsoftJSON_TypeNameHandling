<Query Kind="Program">
  <Reference Relative="Newtonsoft.Json-9.0.1\Src\Newtonsoft.Json\bin\Debug\Net45\Newtonsoft.Json.dll">D:\MOE\SourcesCS\NewtonsoftJSON_TypeNameHandling\Newtonsoft.Json-9.0.1\Src\Newtonsoft.Json\bin\Debug\Net45\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Dynamic</Namespace>
</Query>

void Main()
{
	// Demonstrate deserialization from a raw string
	string json2 = "[{\"JobId\":1,\"ModuleName\":\"Corrigo.QueuedJobs.Module.ImportExport\",\"JobTypeName\":\"AdminImportJob\",\"Origin\":14,\"InformationInJson\":\"\",\"StatusId\":3,\"StatusLiteral\":\"Succeeded\",\"UtcQueued\":\"2018-03-15T16:54:40.62\",\"UtcStarted\":\"2018-03-15T16:54:43.44\",\"UtcCompleted\":\"2018-03-15T16:55:01.59\",\"Result\":{\"Plain\":{\"ImportId\":6366,\"TotalRecords\":1},\"ErrorMessage\":null,\"IsErrorMessageLocalized\":false,\"Url\":null,\"FileName\":\"\",\"ContentType\":\"\",\"Exception\":null},\"Progress\":{\"TotalCount\":1,\"ProcessedCount\":1,\"ErrorCount\":1,\"Json\":\"\"}}]";

//	Console.WriteLine(json2);

	// make sure we can serialize back
	var album2 = JsonConvert.DeserializeObject<List<MyImportExportsRecordModel>>(json2);
	album2.Dump();
//	Console.WriteLine(album2[0].StatusId == QueuedJobStatus.Succeeded);
}

// Define other methods and classes here
[JsonObject]
public class MyImportExportsRecordModel
{
	private static string[] _statusLocalization =
		{
				"Unknown",	// Added to get 1-based indexes for the real statuses.
				"CommonResources.MyImportExportsGridService_Column_Status_Pending",
				"CommonResources.MyImportExportsGridService_Column_Status_Processing",
				"CommonResources.MyImportExportsGridService_Column_Status_Succeeded",
				"CommonResources.MyImportExportsGridService_Column_Status_Failed",
				"CommonResources.MyImportExportsGridService_Column_Status_ExpectedlyFailed",
				"CommonResources.MyImportExportsGridService_Column_Status_Cancelled",
				"CommonResources.MyImportExportsGridService_Column_Status_Hanged"
			};

	public int JobId { get; set; }

	public string ModuleName { get; set; }

	public JobResultModel Result { get; set; }

	public DateTime UtcQueued { get; set; }

	public DateTime? UtcStarted { get; set; }

	public DateTime? UtcCompleted { get; set; }

	public QueuedJobStatus StatusId { get; set; }

	public int RowsBeingProcessed { get; set; }

	public int RowsTotal { get; set; }

	public JobProgressModel Progress { get; set; }

	public int CountOfErrors { get; set; }

	public string StatusLiteral { get; set; }

	public string Status => _statusLocalization[(int)StatusId];

	public DateTime Queued { get; set; }

	public DateTime? Started { get; set; }

	public DateTime? Completed { get; set; }

	public string JobTypeName { get; set; }


	public ExportOrigin Origin { get; set; }
}
public enum QueuedJobStatus
{
	Pending = 1,
	Processing = 2,
	Succeeded = 3,
	Failed = 4,
	ExpectedlyFailed = 5,
	Cancelled = 6,
	Hanged = 7
}

[JsonObject]
public class JobResultModel
{
	public string Url { get; set; }

	public ImportResultModel Plain { get; set; }

	public string Filename { get; set; }

	public string ContentType { get; set; }

	public bool IsErrorMessageLocalized{ get; set; }

	public string ErrorMessage { get; set; }

	public string Exception { get; set; }
}

[JsonObject]
public class ImportResultModel
{
	public int ImportId { get; set; } // DiHeader.Id

	public int TotalRecords { get; set; }
}

[JsonObject]
public class JobProgressModel
{
	public int TotalCount { get; set; }

	public int ProcessedCount { get; set; }

	public int ErrorCount { get; set; }

	public string Json { get; set; }
}

[JsonObject]
class FailedJobResultModel
{
	public string ErrorMessage { get; set; }

	public string Details { get; set; }
}

public enum ExportOrigin
{
	KbModel = 19,
	AssetAttributes = 6,
	KbTask = 20,
	WarrantyAsset = 52,
	AssetTemplate = 64,
	Asset = 65,
	AssetSystem = 76,
	Specialty = 14,
	SpecialtyLocalization = 59,
	ProcedureTemplateLocalization = 58,
	Role = 41,
	LnkRoleApStatus = 40,
	Org = 49,
	Employee = 42,
	EmployeeManage = 90,
	Team = 48,
	WarrantyTemplate = 51,
	Provider = 44,
	EmployeePriceListConfiguration = 5,
	License = 9,
	LicenseCoverages = 1,
	LicenseBillAccount = 2,
	LicenseSpecialty = 11,
	Community = 63,
	GroupingPortfolio = 53,
	PrimaryPortfolio = 54,
	Escalation = 43,
	Holiday = 55,
	OnCallList = 57,
	OfficeHours = 56,
	ServiceDispatchRules = 60,
	Approval = 46,
	WoRepairCode = 15,
	PmSchedule = 31,
	WoCreate = 45,
	WoCfUpdate = 47,
	ProcedureField = 93,
	ProcedureFieldsLocalization = 94,
	ProcedureFlagReasonLocalization = 95,
	Customer = 26,
	LeaseContacts = 22,
	LeaseSpaces = 25,
	Contract = 91,
	CustomerGroup = 81,
	BillingAccount = 82,
	CustomerNote = 24,
	SimpleCustomer = 74,
	CpTheme = 35,
	CpThemeLabel = 37,
	CpSettingValue = 36,
	CpSrConfig = 33,
	CpThemeNotification = 34,
	Announcement = 38,
	TimeInterval = 17,
	LaborCode = 27,
	TaxCode = 12,
	InvoiceItem = 10,
	GlAccount = 18,
	DefTax = 29,
	DefTaxItem = 30,
	VendorPriceList = 13,
	CustomerPriceList = 3,
	PriceListSnapshot = 92,
	LnkTimeSetBudget = 32,
	Product = 78,
	Bin = 83,
	StockLocationQuantities = 85,
	MhLocation = 84,
	TransactionItemAdjust = 86,
	TransactionItemReceive = 88,
	TransactionItemReturn = 87,
	TransactionItemTransfer = 89,
	KbTaskLocalization = 23,
	ModelLocalization = 80,
	Insurance = 50,
	CustomFieldMetadata = 16,
	InvoiceItemLocalization = 66,
	LaborCodeLocalization = 67,
	ActionReasonLocalization = 68,
	EmergencyReasonLocalization = 69,
	WoRepairCodeLocalization = 70,
	ModPropLocalization = 61,
	CfDescrLocalization = 62,
	RoleLocalization = 71,
	LeaseContactLocalization = 72,
	EmployeeLocalization = 73,
	InvoiceBulkImport = 75,
	VacationSchedule = 77,
	ProductLocalization = 79,
	ProcedureTemplate = 21,

	// "ExportToExcel" items. Name corresponds to the underlying GridService .NET class name.

	IssueResolutionGridService = -4,
	CustomerContactsGridService = -5,
	OrganizationsGridService = -6,
	EmployeeWorkZonesGridService = -7,
	AssetListGridService = -8,
	CustomerLocationsGridService = -9,
	EmployeesListGridService = -10,
	ManageEmployeesGridService = -11,
	ManageProvidersGridService = -12,
	InventoryListGridService = -13,
	WoFinancialReviewGridService = -14,
	WoFinancialReviewExportGridService = -15,
	WorkorderGridService = -16
}