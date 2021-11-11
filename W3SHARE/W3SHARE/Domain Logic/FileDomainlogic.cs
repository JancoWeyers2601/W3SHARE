using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using W3SHARE.Models;
using W3SHARE.Repository;

namespace W3SHARE.Domain_Logic
{
    public class FileDomainlogic
    {

        public async Task <bool> UploadFile(File fileModel,Metadata metadataModel,string filePath)
        {
            FileStorageRepository fileStorageRepository = new FileStorageRepository();
            FileRepository fileRepository = new FileRepository();

            try
            {
                //Upload file to blob
                var filePathSplit = filePath.Split("\\");
                string fileName = filePathSplit[filePathSplit.Length - 1];

                //Write image from db to blob
                //string blobURL = fileStorageRepository.WriteFileToBlob(filePath,fileName);
                //USE FOR DOWNLOAD bool downloaded = fileStorageRepository.DownloadFileFromBlob(fileName);

                //Create file in DB 
                
                bool result = await fileRepository.CreateImageAsync(fileModel);

                //TODO add metadata here aswell (AS LINE 27)

                return result;//result;
            }
            catch (Exception e)
            {
                var logMessage = e;

                return false;
            }
     
        }

        //skryf een v edit 



    }
}
