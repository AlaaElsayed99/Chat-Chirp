using System.Text.Json;

namespace API.Data.SeedData
{
    public class Seed
    {
        public static async Task SeedUsers(AppDbContext _context)
        {
            if (!_context.AppUsers.Any())
            {
                var userData = await File.ReadAllTextAsync("Data/SeedData/UserSeedData.json");
                
                var User = JsonSerializer.Deserialize<List<AppUser>>(userData);
                foreach (var user in User)
                {
                    using var hmac = new HMACSHA512();
                    user.UserName = user.UserName.ToLower();
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                    user.PasswordSalt = hmac.Key;
                    _context.AppUsers.AddRange(user);
                }
                _context.AppUsers.AddRange(User);
            }
            if ( _context.ChangeTracker.HasChanges())
                await _context.SaveChangesAsync();

        }
    } 
}
