
using BlobWebApplication;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Web;

namespace BlobWebApplication
{
    public class BlobManager
    {
        private CloudBlobContainer blobContainer;

        public BlobManager(string ContainerName)
        {
            if (string.IsNullOrEmpty(ContainerName))
            {
                throw new ArgumentNullException(Constants.Container, Constants.ContainerNameError);
            }
            try
            {
                Keymanager keyVaultManager = new Keymanager();
                string ConnectionString = keyVaultManager.GetValueFromAzureVault(Constants.SecretName);
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
            if (FileToUpload == null || FileToUpload.ContentLength == 0)
                return null;
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