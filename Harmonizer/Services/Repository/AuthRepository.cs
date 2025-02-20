using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Harmonizer.Data;
using Harmonizer.DTO;
using Harmonizer.Model;
using Harmonizer.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Harmonizer.Services.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly IConfiguration _configuration;
        private string SecretKey;

        public AuthRepository(ApplicationDBContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            SecretKey = _configuration.GetValue<string>("ApiSettings:Secret");
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginRequestDTO.Email);
                if (user == null)
                {

                    user = new User
                    {
                        Google_id = loginRequestDTO.Google_id,
                        Email = loginRequestDTO.Email,
                        Name = loginRequestDTO.Name,
                        Picture = loginRequestDTO.Picture,
                        Role = "User",  // Default role
                        CreatedDate = DateTime.UtcNow
                    };

                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    user.Last_login = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }

                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("UserId", user.UserId.ToString())
        };

                var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

                return new LoginResponseDTO
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Username = user.Name,
                    Userid = user.UserId
                };
            }
            catch (Exception ex) {
                throw;
            }
        }

        public async Task<ProfileDTO> GetUserDetails(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var profile = new ProfileDTO
            {
                Name = user.Name,
                Email = user.Email,
                Picture = user.Picture,
                CreatedDate = user.CreatedDate,
            };
            return profile;
        }
    }
}
