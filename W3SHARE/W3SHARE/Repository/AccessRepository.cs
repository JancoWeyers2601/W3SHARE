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

        //Constructor
        public AccessRepository()
        {
            _context = new W3SHAREContext();
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

    }
}
