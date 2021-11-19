using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using W3SHARE.Data;
using W3SHARE.Models;

namespace W3SHARE.Repository
{
    public class AccessRepository
    {
        private readonly W3SHAREContext _context;

        //CONSTRUCTOR
        public AccessRepository()
        {
            _context = new W3SHAREContext();
        }

        //ACCESS EXIST
        public bool AccessExists(Guid id)
        {
            return _context.File.Any(e => e.FileId == id);
        }

        //GET ACCESS BY USER ID 
        public async Task<List<Access>> GetAccessByUserAsync(Guid userId)
        {

            try
            {
                var results = await _context.Access
                                .Where(Access => Access.UserId == userId)
                                .ToListAsync();

                return results;
            }
            catch (Exception e)
            {
                return new List<Access>();
            }

        }

        //GET ACCESS BY GUID ID
        public async Task<Access> GetAccessByIdAsync(Guid? id)
        {
            var result = await _context.Access.FirstOrDefaultAsync(m => m.AccessId == id);

            return result;
        }

        

        //CREATE ACCESS
        public async Task<Boolean> CreateAccessAsync(Access model)
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
        public async Task<Boolean> UpdateAccessAsync(Access model)
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
        public async Task<Boolean> DeleteAccessAsync(Guid id)
        {
            try
            {
                var result = await _context.Access.FindAsync(id);

                _context.Access.Remove(result);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //DELETE
        public async Task<Boolean> DeleteAccessByFileAsync(Guid fileId)
        {
            try
            {
                //var accessId = from access in _context.Access
                //             join file in _context.File on access.FileId equals file.FileId
                //             where file.FileId == fileId
                //             select access.AccessId;


                var result = _context.Access.Where(e => e.FileId == fileId).FirstOrDefault();

                _context.Access.Remove(result);


                await _context.SaveChangesAsync();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }



    }
}
