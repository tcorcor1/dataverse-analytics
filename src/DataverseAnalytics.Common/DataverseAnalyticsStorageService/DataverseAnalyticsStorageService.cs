using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using CsvHelper;

namespace DataverseAnalytics.Common
{
	public class DataverseAnalyticsStorageService
	{
		private string _connectionString;
		private BlobServiceClient _blobServiceClient;

		public DataverseAnalyticsStorageService (string connectionString)
		{
			_connectionString = connectionString;

			_blobServiceClient = new BlobServiceClient(_connectionString);
		}

		public async Task<TrackingLog> GetTrackingLogAsync (string orgName)
		{
			TrackingLog trackingLog;

			var trackingContainer = _blobServiceClient.GetBlobContainerClient("tracking");
			var blobClient = trackingContainer.GetBlobClient($"/{orgName}/tracking.json");

			using (var trackingBlobReader = await blobClient.OpenReadAsync(true))
			using (var streamReader = new StreamReader(trackingBlobReader))
			{
				var trackingLogSerialized = await streamReader.ReadToEndAsync();
				trackingLog = JsonSerializer.Deserialize<TrackingLog>(trackingLogSerialized);
			}

			return trackingLog;
		}

		public async Task UpdateTrackingLogAsync (TrackingLog newTrackingLog, string orgName)
		{
			var trackingContainer = _blobServiceClient.GetBlobContainerClient("tracking");
			var blobClient = trackingContainer.GetBlobClient($"/{orgName}/tracking.json");

			using (var trackingBlobWriter = await blobClient.OpenWriteAsync(true))
			using (var streamWriter = new StreamWriter(trackingBlobWriter))
			{
				var newTrackingLogSerialized = JsonSerializer.Serialize(newTrackingLog);

				await streamWriter.WriteAsync(newTrackingLogSerialized);
			}
		}

		public async Task WritePageViewsToStorageAsync<T> (string containerName, string blobName, IEnumerable<T> pageViews)
		{
			var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			var blobClient = blobContainerClient.GetBlobClient($"/page_views/{blobName}");

			using (var blobWriter = await blobClient.OpenWriteAsync(true))
			using (var streamWriter = new StreamWriter(blobWriter))
			using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
			{
				csvWriter.WriteRecords(pageViews);
			}
		}
	}
}