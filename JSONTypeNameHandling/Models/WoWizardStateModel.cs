using System.Web.Mvc;
using Corrigo.Web.CorpNet.Areas.WorkOrder.Services.WoWizard.BizObjectModels;
using Corrigo.Web.Infrastructure.MVC;
using Newtonsoft.Json;

namespace Corrigo.Web.CorpNet.Areas.WorkOrder.Services.WoWizard
{
	[ModelBinder(typeof(JsonModelBinder))]
	public class WoWizardStateModel
	{
		[JsonProperty("workOrder")]
        //This is corrected to show how TypeNameHandling.Auto setting works more clear.
		/* public WoWizardWorkOrderModel WorkOrder { get; set;  } */
        public IHasModelId WorkOrder { get; set; }
	}
}
