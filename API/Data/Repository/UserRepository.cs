using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppUserDTO>> GetAllUserAsync()
        {
            return await _context.AppUsers
                .ProjectTo<AppUserDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<AppUserDTO> GetMemberAsync(string name)
        {
            return await _context.AppUsers.Where(s => s.UserName == name)
                .ProjectTo< AppUserDTO >(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUserAsync()
        {
            return await _context.AppUsers.Include(s=>s.Photos).ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.AppUsers.Include(s => s.Photos).SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string name)
        {
            return await _context.AppUsers.Include(s => s.Photos).SingleOrDefaultAsync(s => s.UserName == name);
        }

        public async Task<bool> SaveAllAsync()
        {
          return  await _context.SaveChangesAsync()>0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State= EntityState.Modified;
        }
    }
}
