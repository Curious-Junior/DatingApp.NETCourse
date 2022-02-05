using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        // this is just like getting users but it's posting users
        // we would have to add some attributes to the argments in the function but the [ApiController] tag
        // automates that for us
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // check if user is already registered and if it is, retur a badrequest error 
            if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            // this statement get the hashing algorithm and by adding the using keyword, this variable will be
            // properly disposed of when we are donw with it
            using var hmac = new HMACSHA512();

            // making the user that we will add to the database
            var user = new AppUser{
                // setting the username in lowercase
                UserName = registerDto.Username.ToLower(),
                // computing and setting the password hash
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                // getting and setting the password salt to the hasher key
                PasswordSalt = hmac.Key
            };


            // this adds the user to the tracking list of the dbcontext
            _context.Users.Add(user);
            // and this actually adds the user to the database
            await _context.SaveChangesAsync();

            // return the jwt token
            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }


        // endpoint for logging in
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login( LoginDto loginDto )
        {
            // get the user from the database
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            // if the user is not in the database, then return a unauthorized error
            if (user == null) return Unauthorized("Invalid Username");

            // get hmac to recreate the password hash and use the password salt as the key to get the password
            // same result
            using var hmac = new HMACSHA512(user.PasswordSalt);

            // compute the password hash
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            // match the password hash to see if it is correct and throw an unauthorized error if it is wrong
            for (int i = 0; i<computedHash.Length; i++){
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            // return the jwt token if everything succeeds
            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        // method to check if a user is already registered
        private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}