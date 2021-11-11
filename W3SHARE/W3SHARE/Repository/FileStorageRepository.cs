using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace W3SHARE.Repository
{
    public class FileStorageRepository
    {
        // TO DO: Add Connection String and move into secure storage
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=w3share;AccountKey=bqNgzLtSaSMLeEqS9KUK9Q/YB8bYL8AQ2q/jC7KDZzBHtvZdSCBZRDHnW4x/TXHXbA6qcl1RF7XDH/ZIE0Rkwg==;EndpointSuffix=core.windows.net";
        string container = "data";

        public string ReadFileFromBlob(string fileName)
        {
            // Get blob
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, container);
            var blob = blobContainerClient.GetBlobClient(fileName);

            string filePath = @$"C:\Users\janco\Desktop\New folder\W3SHARE\W3SHARE\wwwroot\Images\Temp\{fileName}";

            // Download blob
            blob.DownloadTo(filePath);

            //using (StreamReader r = new StreamReader(filePath))
            //{
            //    string content = r.ReadToEnd(); // I don't know what you want to do with this
            //}

            return filePath;
        }

        public string WriteFileToBlob(string filePath,string fileName)
        {
            //INSERT TRY CATCH (HIERDIE EEN WERK)
            var containerClient = new BlobContainerClient(connectionString, container);
            var blobClient = containerClient.GetBlobClient(fileName);
            var fileStream = File.OpenRead(filePath);
            blobClient.Upload(fileStream);

            return blobClient.Uri.ToString();
        }

        public bool DownloadFileFromBlob (string fileName)
        {
            var downloadPath = @"C:\Users\janco\Documents\ff846d75-28e5-4303-ac0d-9bb74ddb28a2.png";
            //var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + fileName);

            try
            {
                var con = new BlobClient(connectionString, container, fileName).DownloadTo(downloadPath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }


    }
}
