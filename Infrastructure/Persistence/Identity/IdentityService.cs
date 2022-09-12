using Application.Interfaces.Identity;
using Application.Interfaces.Repositories;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;


        public IdentityService(IHttpContextAccessor context, IUserRepository userRepository, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
            _passwordHasher = passwordHasher ?? throw new ArgumentException(nameof(passwordHasher));
            _userRepository = userRepository;
        }

        public string GetUserIdentity()
        {
            return _context.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
        }


        public string GenerateToken(User user, IEnumerable<string> roles = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtTokenSettings:TokenKey")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            IList<Claim> claims = new List<Claim>
            {
                //new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
            if(roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            var token = new JwtSecurityToken(_configuration.GetValue<string>("JwtTokenSettings:TokenIssuer"),
                _configuration.GetValue<string>("JwtTokenSettings:TokenIssuer"),
                claims,
                DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration.GetValue<string>("JwtTokenSettings:TokenExpiryPeriod"))),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public JwtSecurityToken GetClaims(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                /*if (token.StartsWith("B"))
                {
                    token = token.Split(" ")[1];
                }*/
                var handler = new JwtSecurityTokenHandler();

                var decodedToken = handler.ReadToken(token) as JwtSecurityToken;

                return decodedToken;
            }
            return null;
        }

        public string GetClaimValue(string type)
        {

            return _context.HttpContext.User.FindFirst(type).Value;

        }

        public string GetUniqueKey(int size)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            using var crypto = RandomNumberGenerator.Create();
            byte[] data = new byte[size];
            crypto.GetBytes(data);
            /*using (var crypto = new RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }*/
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public string GetPasswordHash(string password, string salt = null)
        {
            if (string.IsNullOrEmpty(salt))
            {
                return _passwordHasher.HashPassword(new User(), password);
            }
            return _passwordHasher.HashPassword(new User(), $"{password}{salt}");
        }

        public string GenerateSalt()
        {
            using var crypto = RandomNumberGenerator.Create();
            byte[] buffer = new byte[10];
            crypto.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            //var user = await _gateway.GetUserAsync(userName);
            var user = await _userRepository.GetAsync(u => u.UserName == userName);
            if (user == null)
            {
                return null;
            }
            //return user;
            return new User
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<User> FindUserAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            var user = await _userRepository.GetAsync(u => u.UserName == userName);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var roles = await _userRepository.GetUserAndRoles(user.UserName);

            return roles.UserRoles.Select(role => role.Role.Name ).ToList();
        }

        public bool CheckPasswordAsync(User user, string password)
        {
            var hashPassword = HashPasswordAsync(password);
            if (user.Password == hashPassword)
            {
                return true;
            }
            return false;
        }

        private string HashPasswordAsync(string password)
        {
            using (var md5Hash = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = md5Hash.ComputeHash(sourceBytes);
                var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return hash.ToLower();
            }
        }

    }
}
