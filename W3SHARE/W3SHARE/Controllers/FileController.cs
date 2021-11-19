
using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using W3SHARE.Data;
using W3SHARE.Domain_Logic;
using W3SHARE.Models;
using W3SHARE.Repository;




//JANCO DOEN


//TODO: 3 Call controller on text value change for search (Look at C3 chat for 3 files examples)

//TODO: 4 Add friendly message if user not logged in (toastyfy controller) https://github.com/nabinked/NToastNotify
//TODO: 4 ADD access that only log in users have access to other pages (controller)



//TODO: 5 Conn strings https://kontext.tech/column/aspnet-core/231/secrest-management-in-aspnetcore

//GEDOEN
//DONE 1 Implement iformfil solution to get full path and upload file: https://tutexchange.com/uploading-download-and-delete-files-in-azure-blob-storage-using-asp-net-core-3-1/
//DONE 2 Om image te kan view van blob storage https://www.c-sharpcorner.com/UploadFile/8a67c0/upload-image-in-azure-blob-storage-with-Asp-Net-mvc/
//WHEN files is deleted also delete the access in the db to that file


namespace W3SHARE.Controllers
{
    public class FileController : Controller
    {
        private FileRepository fileRepository = new FileRepository();
        private AccessRepository accessRepository = new AccessRepository();

        public async Task<IActionResult> Index()
        {
            
            var curretlyLoggedInUserId = (User.Claims.ToList()[0].Value).ToUpper();

            var results = await fileRepository.GetFilesByAccessAsync (Guid.Parse(curretlyLoggedInUserId));
            

            return View(results);
        }

       




        // GET: File/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await fileRepository.GetImageByIdAsync(id);

            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // GET: File/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: File/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FileId,UserId,AlbumId,Name,LastModifiedBy,Url,DateModified,ContentType,DateCreated")] File file)
        {
            if (ModelState.IsValid)
            {
                file.FileId = Guid.NewGuid();

                var result = await fileRepository.CreateImageAsync(file);

                return RedirectToAction(nameof(Index));
            }
            return View(file);
        }

