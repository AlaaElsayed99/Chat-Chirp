using System.Text.Json;

namespace API.Data.SeedData
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;
            {
                var userData = await File.ReadAllTextAsync("Data/SeedData/UserSeedData.json");
                
                var User = JsonSerializer.Deserialize<List<AppUser>>(userData);

                var roles = new List<AppRole>
                {
                    new AppRole{Name="Member"},
                    new AppRole{Name="Admin"},
                    new AppRole{Name="Moderator"},

                };
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
                foreach (var user in User)
                {
                    user.UserName = user.UserName.ToLower();

                    //using var hmac = new HMACSHA512();
                    //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                    //user.PasswordSalt = hmac.Key;
                    await userManager.CreateAsync(user,"Pa$$w0rd");
                    await userManager.AddToRoleAsync(user,"Member");
                }
                var admin = new AppUser
                {
                    UserName = "admin"
                };
                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});

            }
          

        }
    } 
}
