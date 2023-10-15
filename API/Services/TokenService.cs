

using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.Services
{
    public class TokenService:ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration config;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
            this.config = config;
        }


        public string CreateToken(AppUser User)
        {
           var claims=new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, User.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, User.Id.ToString()));
            
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);
            JwtSecurityToken token = new JwtSecurityToken(
                           issuer: config["JWT:ValidIssuer"],/// The api location 
                           audience: config["JWT:ValidAduiance"],///The angular consumer
                           claims: claims,
                           expires: DateTime.UtcNow.AddHours(1),
                           signingCredentials: creds
                            );
            var Token = new JwtSecurityTokenHandler().WriteToken(token);
            return Token;
        }
    }
}
