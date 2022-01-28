using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        // getting the db context and all of the data on the database
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }


        // first endpoint at URL/api/users this function returns an Ienumerable which is basically a list with values of type AppUser
        // and it does that by first getting all users from the database and then using the db context fetches the list of all users from the database
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }


        // this is the second endpoint at URL/api/users/{id} and this is the syntax to create a httpget for a specific key and this is basically the same
        // thing as the first endpoint but it only returns a AppUser and also only gets and Fetches one user by it's specified primary id then it finds the user 
        // by id from the database table and returns it to the client
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUsers(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}