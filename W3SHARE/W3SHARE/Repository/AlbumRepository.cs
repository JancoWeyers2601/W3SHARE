using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using W3SHARE.Data;
using W3SHARE.Models;

namespace W3SHARE.Repository
{
    public class AlbumRepository
    {
        private readonly W3SHAREContext _context;

        //CONSTRUCTOR
        public AlbumRepository()
        {
            _context = new W3SHAREContext();
        }

        //ALBUM EXIST
        public bool AlbumExists(Guid id)
        {
            return _context.File.Any(e => e.FileId == id);
        }

        //GET Album BY USER ID 
        public async Task<List<Album>> GetAlbumByUserAsync(Guid userId)
        {

            try
            {
                var results = await _context.Album
                                .Where(Album => Album.CreatedBy == userId)
                                .ToListAsync();

                return results;
            }
            catch (Exception e)
            {
                return new List<Album>();
            }

        }

        //GET ACCESS BY GUID ID
        public async Task<Album> GetAlbumByIdAsync(Guid? id)
        {
            var result = await _context.Album.FirstOrDefaultAsync(m => m.AlbumId== id);

            return result;
        }



        //CREATE ACCESS
        public async Task<Boolean> CreateAlbumAsync(Album model)
        {
            try
            {
                _context.Add(model);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //UPDATE ACCESS
        public async Task<Boolean> UpdateAlbumAsync(Album model)
        {
            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //DELETE
        public async Task<Boolean> DeleteAlbumAsync(Guid id)
        {
            try
            {
                var result = await _context.Album.FindAsync(id);
                _context.Album.Remove(result);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
