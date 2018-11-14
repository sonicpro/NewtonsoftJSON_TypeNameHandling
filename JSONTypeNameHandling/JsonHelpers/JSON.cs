using System;
using Newtonsoft.Json;

namespace Corrigo.Web.Infrastructure.JsonHelpers
{
	// ReSharper disable once InconsistentNaming
	public static class JSON
	{
		private static readonly JsonSerializerSettings SerializeSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.None,
			Formatting = Formatting.None,
			Converters = new JsonConverter[] { new SameStringDateTimeConverter() }
		};

		private static readonly JsonSerializerSettings DeserializeSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Auto,
			Converters = new JsonConverter[] { new SameStringDateTimeConverter() }
		};

		public static string Serialize(object value)
		{
			return JsonConvert.SerializeObject(value, SerializeSettings);
		}

		public static T Deserialize<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value, DeserializeSettings);
		}

		public static object Deserialize(Type type, string value)
		{
			return JsonConvert.DeserializeObject(value, type, DeserializeSettings);
		}
	}
}