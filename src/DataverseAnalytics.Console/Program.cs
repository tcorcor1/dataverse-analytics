using Azure.Monitor.Query;
using DataverseAnalytics.Common;

namespace DataverseAnalytics.Console
{
	internal class Program
	{
		private static async Task Main (string[] args)
		{
			var dataverseAnalyticsStorageService = new DataverseAnalyticsStorageService(Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_CONNECTION_STRING"));

			var azureAppInsightsServiceConfig = new AzureAppInsightsServiceConfig()
			{
				AzureTenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID"),
				AzureClientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID"),
				AzureClientSecret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET"),
				AzureAppInsightsResource = Environment.GetEnvironmentVariable("AZURE_APP_INSIGHTS_RESOURCE")
			};
			var azureAppInsightsService = new AzureAppInsightsService(azureAppInsightsServiceConfig);

			var lastTrackingLog = await dataverseAnalyticsStorageService.GetTrackingLogAsync("tldr-dynamics");

			var yesterday = DateTime.UtcNow.Date.AddDays(-1);
			var queryStartDate = lastTrackingLog.LastSyncDate;

			try
			{
				while ((yesterday - queryStartDate).Days > 0)
				{
					queryStartDate = queryStartDate.AddDays(1);

					var queryTimeRange = new QueryTimeRange(queryStartDate, queryStartDate.AddHours(24));
					var pageViewsResults = await azureAppInsightsService.QueryPageViewsAsync(queryTimeRange);

					var pageViewsTableRows = pageViewsResults.Value;

					if (pageViewsTableRows.Count() == 0) continue;

					var blobFileName = $"{queryStartDate.ToString("yyyyMMdd")}.csv";
					await dataverseAnalyticsStorageService.WritePageViewsToStorageAsync("tldr-dynamics-analytics", blobFileName, pageViewsTableRows);
				}

				var successTrackingLog = new TrackingLog(queryStartDate, true, "success");
				await dataverseAnalyticsStorageService.UpdateTrackingLogAsync(successTrackingLog, "tldr-dynamics");
			}
			catch (Exception ex)
			{
				var lastSuccessfulSyncDate = queryStartDate.AddDays(-1);

				var failedTrackingLog = new TrackingLog(lastSuccessfulSyncDate, false, ex.Message);
				await dataverseAnalyticsStorageService.UpdateTrackingLogAsync(failedTrackingLog, "tldr-dynamics");
			}
		}
	}
}