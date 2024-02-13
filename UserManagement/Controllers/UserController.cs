using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Validation;


namespace UserManagement.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {

            try
            {
                Encryption encryption = new Encryption();
                string Encrypt = encryption.Encrypt(user.Password);

                validation validationcheck = new validation();
                bool validpass = validationcheck.IsValidPassword(user.Password);
                if (validpass)
                {
                    var foundData = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email || x.PhoneNumber == user.PhoneNumber);
                    if (foundData == null)
                    {
                        user.Password = Encrypt;
                        _context.Users.Add(user);

                        await _context.SaveChangesAsync();

                        return Ok(user);
                    }
                    return NotFound("User with this Email or PhoneNumber already exists.");

                }
                else
                {
                    return BadRequest("Password must contain atleast one Uppercase, a Digit and one SpecialCharacter.");

                }

            }
            catch (Exception e)
            {
                return BadRequest(new { ErrorMessage = e.Message });
            }
        }

        [HttpGet("{Email}")]
        public async Task<IActionResult> GetUsers(string Email)
        {

            var foundData = await _context.Users.FirstOrDefaultAsync(x => x.Email == Email);
            if (foundData == null)
                return NotFound("User with that Email was not found");

            return Ok(foundData);

        }



    }
}
