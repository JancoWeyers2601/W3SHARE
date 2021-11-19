using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using W3SHARE.Data;
using W3SHARE.Models;

namespace W3SHARE.Repository
{
    public class UserRepository
    {
        private readonly W3SHAREContext _context;

        //CONSTRUCTOR
        public UserRepository()
        {
            _context = new W3SHAREContext();
        }

       
        //GET USER BY USEREMAIL
        public AspNetUsers GetUserByEmail(string email)
        {

            try
            {
                var results = _context.AspNetUsers
                                .Where(user=> user.Email.ToLower().Equals(email.ToLower()))
                                .FirstOrDefault();

                return results;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
