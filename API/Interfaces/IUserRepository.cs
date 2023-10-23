
namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUserAsync();
        Task <AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string name);
        Task<PageList<AppUserDTO>> GetAllUserAsync(Userparams userparams);
        Task<AppUserDTO> GetMemberAsync(string name);
    }
}
