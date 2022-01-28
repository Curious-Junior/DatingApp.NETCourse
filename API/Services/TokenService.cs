using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        // making the key needed for signing the token
        // symmetric key means that the same key is used for encryting and ecryption the token
        private readonly SymmetricSecurityKey _key;

        // the class constructor
        public TokenService(IConfiguration config)
        {
            // setting the key variable
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }


        // function to create a new token
        public string CreateToken(AppUser user)
        {
            // make a list of claims
            var claims = new List<Claim>{
                // make a new claim of type jwt claim.NameId and set it to the username of the user
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            // mkae the credentials that we will be signing the token with
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // create the descriptor of the token and set all the values
            var tokenDescriptor  = new SecurityTokenDescriptor
            {
                // the subject is the claims
                Subject = new ClaimsIdentity(claims),
                // the expiry date is 7 days after creation
                Expires = DateTime.Now.AddDays(7),
                // and these are the credentials that will be used to sign the token
                SigningCredentials = creds
            };

            // we need this token handler to make the token
            var tokenHandler = new JwtSecurityTokenHandler();

            // actually creating the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // and FINALLY returning the token data
            return tokenHandler.WriteToken(token);
        }
    }
}