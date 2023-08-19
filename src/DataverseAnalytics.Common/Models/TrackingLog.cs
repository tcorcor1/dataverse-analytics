using System;
using System.Text.Json.Serialization;

namespace DataverseAnalytics.Common
{
	public class TrackingLog
	{
		[JsonPropertyName("lastSyncDate")]
		public DateTime LastSyncDate { get; set; }

		[JsonPropertyName("isSuccess")]
		public bool IsSuccess { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }

		public TrackingLog (DateTime lastSyncDate, bool isSuccess, string message)
		{
			LastSyncDate = lastSyncDate;
			IsSuccess = isSuccess;
			Message = message;
		}
	}
}