        //CAPTURE 
        public IActionResult Capture()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Capture([Bind("FileId,UserId,AlbumId,Name,LastModifiedBy,Url,DateModified,ContentType,DateCreated,MetadataId,GeoLocation,Tags,CaptureBy,CaptureDate,Image")] FileMetadataViewModel fileMetadataModel)
        {

            var curretlyLoggedInUserId = User.Claims.ToList()[0].Value;

            if (ModelState.IsValid)
            {
                var extension = fileMetadataModel.Image.FileName.Split(".").Last();

                var link = "https://w3share.blob.core.windows.net/data/" + fileMetadataModel.Image.FileName;


                File fileModel = new File()
                {
                    FileId = Guid.NewGuid(),
                    UserId = Guid.Parse(curretlyLoggedInUserId), //file.UserId,
                    AlbumId = null, //Guid.NewGuid(), //Will be empty until album is linked
                    Name = fileMetadataModel.Image.FileName,//$"{fileMetadataModel.Name}.{extension}", //TODO: MEETTWEE Maak sekker filename wat gestoor word word in regte veld gestoor
                    //LastModifiedBy = fileModel.UserId,
                    Url = link, //CHANGE TO SELECTED VALUE 
                    DateModified = DateTime.Now,
                    ContentType = ("." + extension), //CHANGE TO EXTENSION OF PATH
                    DateCreated = DateTime.Now
                };

                fileModel.LastModifiedBy = fileModel.UserId;

                Metadata metadataModel = new Metadata()
                {
                    MetadataId = Guid.NewGuid(), //file.MetadataId,
                    GeoLocation = fileMetadataModel.GeoLocation,
                    FileId = fileModel.FileId, // Keep in sync with File model
                    Tags = fileMetadataModel.Tags,
                    CaptureBy = fileMetadataModel.CaptureBy,
                    CaptureDate = fileMetadataModel.CaptureDate
                };

                Access access = new Access()
                {
                    AccessId = Guid.NewGuid(),
                    FileId = fileModel.FileId,
                    UserId = Guid.Parse(curretlyLoggedInUserId)
                };

                FileDomainlogic fileDomainlogic = new FileDomainlogic();

                //TODO: ADD to repo
                string blobstorageconnection = "DefaultEndpointsProtocol=https;AccountName=w3share;AccountKey=bqNgzLtSaSMLeEqS9KUK9Q/YB8bYL8AQ2q/jC7KDZzBHtvZdSCBZRDHnW4x/TXHXbA6qcl1RF7XDH/ZIE0Rkwg==;EndpointSuffix=core.windows.net";

                byte[] dataFiles;

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("data");

                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };

                string systemFileName = fileMetadataModel.Image.FileName;

                await cloudBlobContainer.SetPermissionsAsync(permissions);

                await using (var target = new System.IO.MemoryStream())
                {

                    fileMetadataModel.Image.CopyTo(target);
                    dataFiles = target.ToArray();
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
                await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);

                string path = "";

                var result = fileDomainlogic.UploadFile(fileModel,metadataModel,access,path); //ADD fileID to be able to upload the file to the blob with the fileID as the name

                return RedirectToAction(nameof(Feed));
            }
            return View(fileMetadataModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await fileRepository.GetImageByIdAsync(id);

            //kopel aan repo

            if (file == null)
            {
                return NotFound();
            }
            return View(file);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FileId,UserId,AlbumId,Name,LastModifiedBy,Url,DateModified,ContentType,DateCreated")] File file)
        {


            if (id != file.FileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await fileRepository.UpdateImageAsync(file);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(file.FileId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(file);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await fileRepository.GetImageByIdAsync((Guid)id); 

            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // POST: File/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //DELETE File in DB
            var result_file = await fileRepository.DeleteImageAsync(id);

            var result_access = await accessRepository.DeleteAccessByFileAsync(id);

            //TODO:DELETE METADATA RECORD in DB
            //var result_access = await accessRepository.DeleteAccessByFileAsync(id);

            //DELETE FROM BLOB


            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(Guid id)
        {
            var file = fileRepository.ImageExists(id);

            return file;
        }

        public async Task<IActionResult> Download(string blobName)
        {

            //TODO: ADD to repo
            CloudBlockBlob blockBlob;

            await using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                string blobstorageconnection = "DefaultEndpointsProtocol=https;AccountName=w3share;AccountKey=bqNgzLtSaSMLeEqS9KUK9Q/YB8bYL8AQ2q/jC7KDZzBHtvZdSCBZRDHnW4x/TXHXbA6qcl1RF7XDH/ZIE0Rkwg==;EndpointSuffix=core.windows.net";
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("data");
                blockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
                await blockBlob.DownloadToStreamAsync(memoryStream);
            }

            System.IO.Stream blobStream = blockBlob.OpenReadAsync().Result;

            return File(blobStream, blockBlob.Properties.ContentType, blockBlob.Name);
        }

        public async Task<IActionResult> Feed(string searchString)
        {
            FileRepository fileRepository = new FileRepository();

            if (User.Identity.IsAuthenticated)
            {



                var curretlyLoggedInUserId = (User.Claims.ToList()[0].Value).ToUpper();

                var results = await fileRepository.GetFilesByAccessAsync(Guid.Parse(curretlyLoggedInUserId));

                List<File> tempResults = new List<File>();

                foreach (var item in results)
                {
                    var tempFile = new File()
                    {
                        FileId = item.FileId,
                        AlbumId = item.AlbumId,
                        ContentType = item.ContentType,
                        DateCreated = item.DateCreated,
                        DateModified = item.DateModified,
                        LastModifiedBy = item.LastModifiedBy,
                        Name = item.Name,
                        UserId = item.UserId,
                        Url = "https://i.pinimg.com/originals/22/ff/08/22ff08bc265e47113a9627114684d7d1.jpg"
                    };

                    tempResults.Add(tempFile);
                }


            //TODO: Fix as it only returns the null no matter what the search string is

            searchString = ".";
                if (!String.IsNullOrEmpty(searchString))
                {
                    tempResults = tempResults.Where(s => s.FileId.ToString().Contains(searchString) ||
                                          s.AlbumId.ToString().Contains(searchString) ||
                                          s.ContentType.ToString().Contains(searchString) ||
                                          s.DateCreated.ToString().Contains(searchString) ||
                                          s.DateModified.ToString().Contains(searchString) ||
                                          s.LastModifiedBy.ToString().Contains(searchString) ||
                                          s.UserId.ToString().Contains(searchString) ||
                                          s.Url.ToString().Contains(searchString)).ToList();
                }
                return View(results);
            }
            return View();
            

        }

        public async Task<IActionResult> Content()
        {
            //TODO: implement search string
            FileRepository fileRepository = new FileRepository();

            var curretlyLoggedInUserId = (User.Claims.ToList()[0].Value).ToUpper();

            var results = await fileRepository.GetFilesByUserAsync(Guid.Parse(curretlyLoggedInUserId));


            // this can be deleted and revert to results when returning the view
            List<File> tempResults = new List<File>();

            foreach (var item in results)
            {
                tempResults.Append(new File()
                {
                    FileId = item.FileId,
                    AlbumId = item.AlbumId,
                    ContentType = item.ContentType,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    LastModifiedBy = item.LastModifiedBy,
                    UserId = item.UserId,
                    Url = "https://i.pinimg.com/originals/22/ff/08/22ff08bc265e47113a9627114684d7d1.jpg"
                });
            }

            return View(results);
        }





    }
}
