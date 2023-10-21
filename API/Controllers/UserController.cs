

using System.Security.Claims;
using API.Extensions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UserController(IUserRepository userRepository, IMapper mapper,IPhotoService photoService)
        {
           
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
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

        [HttpPut]
        public async Task<ActionResult> UpdateUserAsync(MemberUpdateDTO memberUpdateDTO)
        {

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) { return NotFound("user not Found"); }
            
                _mapper.Map(memberUpdateDTO, user);
                if(await _userRepository.SaveAllAsync()) return NoContent();
            
                return BadRequest("Faild to update");    
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhotoAsync(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null)
            {
                return NotFound();
            }

            var result = await _photoService.UploadImageAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (user.Photos.Count == 0) photo.IsMain = true;
            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync()) return Ok(_mapper.Map<PhotoDTO>(photo));
            return BadRequest("Problem addig photo");

        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user =await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("this is main photo");
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Problem setting on main");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhotoAsync(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("this is main photo");
            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletionPhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if(await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("problem deletion photo");
        }


    }
}
