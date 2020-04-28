using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Helper
{
    public class FileManager
    {
        private CloudBlobContainer blobContainer;
        public FileManager()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=training2blobstorage;AccountKey=d4swk28SyaP0RkzEssi2Q39Zsg6+rFQ8bv/UvzYfgwHXIqbuYjY1yrUWXXoH50YdTuipEZhnwQ+JS93TMNFRCg==;EndpointSuffix=core.windows.net";           
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            string containerName = "trainingblobcontainer";
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = cloudBlobClient.GetContainerReference(containerName);
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

        public string Upload(HttpPostedFileBase file)
        {        
            string FileName = Path.GetFileName(file.FileName);
            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(FileName);
            cloudBlockBlob.Properties.ContentType = file.ContentType;
            cloudBlockBlob.UploadFromStream(file.InputStream);
            return  cloudBlockBlob.Uri.AbsoluteUri.ToString();          
        }

        public bool Download(string file)
        {
            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(file);
            var filePath = @"E:\Gateway\Azure\" + file;
            cloudBlockBlob.DownloadToFile(filePath, FileMode.OpenOrCreate);
            return true; 
        }

        public bool Delete(string file)
        {
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(file);
            blockBlob.Delete();
            return true; 
        }
    }
}