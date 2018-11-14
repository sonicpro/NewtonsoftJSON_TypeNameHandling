using System.Web.Mvc;
using Corrigo.Web.Infrastructure.JsonHelpers;

namespace Corrigo.Web.Infrastructure.MVC
{
	public class JsonModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (providerValue == null)
				return null;
			return JSON.Deserialize(bindingContext.ModelType, providerValue.AttemptedValue);
		}
	}
}