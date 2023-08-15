using System.Security.Claims;

namespace FirstProject___Test.Helpful
{
    public static class UserTokenHelper
    {
        public static Guid GetCurrentUserToken(HttpContext httpContext)
        {
            var claim = httpContext.User.Claims;
            var userTokenClaim = claim.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userTokenClaim != null && Guid.TryParse(userTokenClaim.Value, out Guid userToken))
            {
                return userToken;
            }
            return Guid.Empty;
        }
    }
}
