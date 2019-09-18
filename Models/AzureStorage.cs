using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using OnlineEditor.Models;

public class AzureStorage : IFileStorage
{
    CloudBlobContainer container;
    CloudBlobClient blobClient;
    public AzureStorage(IConfiguration configuration)
    {
        string myConnectionString =configuration["StorageConnectionString"];
        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(myConnectionString);
        blobClient = cloudStorageAccount.CreateCloudBlobClient();
    }
    public bool AddFile(string path, string name)
    {
        container = blobClient.GetContainerReference(path);
        if (container.Exists() || container.CreateIfNotExists())
        {
            var blob = container.GetBlockBlobReference(name);
            blob.UploadText(string.Empty);
            return true;
        }
        else
        {
            return false;
        }
    }

    public SourceCode GetFileContents(string path, string name)
    {
        container = blobClient.GetContainerReference(path);
        var blob = container.GetBlockBlobReference(name);
        if (container.Exists() && blob.Exists())
        {
            SourceCode sourceCode = new SourceCode();
            sourceCode.FileName = name;
            using (StreamReader reader = new StreamReader(blob.OpenRead()))
            {
                sourceCode.sourceContent = reader.ReadToEnd();
            }
            return sourceCode;
        }
        else
        {
            return new SourceCode();
        }
    }

    public string[] GetFileNames(string path)
    {
        container = blobClient.GetContainerReference(path);
        List<string> fileNames = new List<string>();
        if (container.Exists())
        {
            foreach (var blobItem in container.ListBlobs())
            {
                var cloudBlockBlob = blobItem as CloudBlockBlob;
                if (cloudBlockBlob != null)
                {
                    fileNames.Add(cloudBlockBlob.Name);
                }
            }
        }
        return fileNames.ToArray();
    }

    public bool RemoveFile(string path, string name)
    {
        container = blobClient.GetContainerReference(path);
        var blob = container.GetBlockBlobReference(name);
        if (container.Exists() && blob.Exists())
        {
            blob.DeleteIfExists();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SaveFile(string path, string name, string sourceCode)
    {
        container = blobClient.GetContainerReference(path);
        var blob = container.GetBlockBlobReference(name);

        if (container.Exists() && blob.Exists())
        {
            blob.UploadText(sourceCode);
            return true;
        }
        else
        {
            return false;
        }
    }
}