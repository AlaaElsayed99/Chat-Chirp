


using AutoMapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
           
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDTO>>> GetAsync()
        {
            var Users = await _userRepository.GetAllUserAsync();
            return Ok(Users);

        }
        //[HttpGet("{id}")]
        //public async Task<ActionResult<AppUserDTO>> GetByIdAsync(int id)
        //{
        //    var User = await _userRepository.GetUserByIdAsync(id);
           
        //    if (User == null)
        //    {
        //        return NotFound();
        //    }
        //    var data = _mapper.Map<IEnumerable<AppUserDTO>>(User);
        //    return Ok(data);
        //}
        [HttpGet("{name}")]
        public async Task<ActionResult<AppUserDTO>> GetByUsernameAsync(string name)
        {
            var User = await _userRepository.GetMemberAsync(name);

            if (User == null)
            {
                return NotFound();
            }
            
            return Ok(User);
        }

    }
}
