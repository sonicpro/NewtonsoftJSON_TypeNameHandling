using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Corrigo.Web.Infrastructure.JsonHelpers
{
	/// <summary>
	/// We use custom converters to pass dates to and from JS. The important things to understand are
	/// 1) Client and server might be in different time zones
	/// 2) Client and server might had different locale settings
	/// 
	/// This class implements most of the logic to serialize and parse back data. More specifically, it
	/// implements all methods to do serialization and parsing but doesn't specify which ones to use.
	/// 
	/// It is imporant to note that default client-to-server and server-to-client formats are different
	/// because in such way it is easier to get required behavior. Particularly server-to-client is expected 
	/// to use either <see cref="FormatDateTimeUtc"/> or <see cref="FormatDateTimeSameString"/> while the 
	/// client alwyas uses fixed format UTC-time + client-time-zone-offset parsed by <see cref="TryParseTicksAndOffset"/> 
	/// (there are no matching serialization as the client doesn't expect such format). In such way server can
	/// parse the same sent value both ways as "same-string" (see further) or as "same-UTC-time" depending on
	/// required logic. This is a good thing because it is much harder to customize JSON-serialization on the
	/// client-side to work differently for different fields than JSON-deserialization on the server-side. 
	/// Still there are some drawbacks of this approach, particularly for compatibility, this class can parse 
	/// inputs in any format. "Compatibility reasons" here mean that we might store some values such as 
	/// GridFilters in a JSON in <see cref="Corrigo.UI.Infrastructure.UIState.IStateManager"/> and thus we 
	/// need to be able to parse back both client-side and server-side values.
	/// See also patched JSON serailization/deserialization in Corrigo.js
	/// 
	/// In most of the cases we want to pass date as "same string" i.e. if server and client are in different
	/// time zones we want times to match not in terms of pointing to the same moment in absolute time (UTC)
	/// but to be the same values in a human-readable form (assuming the same localization settings). In 
	/// other words if the user in New-York enters "6:42 PM" (EST, -4) we want the server (PST, -8) to get 
	/// "6:42 PM" anyway rather than "2:42 PM". Thus <see cref="SameStringDateTimeFormat"/> is the default
	/// converter in our custom <see cref="JSON"/>
	/// 
	/// Currently subclasses specify only:
	/// 1) In <see cref="FormatDateTime"/> - what format to use for serialization as it has to be single format
	/// 2) In <see cref="GetDateTime"/> - using which <see cref="DateTimeKind"/> the parsed value should be represented 
	/// </summary>
	/// <seealso cref="SameStringDateTimeFormat"/>
	/// <seealso cref="UtcDateTimeConverter"/>
	/// <seealso cref="JSON.SerializeSettings"/>
	/// see also ServiceContext.TimeZoneOffsetCookineName in the Corp project
	public abstract class BaseDateTimeConverter : JsonConverter
	{
		private const long InitialJavaScriptDateTicks = 621355968000000000L;
		protected const long TicksInMillis = 10000L;
		private const string DateDecoratorSuffix = ")/";
		private const string DateDecoratorPrefixUtc = "/CorDateUtc(";
		private const string DateDecoratorPrefixTicksAndOffset = "/CorDateTicks(";
		private const string DateDecoratorPrefixSameString = "/CorDateStr(";
		private const string SameStringDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";

		public sealed override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
		}

		public sealed override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var dateTime = ((DateTime)value);
			if (DateTime.MinValue == dateTime)
			{
				writer.WriteNull();
			}
			else
			{
				var strVal = FormatDateTime(dateTime);
				writer.WriteValue(strVal);
			}
		}

		public sealed override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			DateTimeOffset? dateTimeOffset = ReadJsonImpl(reader, objectType, existingValue, serializer);
			// exception if null is not suppported shuold've been thrown by ReadJsonImpl
			if (!dateTimeOffset.HasValue)
				return null;
			else
				return GetDateTime(dateTimeOffset.Value);
		}

		protected DateTimeOffset? ReadJsonImpl(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				if (!IsTypeSupportsNull(objectType))
					return default(DateTimeOffset);
				else
					return null;
			}
			else if (reader.TokenType == JsonToken.Date)
			{
				//Can we be here?
				return (DateTime)reader.Value;
			}
			else if (reader.TokenType == JsonToken.String)
			{
				string str = reader.Value.ToString();
				TryParseDelegate[] allParsers = new[]
				{
					(TryParseDelegate)TryParseTicksAndOffset,
					TryParseSameString,
					TryParseUtc
				};

				foreach (var parser in allParsers)
				{
					DateTimeOffset parsedValue;
					if (parser(reader, str, out parsedValue))
						return parsedValue;
				}
				// no matching parser found
				throw CreateParseException(reader);
			}
			else
			{
				throw CreateParseException(reader);
			}
		}


		#region static helpers

		private static long UniversialTicksToJavaScriptMillis(long universialTicks)
		{
			return (universialTicks - InitialJavaScriptDateTicks) / TicksInMillis;
		}

		private static long JavaScriptMillisToUniversialTicks(long jsMillis)
		{
			return (jsMillis * TicksInMillis + InitialJavaScriptDateTicks);
		}

		private static bool IsTypeSupportsNull(Type t)
		{
			if (!t.IsValueType)
				return true;
			else
				return t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>));
		}

		private static InvalidOperationException CreateParseException(JsonReader reader)
		{
			return new InvalidOperationException("Unexpected token at '" + reader.Path + "' instead of date: "
												 + reader.TokenType + " value = '" + reader.Value + "'");
		}

		private static string ParseDecoratedString(string str, string decoratorPrefix)
		{
			if (!str.StartsWith(decoratorPrefix) || !str.EndsWith(DateDecoratorSuffix))
				return null;

			return str.Substring(decoratorPrefix.Length,
				str.Length - DateDecoratorSuffix.Length - decoratorPrefix.Length);
		}

		#endregion

		#region specific formats

		private delegate bool TryParseDelegate(JsonReader reader, string str, out DateTimeOffset parsedValue);

		protected static bool TryParseSameString(JsonReader reader, string str, out DateTimeOffset parsedValue)
		{
			string val = ParseDecoratedString(str, DateDecoratorPrefixSameString);
			if (val == null)
			{
				parsedValue = DateTimeOffset.MinValue;
				return false;
			}

			var dateTime = DateTime.ParseExact(val, SameStringDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
			parsedValue = new DateTimeOffset(dateTime);
			return true;
		}

		protected static string FormatDateTimeSameString(DateTime dateTime)
		{
			return DateDecoratorPrefixSameString + dateTime.ToString(SameStringDateTimeFormat, CultureInfo.InvariantCulture) + DateDecoratorSuffix;
		}

		protected static bool TryParseTicksAndOffset(JsonReader reader, string str, out DateTimeOffset parsedValue)
		{
			string val = ParseDecoratedString(str, DateDecoratorPrefixTicksAndOffset);
			if (val == null)
			{
				parsedValue = DateTimeOffset.MinValue;
				return false;
			}
			string[] parts = val.Split('|');
			if (parts.Length != 2)
				throw CreateParseException(reader);
			long millis = long.Parse(parts[0]);
			long ticks = JavaScriptMillisToUniversialTicks(millis);
			int shift = int.Parse(parts[1]);
			parsedValue = new DateTimeOffset(ticks, TimeSpan.FromMinutes(shift));
			return true;
		}

		protected static string FormatDateTimeUtc(DateTime dateTime)
		{
			long num = UniversialTicksToJavaScriptMillis(dateTime.Ticks);
			return DateDecoratorPrefixUtc + num + DateDecoratorSuffix;
		}

		protected static bool TryParseUtc(JsonReader reader, string str, out DateTimeOffset parsedValue)
		{
			string val = ParseDecoratedString(str, DateDecoratorPrefixUtc);
			if (val == null)
			{
				parsedValue = DateTimeOffset.MinValue;
				return false;
			}
			long millis = long.Parse(val);
			long ticks = JavaScriptMillisToUniversialTicks(millis);
			parsedValue = new DateTimeOffset(ticks, TimeSpan.FromMinutes(0));
			return true;
		}

		#endregion


		/// <summary>
		/// In this method you are expected to specify serialization format by calling one of existing 
		/// protected methods from the base class:
		/// <see cref="FormatDateTimeSameString"/>
		/// <see cref="FormatDateTimeUtc"/>
		/// </summary>
		protected abstract string FormatDateTime(DateTime dateTime);

		/// <summary>
		/// In this method you should decide how to use UTC time and offset present in <paramref name="dateTimeOffset"/>
		/// to create output. In other words you should decide what <see cref="DateTimeKind"/> you return and 
		/// create <see cref="DateTime"/> accordingly
		/// </summary>
		/// <param name="dateTimeOffset"></param>
		protected abstract DateTime GetDateTime(DateTimeOffset dateTimeOffset);
	}
}
