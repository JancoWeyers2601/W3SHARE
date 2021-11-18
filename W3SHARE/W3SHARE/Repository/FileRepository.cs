using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using W3SHARE.Data;
using W3SHARE.Models;

namespace W3SHARE.Repository
{

    public class FileRepository
    {
        private readonly W3SHAREContext _context;

        //Constructor
        public FileRepository()
        {
            _context = new W3SHAREContext();
        }

        //FILE EXIST
        public bool ImageExists(Guid id)
        {
            return _context.File.Any(e => e.FileId == id);
        }

        //GET FILE BY GUID ID
        public async Task<File> GetImageByIdAsync(Guid? id)
        {      
            var result = await _context.File.FirstOrDefaultAsync(m => m.FileId == id);

            return result;
        }

        //GET ALL FILES
        public async Task<IEnumerable<File>> GetAllImagesAsync()
        {
            var query = await _context.File.ToListAsync();

            return query;
        }

        //GET FILES IN MODEL LIST

        public List<SelectListItem> GetImagesList()
        {
            var query = _context.File.Select(a =>
                          new SelectListItem
                          {
                              Value = a.FileId.ToString(),
                              Text = a.Url
                          }).ToList(); ;
            return query;
        }

        //CREATE FILE
        public async Task<Boolean> CreateImageAsync(File model)
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

        //UPDATE FILE
        public async Task<Boolean> UpdateImageAsync(File model)
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

        //DELETE FILE
        public async Task<Boolean> DeleteImageAsync(Guid id)
        {
            try
            {
                var result = await _context.File.FindAsync(id);
                _context.File.Remove(result);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Get all files user has access to
        public async Task<List<File>> GetFilesByUserAsync(Guid userId)
        {
            var Files = new List<File>();

            try
            {
                var results = await _context.File
                                .Where(File => File.UserId == userId)
                                .ToListAsync();

                return results;
            }
            catch (Exception e)
            {
                return new List<File>();
            }
        }

        //Get all files that belongs to user
        public async Task<List<File>> GetFilesByAccessAsync(Guid userId)
        {


            try
            {
                var results = from access in _context.Access
                              join file in _context.File on access.FileId equals file.FileId
                              where access.UserId == userId
                              select file;

                List<File> lst = new List<File>();
                lst = results.ToList();

                return lst;
            }
            catch (Exception e)
            {
                return new List<File>();
            }
        }



    }
}