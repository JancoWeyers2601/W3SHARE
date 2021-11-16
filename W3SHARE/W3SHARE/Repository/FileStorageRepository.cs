using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace W3SHARE.Repository
{
    public class FileStorageRepository
    {
        //TODO: UPDATE CONNECTION STRING TO READ FROM appsettings.json
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

        //TODO//:fix downloads https://tutexchange.com/uploading-download-and-delete-files-in-azure-blob-storage-using-asp-net-core-3-1/

        public bool DownloadFileFromBlob (string fileName)
        {
            var downloadPath = @"C:\Users\janco\Downloads\ff846d75-28e5-4303-ac0d-9bb74ddb28a2.png";
            //var downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + fileName);

            try
            {
                var content = new BlobClient(connectionString, container, fileName).DownloadTo(downloadPath);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }


    }
}
