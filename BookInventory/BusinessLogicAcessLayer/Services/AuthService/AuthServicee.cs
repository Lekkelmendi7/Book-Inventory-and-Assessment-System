using AutoMapper;
using BookInventory.BusinessLogicAcessLayer.Models.AccountModels;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookInventory.BusinessLogicAcessLayer.Services.AuthService
{
    public class AuthServicee : IAuthService
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthServicee(DatabaseContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.Include(s => s.RoleUsers).ThenInclude(ru => ru.Role).FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User Not Found!";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password!";
            }
            else
            {
                // Get permissions for roles of the user
                List<int> roleIds = user.RoleUsers.Select(u => u.RoleId).ToList();
                var permissions = await GetPermissions(roleIds);

                // Ensure permissions are unique
                permissions = permissions.Distinct().ToList();

                response.Data = CreateToken(user, permissions);
            }
            return response;
        }

        private async Task<List<string>> GetPermissions(List<int> roleIds)
        {
            var rolePermissions = await _context.RolePermissions
                .Include(x => x.Permission)
                .Where(x => roleIds.Contains(x.RoleId))
                .ToListAsync();

            return rolePermissions.Select(rp => rp.Permission.Name).ToList();
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            if (await UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "User already exists!";
                return response;
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }

        public async Task<IEnumerable<UserGetModel>> GetUsers()
        {
            var users = await _context.Users.Include(s => s.RoleUsers).ThenInclude(ru => ru.Role).ToListAsync();
            return _mapper.Map<IEnumerable<UserGetModel>>(users);
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        /*private string CreateToken(User user, List<string> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permissions", permission));
            }

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken is null)
                throw new Exception("AppSettings Token is null");

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }*/




        private string CreateToken(User user, List<string> permissions)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        // Add other necessary claims
    };

            // Add role claim(s)
            var roleClaims = user.RoleUsers.Select(r => new Claim(ClaimTypes.Role, r.Role.Name));
            claims.AddRange(roleClaims);

            // Add permission claims
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permissions", permission));
            }

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken is null)
                throw new Exception("AppSettings Token is null");

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1), // Adjust expiration as needed
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }




        public async Task<UserGetModel> GetUserById(int id)
        {
            var user = await _context.Users.Include(r => r.RoleUsers).ThenInclude(ru => ru.Role).FirstOrDefaultAsync(r => r.Id == id);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserGetModel>(user);
        }

        public async Task UpdateUser(UserUpdateModel user, int id)
        {
            var userModel = await _context.Users.Include(u => u.RoleUsers).FirstOrDefaultAsync(l => l.Id == id);

            if (userModel == null)
            {
                return;
            }

            userModel.Username = user.Username;

            _context.RoleUsers.RemoveRange(userModel.RoleUsers);

            if (user.RolesIds != null && user.RolesIds.Any())
            {
                foreach (var role in user.RolesIds)
                {
                    var roleUser = new RoleUser
                    {
                        UserId = userModel.Id,
                        RoleId = role
                    };
                    _context.RoleUsers.Add(roleUser);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ServiceResponse<string>> ChangePassword(string username, string currentPassword, string newPassword)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

            //Checking if user does not exist
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            //verify the current passowrd to authenticate user
            if (!VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect current password!";
                return response;
            }


            //Generate new password hash and salt
            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Password reset successfully";
            return response;

        }






        public async Task<ServiceResponse<string>> ForgotPassword(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var response = new ServiceResponse<string>();

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found!";
                response.Data = null;
            }
            else
            {
                var resetToken = GeneratePasswordResetToken(user);
                user.PasswordResetToken = resetToken;
                user.ResetTokenExpires = DateTime.Now.AddHours(1); // Token expiration time
                await _context.SaveChangesAsync();

                // Send the reset token to the user through a secure channel (e.g., SMS)
                // await SendResetTokenToUser(user.PhoneNumber, resetToken);

                response.Success = true;
                response.Message = "Password reset token has been generated.";
                response.Data = resetToken; // Return the token for testing/debugging purposes
            }
            return response;
        }



        public async Task<ServiceResponse<string>> ResetPassword(PasswordResetModel request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            var response = new ServiceResponse<string>();

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                response.Success = false;
                response.Message = "Invalid token!";
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Password reset successfully!";
            return response;
        }

        private string GeneratePasswordResetToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken is null)
                throw new Exception("AppSettings Token is null");

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Token valid for 1 hour
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
