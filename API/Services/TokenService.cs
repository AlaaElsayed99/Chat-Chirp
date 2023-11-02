

using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.Services
{
    public class TokenService:ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration config;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
            this.config = config;
            _userManager = userManager;
        }


        public async Task< string> CreateToken(AppUser User)
        {
            var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.NameId, User.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, User.UserName)
        };
            var roles = await _userManager.GetRolesAsync(User);

            claims.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role, role)));


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
