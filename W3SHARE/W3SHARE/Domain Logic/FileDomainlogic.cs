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

        public async Task <bool> UploadFile(File fileModel,Metadata metadataModel,Access accessModel,string filePath)
        {
            FileStorageRepository fileStorageRepository = new FileStorageRepository();

            FileRepository fileRepository = new FileRepository();
            MetadataRepository metadataRepository = new MetadataRepository();
            AccessRepository accessRepository = new AccessRepository();

            try
            {
                string fileName = fileModel.Name;

                //Upload file to BLOB 
                //string blobURL = fileStorageRepository.WriteFileToBlob(filePath,fileName);

                //Update DB
                bool result = await fileRepository.CreateImageAsync(fileModel);
                bool result_2 = await metadataRepository.CreateMetadataAsync(metadataModel);
                bool result_3 = await accessRepository.CreateAccessAsync(accessModel);


                return result;
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
