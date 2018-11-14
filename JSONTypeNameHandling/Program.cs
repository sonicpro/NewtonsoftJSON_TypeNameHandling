using System;
using System.IO;
using Corrigo.Core;
using Newtonsoft.Json;
using Corrigo.Web.CorpNet.Areas.WorkOrder.Services.WoWizard;
using Corrigo.Web.CorpNet.Areas.WorkOrder.Services.WoWizard.BizObjectModels;
using Corrigo.Web.Infrastructure.JsonHelpers;
using static Corrigo.Web.Infrastructure.JsonHelpers.JSON;

namespace JSONTypeNameHandling
{
	class Program
	{
        static readonly string FileName = "WOSerialized.json";
        static readonly string FileNameJSONTyped = "WOSerializedWithType.json";

        static void Main(string[] args)
		{
            var wo = new WoWizardWorkOrderModel
            {
                ModelId = 1,
                DtUtcCreated = DateTime.UtcNow,
                DtUtcDue = DateTime.UtcNow + TimeSpan.FromDays(1),
                AssetLocation = "Knarkiv",
                CustomerNTE = 9999.99m
            };
	        var wizardState = new LocalizationContext(LocalizationLocale.EnglishCanada,
				LocalizationLocale.EnglishCanada,
				LocalizationLocale.EnglishCanada); //new WoWizardStateModel { WorkOrder = wo };

            using (var sw = new StreamWriter(FileName))
            {
                sw.Write(Serialize(wizardState));
            }

            using (var sr = new StreamReader(FileName))
            {
                string wizardData = sr.ReadToEnd();

                // This won't work due to the type mismatch and lack of $type property in the serialized data.
                // The error is "Could not create an instance of type Corrigo.Web.CorpNet.Areas.WorkOrder.Services.WoWizard.BizObjectModels.IHasModelId. Type is an interface or abstract class..."
                /* var wizardStateRead = Deserialize(typeof(WoWizardStateModel), wizardData); */
            }

            JsonSerializerSettings sett = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto, // Here is what we have changed.
                Formatting = Formatting.None,
                Converters = new JsonConverter[] { new SameStringDateTimeConverter() }
            };

            using (var sw = new StreamWriter(FileNameJSONTyped))
            {
                sw.Write(JsonConvert.SerializeObject(wizardState, sett));
            }

            // Now it works:
            using (var sr = new StreamReader(FileNameJSONTyped))
            {
                string wizardData = sr.ReadToEnd();
                var wizardStateRead = Deserialize(typeof(WoWizardStateModel), wizardData);
            }
        }
	}
}
