using pelsoft.auth.common;
using pelsoft.auth.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace pelsoft.auth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticator _authenticator;
        private readonly string _defaultSecurityProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;

        public AuthenticationController(IAuthenticator authenticator, IRefreshTokenProvider refreshTokenProvider, string defaultSecurityProvider)
        {
            _authenticator = authenticator;
            _defaultSecurityProvider = defaultSecurityProvider;
            _refreshTokenProvider = refreshTokenProvider;
        }

        public AuthenticationController(IAuthenticator authenticator, IRefreshTokenProvider refreshTokenProvider) : this(authenticator, refreshTokenProvider, null)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Retorna JWT</returns>
        [HttpPost]
        [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult auth(AuthenticationRequest req)
        {
            try
            {
                if (req.grant_type == "refresh_token")
                {
                    var tokenData = _refreshTokenProvider.FetchToken(req.refresh_token);

                    var user = _authenticator.FetchUser(req, tokenData.UserID);

                    var jwt = TokenGenerator.GenerateToken(user.Claims, req.security_provider ?? _defaultSecurityProvider);
                    var refreshTokenString = _refreshTokenProvider.RefreshToken(tokenData, IpAddress()).Token;

                    var res = new AuthenticationResponse
                    {
                        token = jwt,
                        refresh_token = refreshTokenString
                    };

                    return Ok(res);
                }
                else if (req.grant_type == null || req.grant_type == "password")
                {
                    var user = _authenticator.Authenticate(req);

                    var jwt = TokenGenerator.GenerateToken(user.Claims, req.security_provider ?? _defaultSecurityProvider);
                    var refreshTokenString = _refreshTokenProvider.GenerateRefreshToken(user.UserId, IpAddress()).Token;

                    var res = new AuthenticationResponse
                    {
                        token = jwt,
                        refresh_token = refreshTokenString
                    };

                    return Ok(res);
                }

                return BadRequest("grant_type no soportado");
            }
            catch (AuthorizationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
