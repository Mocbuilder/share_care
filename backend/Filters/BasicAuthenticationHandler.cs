using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using sharecare_backend.Services;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using sharecare_backend.Models.User;

namespace sharecare_backend.Filters
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DbService _dbService;
        public BasicAuthenticationHandler(DbService db,
                IOptionsMonitor<AuthenticationSchemeOptions> options,
                ILoggerFactory logger,
                UrlEncoder encoder) : base(options, logger, encoder)
        {
            _dbService = db;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (!"Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
                    return AuthenticateResult.Fail("Invalid Authorization Scheme");

                if (string.IsNullOrEmpty(authHeader.Parameter))
                    return AuthenticateResult.Fail("Missing Credentials");

                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

                var username = credentials[0];
                var password = credentials[1];

                if (await IsAuthenticated(username, password))
                {
                    // If valid, create the user identity/claims principal
                    var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Name, username)
                    };

                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }

                return AuthenticateResult.Fail("Invalid Username or Password");
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header Format");
            }
        }

        public async Task<bool> IsAuthenticated(string username, string password)
        {
            UserEntity user = await _dbService.GetUserByEmailAsync(username);
            if(user.PasswordHash == password)
                return true;
            else
                return false;
        }
    }
}
