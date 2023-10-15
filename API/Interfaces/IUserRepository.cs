namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        void SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUserAsync();
        Task <AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string name);
        Task<IEnumerable<AppUserDTO>> GetAllUserAsync();
        Task<AppUserDTO> GetMemberAsync(string name);
    }
}
