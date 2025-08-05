using Google.Apis.Auth;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MediTrackWebAPI.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        public class GoogleTokenRequest
        {
            public string Token { get; set; }
        }

        [HttpPost]
        [Route("google")]
        public async Task<IHttpActionResult> GoogleLogin(GoogleTokenRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);

                // You can now use payload.Email, payload.Name, etc.
                // Optional: Check DB and create a local user or return JWT
                return Ok(new
                {
                    email = payload.Email,
                    name = payload.Name,
                    picture = payload.Picture,
                    sub = payload.Subject
                });
            }
            catch (InvalidJwtException)
            {
                return Content(HttpStatusCode.Unauthorized, "Invalid Google token");
            }
        }
    }
}