
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Data.Repository
{
    public class AccountRepository:IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<AppUser>> GetAccounts()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllAccounts()
        {
            return await _context.AppUsers.ToListAsync();
        }
        
    }
}
