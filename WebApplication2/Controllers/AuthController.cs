using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WebApplication2.Models;
using System.Web;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http;
using System;
using Microsoft.OpenApi.Models;
using System.Web.Http.Controllers;
using Azure.Core;
using Microsoft.AspNetCore.Components.Forms;

namespace WebApplication2.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        
        public User user = new User();
        public ILogger<AuthController> _logger;
        public ToDoContext ToDoContext { get; set; }
        public IConfiguration Configuration;
        public string JWTTOKEN { get; set; }

        public AuthController(ILogger<AuthController> logger, ToDoContext toDoContext, IConfiguration configuration)
        {
                ToDoContext = toDoContext;
                Configuration = configuration;
                _logger = logger;         
        }


       [HttpPost("SignUp")]
       public async Task<string> SignUp (UserDto userdto)
        {
         
            
            var check_username = (from x in ToDoContext.Users where x.Username == userdto.Username select x.Id).FirstOrDefault();
                       
            //if check_username is 0 means it is a new username
            if (check_username == 0 && ModelState.IsValid)
            {

                CreatePasswordHash(userdto.Password, out byte[] passwordHash, out byte[] passwordSalt);
               
                user.Username = userdto.Username;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                //use in memory database for now
                ToDoContext.Users.Add(user);
                await ToDoContext.SaveChangesAsync();

                return "the user has been created";
            }
            else
            {
                return "the user has not been created yet";
            }
          
        }



        [HttpPost("login")]
        public async Task<string> Login(UserDto userdto)
        {
          
            var firstuser = await ToDoContext.Users.FirstOrDefaultAsync(x => x.Username == userdto.Username);
            var username = firstuser?.Username.ToString();

            _logger.LogInformation(username);

            if (username != userdto.Username)
            {
               return "Invalid Username";
            }
            //if the new the dto password matches what's in the database then..the password has been varified correctly 
            if (!VerifyPasswordHash(userdto.Password, firstuser.PasswordHash, firstuser?.PasswordSalt))
            {
               return "Invalid Password";
            }

            JWTTOKEN = ReturnJWTToken(userdto, "Admin");


            return JWTTOKEN;
        }


        //get all the users if you are authorized to do so 
         [HttpGet("getusers"), Authorize(Roles = "Admin")]
      // [HttpGet("getusers")]
        public async Task<IEnumerable<User>> GetUsers()
        {
           
            var allusers = await ToDoContext.Users.ToArrayAsync();
            return allusers;
        }


        private string ReturnJWTToken(UserDto user, string? role)
        {
            _logger.LogInformation(user.Username.ToString());
            List<Claim> claimInfo = new List<Claim>() { new Claim("username", user.Username.ToString()), 
           // new Claim(ClaimTypes.Role, "Admin")
              new Claim(ClaimTypes.Role, role)
            };

            //secret key currently stored in appsettings.json
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature); 
            var token = new JwtSecurityToken(claims: claimInfo, expires: DateTime.Now.AddDays(1), signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            //send the cookie to the frontend response header 
            //HttpContext.Response.Cookies.Append("token", jwt, 
            //    new CookieOptions
            //    {
            //        Expires = DateTime.Now.AddDays(1),
            ////        HttpOnly = true, 
            //        Secure = true,
            //        IsEssential = true,
            //        SameSite = SameSiteMode.None,
            //    }
            //); 

            return jwt;
        }

       private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt ) {
            
            //if the user has a password salt, reingneer the values so that you can compare if the hash goes with the salt 
            using (var HMAC = new HMACSHA512(passwordSalt))
            {
                //converts the password into a byte and compute the hash 
                var computeHash = HMAC.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computeHash.SequenceEqual(passwordHash);
            }
        
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[]passwordSalt ) {
        
            using (var hmac = new HMACSHA512())
            {
                //get the hash and the salt 
                passwordSalt = hmac.Key; 
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
        

    }
}
