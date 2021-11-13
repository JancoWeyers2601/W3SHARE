using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using W3SHARE.Data;
using W3SHARE.Models;

namespace W3SHARE.Repository
{
    public class MetadataRepository
    {
        private readonly W3SHAREContext _context;

        //CONSTRUCTOR
        public MetadataRepository()
        {
            _context = new W3SHAREContext();
        }

        //Metadata EXIST
        public bool MetadataExists(Guid id)
        {
            return _context.Metadata.Any(e => e.MetadataId == id);
        }

        //GET Metadata BY USER ID 
        public async Task<List<Metadata>> GetMetadataByUserAsync(Guid userId)
        {
            //TODO: moet gejoin word van user na file na metadata
            try
            {
                var results = from access in _context.Access
                              join file in _context.File on access.FileId equals file.FileId
                              join metadata in _context.Metadata on file.FileId equals metadata.FileId
                              where access.UserId == userId
                              select metadata;

                List<Metadata> arr = new List<Metadata>();
                arr = results.ToList();

                return arr;
            }
            catch (Exception e)
            {
                return new List<Metadata>();
            }
        }

        //GET Metadata BY GUID ID
        public async Task<Metadata> GetMetadataByIdAsync(Guid? id)
        {
            var result = await _context.Metadata.FirstOrDefaultAsync(m => m.MetadataId == id);

            return result;
        }



        //CREATE Metadata
        public async Task<Boolean> CreateMetadataAsync(Metadata model)
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

        //UPDATE Metadata
        public async Task<Boolean> UpdateMetadataAsync(Metadata model)
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

        //DELETE Metadata
        public async Task<Boolean> DeleteMetadataAsync(Guid id)
        {
            try
            {
                var result = await _context.Metadata.FindAsync(id);
                _context.Metadata.Remove(result);
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
