using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Linq;
using System.Web;
using System.IO;


namespace TableCrud.Configuration
{
    public class BlobManager
    {
        private CloudBlobContainer blobContainer;

       
        public BlobManager(string ContainerName)
        {
            if (string.IsNullOrEmpty(ContainerName))
            {
                throw new ArgumentNullException("ContainerName", "Container Name can't be empty");
            }
            try
            {
                string secretName = "StorageConnectionString";
                string kvUri = "https://training1vault.vault.azure.net/secrets/";
                //string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=training2blobstorage;AccountKey=d4swk28SyaP0RkzEssi2Q39Zsg6+rFQ8bv/UvzYfgwHXIqbuYjY1yrUWXXoH50YdTuipEZhnwQ+JS93TMNFRCg==;EndpointSuffix=core.windows.net";
                string ConnectionString = KeyVaultManager.GetValueFromAzureVault(kvUri, secretName);
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = cloudBlobClient.GetContainerReference(ContainerName);
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
            /*if (FileToUpload == null || FileToUpload.ContentLength == 0)
                return null;*/
            try
            {
                string FileName = Path.GetFileName(FileToUpload.FileName);
                CloudBlockBlob blockBlob;
                blockBlob = blobContainer.GetBlockBlobReference(FileName);
                blockBlob.Properties.ContentType = FileToUpload.ContentType;
                blockBlob.UploadFromStream(FileToUpload.InputStream);
                AbsoluteUri = blockBlob.Uri.AbsoluteUri;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
            return AbsoluteUri;
        }
        public CloudBlockBlob Download(string name)
        {
            name = HttpUtility.UrlDecode(name);
            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(name);
            return cloudBlockBlob;
        }
    }
}