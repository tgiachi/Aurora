using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using Aurora.Api.MethodEx;
using Aurora.Turbine.Api.Data;
using Aurora.Turbine.Api.Data.Rest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Aurora.Turbine.Api.Middleware
{

    /// <summary>
    /// Custom authentication middleware
    /// </summary>
    public class IamAuthenticationMiddlewareAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly TurbineConfig _turbineConfig;
        private readonly ILogger _logger;
        public IamAuthenticationMiddlewareAttribute(TurbineConfig turbineConfig, ILogger<IamAuthenticationMiddlewareAttribute> logger)
        {
            _turbineConfig = turbineConfig;
            _logger = logger;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            var httpClient = new HttpClient();
            if (string.IsNullOrEmpty(context.HttpContext.Request.Headers.Authorization))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var stringToken = context.HttpContext.Request.Headers.Authorization[0][("Bearer".Length + 1)..];
            var token = new JwtSecurityToken(stringToken);

            if (token.Payload.Exp <= DateTime.Now.GetMills())
            {
                context.Result = new UnauthorizedResult();
                _logger.LogWarning("Token expired for {User}", context.HttpContext.Request.Host);
                return;
            }

            if (string.IsNullOrEmpty(_turbineConfig.IamAuthorizationUrl))
            {
                context.Result = new ObjectResult(new { Message = "IamAuthorizationUrl is empty" }) { StatusCode = 500 };
                return;
            }

            try
            {
                var response = await httpClient.PostAsJsonAsync(_turbineConfig.IamAuthorizationUrl + "/auth/verify",
                    new IamVerifyData() { Token = stringToken });

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    context.Result = new UnauthorizedResult();
                    _logger.LogWarning("Unauthorized {User}", context.HttpContext.Request.Host);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during check JWT token auth: {Err}", ex);
            }
        }
    }
}
