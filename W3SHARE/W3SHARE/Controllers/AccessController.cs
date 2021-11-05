using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using W3SHARE.Data;
using W3SHARE.Models;

namespace W3SHARE.Controllers
{
    public class AccessController : Controller
    {
        private readonly W3SHAREContext _context;

        public AccessController(W3SHAREContext context)
        {
            _context = context;
        }

        // GET: Access
        public async Task<IActionResult> Index()
        {
            return View(await _context.Access.ToListAsync());
        }

        // GET: Access/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var access = await _context.Access
                .FirstOrDefaultAsync(m => m.AccessId == id);
            if (access == null)
            {
                return NotFound();
            }

            return View(access);
        }

        // GET: Access/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Access/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccessId,UserId,FileId")] Access access)
        {
            if (ModelState.IsValid)
            {
                access.AccessId = Guid.NewGuid();
                _context.Add(access);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(access);
        }

        // GET: Access/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var access = await _context.Access.FindAsync(id);
            if (access == null)
            {
                return NotFound();
            }
            return View(access);
        }

        // POST: Access/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AccessId,UserId,FileId")] Access access)
        {
            if (id != access.AccessId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(access);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccessExists(access.AccessId))
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
            return View(access);
        }

        // GET: Access/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var access = await _context.Access
                .FirstOrDefaultAsync(m => m.AccessId == id);
            if (access == null)
            {
                return NotFound();
            }

            return View(access);
        }

        // POST: Access/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var access = await _context.Access.FindAsync(id);
            _context.Access.Remove(access);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccessExists(Guid id)
        {
            return _context.Access.Any(e => e.AccessId == id);
        }
    }
}
