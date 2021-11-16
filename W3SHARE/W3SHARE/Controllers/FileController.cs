
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

//TODO: 2 Om image te kan view van blob storage https://www.c-sharpcorner.com/UploadFile/8a67c0/upload-image-in-azure-blob-storage-with-Asp-Net-mvc/

//TODO: 4 Add friendly message if user not logged in (toastyfy controller) https://github.com/nabinked/NToastNotify
//TODO: 4 ADD access that only log in users have access to other pages (controller)
//TODO: 3 Call controller on text value change for search (Look at C3 chat for 3 files examples)


//TODO: 5 Conn strings https://kontext.tech/column/aspnet-core/231/secrest-management-in-aspnetcore


//DONE 1 Implement iformfil solution to get full path and upload file: https://tutexchange.com/uploading-download-and-delete-files-in-azure-blob-storage-using-asp-net-core-3-1/

namespace W3SHARE.Controllers
{
    public class FileController : Controller
    {
        private FileRepository fileRepository = new FileRepository();

        // GET: File
        public async Task<IActionResult> Index()
        {
            
            var curretlyLoggedInUserId = (User.Claims.ToList()[0].Value).ToUpper();

            var results = await fileRepository.GetFilesByUserAsync (Guid.Parse(curretlyLoggedInUserId));
            //TODO: implement search button here !

            return View(results);
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

        // GET: File
        public async Task<IActionResult> Test()
        {
            FileRepository fileRepository = new FileRepository();

            var results = await fileRepository.GetAllImagesAsync();

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
            //TODO: Get path from iFormFile


            string path = @"C:\Users\janco\Desktop\New folder\W3SHARE\W3SHARE\wwwroot\Images\Temp\Boy.png";
      


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
                    LastModifiedBy = fileMetadataModel.LastModifiedBy,
                    Url = link, //CHANGE TO SELECTED VALUE 
                    DateModified = fileMetadataModel.DateModified,
                    ContentType = ("." + extension), //CHANGE TO EXTENSION OF PATH
                    DateCreated = DateTime.Now
                };

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





                //===================================================
                //TODO: ADD to repo
                string blobstorageconnection = "DefaultEndpointsProtocol=https;AccountName=w3share;AccountKey=bqNgzLtSaSMLeEqS9KUK9Q/YB8bYL8AQ2q/jC7KDZzBHtvZdSCBZRDHnW4x/TXHXbA6qcl1RF7XDH/ZIE0Rkwg==;EndpointSuffix=core.windows.net";

                byte[] dataFiles;
                // Retrieve storage account from connection string.
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
                // Create the blob client.
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                // Retrieve a reference to a container.
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
                // This also does not make a service call; it only creates a local object.
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(systemFileName);
                await cloudBlockBlob.UploadFromByteArrayAsync(dataFiles, 0, dataFiles.Length);

                //++++++++=+++++++++++++++++++++++++++++++++++++++++++++++


                //DO NOT DELETE
                var result = fileDomainlogic.UploadFile(fileModel,metadataModel,access,path); //ADD fileID to be able to upload the file to the blob with the fileID as the name

                return RedirectToAction(nameof(Test));
            }
            return View(fileMetadataModel);
        }

        // GET: File/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            // Trek inligting van die image wat ons wil update
            // GET = Voor update
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

        // POST: File/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FileId,UserId,AlbumId,Name,LastModifiedBy,Url,DateModified,ContentType,DateCreated")] File file)
        {
            // Veranderinge wat ons gemaak het
            // POST = na update

            if (id != file.FileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await fileRepository.UpdateImageAsync(file);

                    //update image async
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

        // GET: File/Delete/5
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
            var result = await fileRepository.DeleteImageAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(Guid id)
        {
            var file = fileRepository.ImageExists(id);

            return file;
        }
    }
}
