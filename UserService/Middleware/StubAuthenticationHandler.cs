using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

public class StubAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public StubAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
        // Ensure TimeProvider is set in AuthenticationSchemeOptions
        if (options.CurrentValue.TimeProvider == null)
        {
            throw new ArgumentNullException(nameof(options.CurrentValue.TimeProvider), "TimeProvider must be set in AuthenticationSchemeOptions.");
        }
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.User?.Identity?.IsAuthenticated == true)
        {
            return Task.FromResult(AuthenticateResult.Success(
                new AuthenticationTicket(Context.User, "Stub")));
        }
        return Task.FromResult(AuthenticateResult.Fail("No stub user authenticated"));
    }
}
