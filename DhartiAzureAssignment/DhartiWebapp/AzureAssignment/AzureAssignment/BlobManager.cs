using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AzureAssignment
{
    public class BlobManager
    {
        // private property  
        private CloudBlobContainer blobContainer;

        public BlobManager(string ContainerName)
        {
            // Check if Container Name is null or empty  
            if (string.IsNullOrEmpty(ContainerName))
            {
                throw new ArgumentNullException("ContainerName", "Container Name can't be empty");
            }
            try
            {
                KeyVaultManager keyVaultManager = new KeyVaultManager();
                string ConnectionString = keyVaultManager.GetValueFromAzureVault("DTSecret");
                //Get azure table storage connection string.  
                 //string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=training2blobstorage;AccountKey=d4swk28SyaP0RkzEssi2Q39Zsg6+rFQ8bv/UvzYfgwHXIqbuYjY1yrUWXXoH50YdTuipEZhnwQ+JS93TMNFRCg==;EndpointSuffix=core.windows.net";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = cloudBlobClient.GetContainerReference(ContainerName);

                // Create the container and set the permission  
                if (blobContainer.CreateIfNotExists())
                {
                    blobContainer.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                    );
                }
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        public string UploadFile(HttpPostedFileBase FileToUpload)
        {
            string AbsoluteUri;
            // Check HttpPostedFileBase is null or not  
            if (FileToUpload == null || FileToUpload.ContentLength == 0)
                return null;
            try
            {
                string FileName = Path.GetFileName(FileToUpload.FileName);
                CloudBlockBlob blockBlob;
                // Create a block blob  
                blockBlob = blobContainer.GetBlockBlobReference(FileName);
                // Set the object's content type  
                blockBlob.Properties.ContentType = FileToUpload.ContentType;
                // upload to blob  
                blockBlob.UploadFromStream(FileToUpload.InputStream);

                // get file uri  
                AbsoluteUri = blockBlob.Uri.AbsoluteUri;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
            return AbsoluteUri;
        }

        //public bool Download(string AbsoluteUri)
        //{
        //    try
        //    {
        //        Uri uriObj = new Uri(AbsoluteUri);
        //        string BlobName = Path.GetFileName(uriObj.LocalPath);
        //        //string filetoDownload = "test1.xlsx";
        //        string azure_ContainerName = "trainingblobcontainer";
        //        string storageAccount_connectionString = "DefaultEndpointsProtocol=https;AccountName=training2blobstorage;AccountKey=d4swk28SyaP0RkzEssi2Q39Zsg6+rFQ8bv/UvzYfgwHXIqbuYjY1yrUWXXoH50YdTuipEZhnwQ+JS93TMNFRCg==;EndpointSuffix=core.windows.net";
        //        var filePath = @"D:\Dharti\Azure\" + BlobName;
        //        CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(storageAccount_connectionString);
        //        CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

        //        CloudBlobContainer container = blobClient.GetContainerReference(azure_ContainerName);

        //        CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(BlobName);
        //        //using (var fileStream = System.IO.File.OpenWrite(filePath))
        //        //{
        //        //    cloudBlockBlob.DownloadToStream(fileStream);
        //        //}

        //        cloudBlockBlob.DownloadToFile(filePath, FileMode.OpenOrCreate);

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        public CloudBlockBlob Download(string name)
        {
            name = HttpUtility.UrlDecode(name);
            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(name);
            return cloudBlockBlob;
        }
    }
}