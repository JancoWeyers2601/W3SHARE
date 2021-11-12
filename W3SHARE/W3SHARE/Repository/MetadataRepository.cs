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

        //Constructor
        public MetadataRepository()
        {
            _context = new W3SHAREContext();
        }

        //CREATE META DATA
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

    }
}
