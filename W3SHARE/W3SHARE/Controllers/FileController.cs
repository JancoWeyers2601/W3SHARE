using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using W3SHARE.Data;
using W3SHARE.Domain_Logic;
using W3SHARE.Models;
using W3SHARE.Repository;


namespace W3SHARE.Controllers
{
    public class FileController : Controller
    {
        private readonly W3SHAREContext _context;
        private FileRepository fileRepository = new FileRepository();

        public FileController(W3SHAREContext context)
        {
            _context = context;
        }

        // GET: File
        public async Task<IActionResult> Index()
        {
            
            var curretlyLoggedInUserId = User.Claims.ToList()[0].Value;

            var results = await fileRepository.GetFilesByAccessAsync(Guid.Parse(curretlyLoggedInUserId));

            return View(results);
        }

        // GET: File/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.File
                .FirstOrDefaultAsync(m => m.FileId == id);
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
        public async Task<IActionResult> Create([Bind("FileId,UserId,AlbumId,LastModifiedBy,Url,DateModified,ContentType,DateCreated")] File file)
        {
            if (ModelState.IsValid)
            {
                file.FileId = Guid.NewGuid();

                FileRepository fileRepository = new FileRepository();
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
        public async Task<IActionResult> Capture([Bind("FileId,UserId,AlbumId,LastModifiedBy,Url,DateModified,ContentType,DateCreated,MetadataId,GeoLocation,Tags,CaptureBy,CaptureDate")] FileMetadataViewModel file , IFormFile formFile)
        {
            string path = @"C:\Users\janco\Desktop\New folder\W3SHARE\W3SHARE\wwwroot\Images\Temp\Boy.png";
            var curretlyLoggedInUserId = User.Claims.ToList()[0].Value;

            if (ModelState.IsValid)
            {
                File fileModel = new File()
                {
                    FileId = Guid.NewGuid(),
                    UserId = Guid.Parse(curretlyLoggedInUserId), //file.UserId,
                    AlbumId = Guid.NewGuid(), //file.AlbumId,
                    LastModifiedBy = file.LastModifiedBy,
                    Url = file.Url,
                    DateModified = file.DateModified,
                    ContentType = file.ContentType,
                    DateCreated = file.DateCreated
                };

                Metadata metadataModel = new Metadata()
                {
                    MetadataId = Guid.NewGuid(), //file.MetadataId,
                    GeoLocation = file.GeoLocation,
                    FileId = fileModel.FileId, // Keep in sync with File model
                    Tags = file.Tags,
                    CaptureBy = file.CaptureBy,
                    CaptureDate = file.CaptureDate
                };

                FileDomainlogic fileDomainlogic = new FileDomainlogic();

                var result = fileDomainlogic.UploadFile(fileModel,metadataModel,path);
                
                return RedirectToAction(nameof(Index));
            }
            return View(file);
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

            FileRepository fileRepository = new FileRepository();
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
        public async Task<IActionResult> Edit(Guid id, [Bind("FileId,UserId,AlbumId,LastModifiedBy,Url,DateModified,ContentType,DateCreated")] File file)
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
                    FileRepository fileRepository = new FileRepository();
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

            var file = await _context.File
                .FirstOrDefaultAsync(m => m.FileId == id);
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
            var file = await _context.File.FindAsync(id);
            _context.File.Remove(file);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(Guid id)
        {
            return _context.File.Any(e => e.FileId == id);
        }
    }
}
