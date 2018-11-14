using System;

namespace Corrigo.Web.Infrastructure.JsonHelpers
{
	public class SameStringDateTimeConverter : BaseDateTimeConverter
	{
		protected override string FormatDateTime(DateTime dateTime)
		{
			return FormatDateTimeSameString(dateTime);
		}

		protected override DateTime GetDateTime(DateTimeOffset dateTimeOffset)
		{
			var dateTime = new DateTime(dateTimeOffset.DateTime.Ticks - (long)dateTimeOffset.Offset.TotalMilliseconds * TicksInMillis, DateTimeKind.Unspecified);
			return dateTime;
		}
	}
}
