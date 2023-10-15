namespace API.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AppUser>> GetAccounts();

    }
}
