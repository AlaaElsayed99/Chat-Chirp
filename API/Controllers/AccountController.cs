


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ITokenService _service;
        private readonly IMapper _mapper;

        public AccountController(AppDbContext context,ITokenService service, IMapper mapper)
        {
            _context = context;
            _service = service;
            _mapper = mapper;
        }
        [HttpPost("Register")]

        public async Task<ActionResult<UserDTO>> Register(RegisterDTO Dto)
        {
            if (await UserExist(Dto.Username))
            {
                return BadRequest("Username already exist");
            }

            var user = _mapper.Map<AppUser>(Dto);
            using var hmac = new HMACSHA512();

            user.UserName = Dto.Username;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Dto.Password));
            user.PasswordSalt = hmac.Key;

            
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new UserDTO
            {
                Username=user.UserName,
                token = _service.CreateToken(user),
                KnownAs = user.KnownAs,
            });
        }
        [HttpGet]
        public async Task<bool> UserExist(string Username)
        {
            return await _context.AppUsers.AnyAsync(x => x.UserName == Username.ToLower());
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO Dto)
        {
            var user = _context.AppUsers.Include(s=>s.Photos).FirstOrDefault(x => x.UserName == Dto.Username);
            if (user == null)
            {
                return Unauthorized("invalid Username");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHashed = hmac.ComputeHash(Encoding.UTF8.GetBytes(Dto.Password)); 
            for(int i=0; i < computedHashed.Length; i++)
            {
                if (computedHashed[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }

            return Ok(new UserDTO
            {
                Username = user.UserName,
                token = _service.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(s=>s.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender=user.Gender,
            });
        }

    }
}
