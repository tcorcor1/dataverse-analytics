using System.Threading.Tasks;
using System.Collections.Generic;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Monitor.Query;

namespace DataverseAnalytics.Common
{
	public class AzureAppInsightsService
	{
		private AzureAppInsightsServiceConfig _config;
		private string[] _pageViewColumns => new string[] { "name", "timestamp", "duration", "performanceBucket", "itemType", "operation_Name", "operation_Id", "operation_ParentId", "user_Id", "user_AuthenticatedId", "application_Version", "client_Type", "client_IP", "client_City", "client_CountryOrRegion", "appId", "appName", "itemId", "itemCount" };

		public LogsQueryClient LogsQueryClient { get; private set; }

		public AzureAppInsightsService (AzureAppInsightsServiceConfig azureAppInsightsServiceConfig)
		{
			_config = azureAppInsightsServiceConfig;

			LogsQueryClient = new LogsQueryClient(new ClientSecretCredential(_config.AzureTenantId, _config.AzureClientId, _config.AzureClientSecret));
		}

		public async Task<Response<IReadOnlyList<T>>> QueryAsync<T> (string query, QueryTimeRange timeRange)
		{
			return await LogsQueryClient.QueryResourceAsync<T>(
				new ResourceIdentifier(_config.AzureAppInsightsResource),
				query,
				timeRange);
		}

		public async Task<Response<IReadOnlyList<PageView>>> QueryPageViewsAsync (QueryTimeRange timeRange)
		{
			return await LogsQueryClient.QueryResourceAsync<PageView>(
				new ResourceIdentifier(_config.AzureAppInsightsResource),
				$"pageViews | project {string.Join(",", _pageViewColumns)} | order by timestamp desc",
				timeRange);
		}
	}
}