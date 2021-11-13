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
    public class AlbumController : Controller
    {
        AlbumRepository albumRepository = new AlbumRepository();

        // GET: Album
        public async Task<IActionResult> Index()
        {

            var curretlyLoggedInUserId = (User.Claims.ToList()[0].Value).ToUpper();

            return View( await albumRepository.GetAlbumByUserAsync(Guid.Parse(curretlyLoggedInUserId)));
        }

        // GET: Album/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await albumRepository.GetAlbumByIdAsync(id);

            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Album/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Album/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumId,CreatedBy,Name,Description,DateCreated")] Album album)
        {
            var curretlyLoggedInUserId = (User.Claims.ToList()[0].Value).ToUpper();

            if (ModelState.IsValid)
            {
                album.AlbumId = Guid.NewGuid();
                album.CreatedBy = Guid.Parse(curretlyLoggedInUserId);

                var result = await albumRepository.CreateAlbumAsync(album);

                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        // GET: Album/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await albumRepository.GetAlbumByIdAsync(id);

            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        // POST: Album/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AlbumId,CreatedBy,Name,Description,DateCreated")] Album album)
        {
            if (id != album.AlbumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await albumRepository.UpdateAlbumAsync(album);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumId))
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
            return View(album);
        }

        // GET: Album/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await albumRepository.GetAlbumByIdAsync(id);

            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await albumRepository.DeleteAlbumAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(Guid id)
        {
            return albumRepository.AlbumExists(id);
        }
    }
}
