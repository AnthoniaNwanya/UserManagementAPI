using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Data;
using UserManagement.Models;


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

                HashSet<char> specialCharacters = new HashSet<char>() { '%', '$', '#', '@', '.', '&' };
                if (user.Password.Any(char.IsLower) &&
                     user.Password.Any(char.IsUpper) &&
                     user.Password.Any(char.IsDigit) &&
                    user.Password.Any(specialCharacters.Contains))
                {

                    var foundData = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email || x.PhoneNumber == user.PhoneNumber);
                    if (foundData == null)
                    {
                        user.Password = Encrypt(user.Password);
                        _context.Users.Add(user);

                        await _context.SaveChangesAsync();

                        return Ok(user);
                    }
                    return NotFound("User with this Email or PhoneNumber already exists.");
                }
                return BadRequest("Password must contain atleast one Uppercase, a Digit and one SpecialCharacter.");
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
        private string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212hgvfy";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

    }
}
