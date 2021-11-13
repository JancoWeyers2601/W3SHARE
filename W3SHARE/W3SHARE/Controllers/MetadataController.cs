using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using W3SHARE.Data;
using W3SHARE.Models;
using W3SHARE.Repository;

namespace W3SHARE.Controllers
{
    public class MetadataController : Controller
    {
        MetadataRepository metadataRepository = new MetadataRepository();

        // GET: Metadata
        public async Task<IActionResult> Index()
        {
            var curretlyLoggedInUserId = (User.Claims.ToList()[0].Value).ToUpper();

            //var results = await fileRepository.GetFilesByAccessAsync(Guid.Parse(curretlyLoggedInUserId));
            var results = await metadataRepository.GetMetadataByUserAsync(Guid.Parse(curretlyLoggedInUserId));

            return View(results);
        }

        // GET: Metadata/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metadata = await metadataRepository.GetMetadataByIdAsync(id);

            if (metadata == null)
            {
                return NotFound();
            }

            return View(metadata);
        }

        // GET: Metadata/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Metadata/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MetadataId,FileId,GeoLocation,Tags,CaptureBy,CaptureDate")] Metadata metadata)
        {
            if (ModelState.IsValid)
            {
                var result = metadataRepository.CreateMetadataAsync(metadata);

                return RedirectToAction(nameof(Index));
            }
            return View(metadata);
        }

        // GET: Metadata/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metadata = await metadataRepository.GetMetadataByIdAsync(id);

            if (metadata == null)
            {
                return NotFound();
            }
            return View(metadata);
        }

        // POST: Metadata/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MetadataId,FileId,GeoLocation,Tags,CaptureBy,CaptureDate")] Metadata metadata)
        {
            if (id != metadata.MetadataId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await metadataRepository.UpdateMetadataAsync(metadata);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetadataExists(metadata.MetadataId))
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
            return View(metadata);
        }

        // GET: Metadata/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metadata = await metadataRepository.GetMetadataByIdAsync(id);

            if (metadata == null)
            {
                return NotFound();
            }

            return View(metadata);
        }

        // POST: Metadata/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await metadataRepository.DeleteMetadataAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private bool MetadataExists(Guid id)
        {
            var result = metadataRepository.MetadataExists(id);

            return result ;
        }
    }
}
