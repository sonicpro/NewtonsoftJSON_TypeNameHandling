using System;
using System.Web.Mvc;
//using Corrigo.Constants;
using Corrigo.Web.Infrastructure.MVC;
using Newtonsoft.Json;

namespace Corrigo.Web.CorpNet.Areas.WorkOrder.Services.WoWizard.BizObjectModels
{
	// Corresponds to Corrigo.BO.WorkOrder.WorkOrder2
	[ModelBinder(typeof(JsonModelBinder))]
	public class WoWizardWorkOrderModel : IHasModelId
	{
		//public WoWizardWorkOrderModel()
		//{
		//	Items = new List<WoWizardWoItemModel>();
		//}

		[JsonProperty("modelId")]
		public int ModelId { get; set; }

		//[JsonProperty("woTypeId")]
		//public WOType WoTypeId { get; set; }

		//[JsonProperty("scanTypeId")]
		//public ScanType ScanTypeId { get; set; }

		//[JsonProperty("workOrderCost")]
		//public WoWizardWorkOrderCostModel WorkOrderCost { get; set; }

		//[JsonProperty("priority")]
		//public WoWizardWoPriorityLookupModel Priority { get; set; }

		[JsonProperty("creatorId")]
		public int CreatorId { get; set; }

		//[JsonProperty("pteType")]
		//public WoWizardPteTypeLookupModel PteType { get; set; }

		//[JsonProperty("asset")]
		//public WoWizardInvItemModel Asset { get; set; }

		[JsonProperty("assetLocation")]
		public string AssetLocation { get; set; }

		//[JsonProperty("community")]
		//public WoWizardCommunityModel Community { get; set; }

		//[JsonProperty("employee")]
		//public WoWizardNhEmployeeModel Employee { get; set; }

		[JsonProperty("duration")]
		public int Duration { get; set; }

		[JsonProperty("billbackAmount")]
		public decimal BillbackAmount { get; set; }

        [JsonProperty("customerNTE")]
        public decimal CustomerNTE { get; set; }

		//[JsonProperty("specialty")]
		//public WoWizardSpecialtyModel Specialty { get; set; }

		[JsonProperty("subTypeId")]
		public int SubTypeId { get; set; }

		//[JsonProperty("lease")]
		//public WoWizardLeaseModel Lease { get; set; }

		//[JsonProperty("location")]
		//public WoWizardInvItemModel Location { get; set; }

		[JsonProperty("creatorRefinement")]
		public string CreatorRefinement { get; set; }

		[JsonProperty("wonIsPossibleCoverWarranty")]
		public bool WonIsPossibleCoverWarranty { get; set; }

		[JsonProperty("timeZone")]
		public int TimeZone { get; set; }

		[JsonProperty("dtUtcDue")]
		public DateTime? DtUtcDue { get; set; }

        [JsonProperty("dtUtcOnSiteBy")]
        public DateTime? DtUtcOnSiteBy { get; set; }

        [JsonProperty("dtUtcAckBy")]
        public DateTime? DtUtcAckBy { get; set; }

        [JsonProperty("dtUtcCreated")]
		public DateTime? DtUtcCreated { get; set; }

		[JsonProperty("dtUtcScheduledStart")]
		public DateTime? DtUtcScheduledStart { get; set; }

		//[JsonProperty("items")]
		//public IList<WoWizardWoItemModel> Items { get; set; }

		//[JsonProperty("contactAddresses")]
		//public IList<WoWizardCAddrModel> ContactAddresses { get; set; }

		//[JsonProperty("assigments")]
		//public IList<WoWizardWoAssignmentModel> Assigments { get; set; }
	}
}
