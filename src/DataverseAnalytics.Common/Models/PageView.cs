using System;
using System.Runtime.Serialization;

namespace DataverseAnalytics.Common
{
	public class PageView
	{
		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "timestamp")]
		public DateTimeOffset Timestamp { get; set; }

		[DataMember(Name = "duration")]
		public int Duration { get; set; }

		[DataMember(Name = "performanceBucket")]
		public string PerformanceBucket { get; set; }

		[DataMember(Name = "itemType")]
		public string ItemType { get; set; }

		[DataMember(Name = "operation_Name")]
		public string OperationName { get; set; }

		[DataMember(Name = "operation_Id")]
		public string OperationId { get; set; }

		[DataMember(Name = "operation_ParentId")]
		public string OperationParentId { get; set; }

		[DataMember(Name = "user_Id")]
		public string UserId { get; set; }

		[DataMember(Name = "user_AuthenticatedId")]
		public string UserAuthenticatedId { get; set; }

		[DataMember(Name = "application_Version")]
		public string ApplicationVersion { get; set; }

		[DataMember(Name = "client_Type")]
		public string ClientType { get; set; }

		[DataMember(Name = "client_IP")]
		public string ClientIP { get; set; }

		[DataMember(Name = "client_City")]
		public string ClientCity { get; set; }

		[DataMember(Name = "client_CountryOrRegion")]
		public string ClientCountryOrRegion { get; set; }

		[DataMember(Name = "appId")]
		public string AppId { get; set; }

		[DataMember(Name = "appName")]
		public string AppName { get; set; }

		[DataMember(Name = "itemId")]
		public string ItemId { get; set; }

		[DataMember(Name = "itemCount")]
		public int ItemCount { get; set; }
	}
}