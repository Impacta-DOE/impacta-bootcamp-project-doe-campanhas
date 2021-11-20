using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.Entities.DoeAuth;
using impacta.bootcamp.project_doe.campanhas.infra.data.ExternalServices.DoeAuth.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace impacta.bootcamp.project.doe.campanhas.api.Helpers
{
    public class BearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>

    {
        private readonly IAuthServices _authServices;
        public BearerAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAuthServices authServices) :base(options,logger,encoder,clock)

        {
            _authServices = authServices;
        }

        protected override async  Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var endpont = Context.GetEndpoint();
                if(endpont?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                {
                    return AuthenticateResult.NoResult();
                }

                if(!Request.Headers.ContainsKey("Authorization"))
                {
                    return AuthenticateResult.Fail("Sem Header de authorizacao");

                }

                UserAuth user = null;


                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                    var token = authHeader.Parameter;

                    user = await _authServices.validate(token);
            

                if(user == null)
                {
                    AuthenticateResult.Fail("Falha na autenticacao de usuario");
                }


                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, user.userName)
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);


                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                AuthenticateResult.Fail("Falha na autenticacao de usuario");

            }

            return null;

            
        }
    }
}
