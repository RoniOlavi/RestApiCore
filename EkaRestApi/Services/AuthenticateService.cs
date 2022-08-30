using RestApiCore.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Text;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using RestApiCore.Services.Interfaces;

namespace RestApiCore.Services

{
    public class AuthenticateService : IAuthenticateService
    {

        private readonly NorthwindOriginalContext db;

        private readonly AppSettings _appSettings;
        public AuthenticateService(IOptions<AppSettings> appSettings, NorthwindOriginalContext nwoc)
        {
            _appSettings = appSettings.Value;
            db = nwoc;
        }


        //Metodin paluutyyppi on LoggedUser luokan mukainen olio
        public LoggedUser? Authenticate(string username, string password)
        {

            var foundUser = db.Users.SingleOrDefault(x => x.UserName == username && x.Password == password);

            // Jos ei käyttäjää löydy palautetaan null
            if (foundUser == null)
            {
                return null;
            }

            // Jos käyttäjä löytyy:
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, foundUser.UserId.ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Version, "V3.1")
                }),
                Expires = DateTime.UtcNow.AddDays(1), // Montako päivää token on voimassa

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoggedUser loggedUser = new LoggedUser();

            loggedUser.UserName = foundUser.UserName;
            loggedUser.AccesslevelId = foundUser.AccesslevelId;
            loggedUser.Token = tokenHandler.WriteToken(token);

            return loggedUser; // Palautetaan kutsuvalle controllerimetodille user ilman salasanaa
        }

    }
}