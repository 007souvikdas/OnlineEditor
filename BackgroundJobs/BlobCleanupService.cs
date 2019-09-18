using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class BlobCleanupService : BackgroundService
{
    CloudBlobClient blobClient;
    int cleanupTime;
    public BlobCleanupService(IConfiguration configuration)
    {
        string myConnectionString = configuration["StorageConnectionString"];
        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(myConnectionString);
        blobClient = cloudStorageAccount.CreateCloudBlobClient();
        cleanupTime = Int32.Parse(configuration["CleanupTime"]);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //delete all unwanted/old blobs
            foreach (var container in blobClient.ListContainers())
            {
                var res = DateTime.UtcNow - container.Properties.LastModified.Value.DateTime;
                if (res.Hours > cleanupTime)//if time period exceeds configured hour, then we will delete the container
                {
                    container.Delete();
                }
            }
            await Task.Delay(5 * 60 * 1000);
        }
    }
}