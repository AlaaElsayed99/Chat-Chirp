


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        public ITokenService _service;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(ITokenService service, IMapper mapper,UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpPost("Register")]

        public async Task<ActionResult<UserDTO>> Register(RegisterDTO Dto)
        {
            if (await UserExist(Dto.Username))
            {
                return BadRequest("Username already exist");
            }

            var user = _mapper.Map<AppUser>(Dto);
            //using var hmac = new HMACSHA512();
            //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Dto.Password));
            //user.PasswordSalt = hmac.Key;

            user.UserName = Dto.Username;

            var result =await _userManager.CreateAsync(user,Dto.Password);

            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            //var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            //if(!roleResult.Succeeded) return BadRequest(roleResult.Errors);
            //_context.AppUsers.Add(user);
            //await _context.SaveChangesAsync();
            return Ok(new UserDTO
            {
                Username=user.UserName,
                token =await _service.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender= user.Gender,
            });
        }
        [HttpGet]
        public async Task<bool> UserExist(string Username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == Username.ToLower());
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO Dto)
        {
            var user = _userManager.Users.Include(s=>s.Photos).FirstOrDefault(x => x.UserName == Dto.Username);
            if (user == null)
            {
                return Unauthorized("invalid Username");
            }
            var result =await _userManager.CheckPasswordAsync(user, Dto.Password);
            if (!result) return Unauthorized("error login");
            //using var hmac = new HMACSHA512(user.PasswordSalt);
            //var computedHashed = hmac.ComputeHash(Encoding.UTF8.GetBytes(Dto.Password)); 
            //for(int i=0; i < computedHashed.Length; i++)
            //{
            //    if (computedHashed[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            //}

            return Ok(new UserDTO
            {
                Username = user.UserName,
                token = await _service.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(s=>s.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender=user.Gender,
            });
        }

    }
}
