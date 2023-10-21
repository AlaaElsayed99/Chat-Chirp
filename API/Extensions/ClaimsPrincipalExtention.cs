using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtention
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var username = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return username;
        }

    }
}
