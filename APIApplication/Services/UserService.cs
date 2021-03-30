using APIApplication.Helpers;
using APIEntity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIApplication.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetAll();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetById(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool ValidateToken(string token);
    }
    /// <summary>
    /// 
    /// </summary>
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Id = 1, Firstname = "Test", Lastname = "User", Username = "test", Password = "test" },
            new User { Id = 2, Firstname = "Test1", Lastname = "User1", Username = "test1", Password = "test1" },
            new User { Id = 3, Firstname = "Test2", Lastname = "User2", Username = "test2", Password = "test2" }
        };
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSettings"></param>
        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// ValidateToken
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Returns boolean value. True if token is valid. False if token is invalid</returns>
        public bool ValidateToken(string token)
        {
            bool retrunValue = false;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var username = string.Format(jwtToken.Claims.First(x => x.Type == "username").Value);

                retrunValue = true;
            }
            catch (Exception)
            {
                retrunValue = false;
            }

            return retrunValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "Issuer",
                Audience = "Audience",
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("username", user.Firstname + "," + user.Lastname) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
