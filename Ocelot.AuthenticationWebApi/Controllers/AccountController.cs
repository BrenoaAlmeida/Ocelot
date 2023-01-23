using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace Ocelot.AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtTokenManager _jwtTokenManager;
        public AccountController(JwtTokenManager jwtTokenManager)
        {
            _jwtTokenManager = jwtTokenManager;
        }

        [HttpPost]
        public ActionResult<AuthenticationResponse?> Authenticate([FromBody] AuthenticationRequest authenticationRequest) 
        {
            var authenticationResponse = _jwtTokenManager.GenerateJwtTokenWithSymmetricKey(authenticationRequest);
            //var authenticationResponse = _jwtTokenManager.GenerateTokenAsymmetric();
            if (authenticationResponse == null)
            {
                return Unauthorized();
            }
            return authenticationResponse;
        }

        //[HttpGet]
        //public ActionResult<object> GetToken() {
        //    var client = new HttpClient()
        //    {
        //        BaseAddress = new Uri("https://hom.id.ms.gov.br/auth/realms/ms/protocol/openid-connect/token"),
        //        Timeout = TimeSpan.FromMinutes(5) //default is 90 seconds
        //    };

        //    //client.DefaultRequestHeaders.Add("grant_type", "client_credentials");
        //    //client.DefaultRequestHeaders.Add("client_id", "efronteiras");
        //    //client.DefaultRequestHeaders.Add("client_secret", "142ccbb2-2634-4887-b151-ca73e966317e");
        //    //
        //    //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");            

        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        //    //req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        //    //access_token: string;
        //    //expires_in: number;
        //    //expiration_date: string;
        //    //refresh_expires_in: number;
        //    //refresh_token: string;
        //    //token_type: string;
        //    //notbeforepolicy: number;
        //    //session_state: string;

        //    //var body = new StringContent("{{\"access_token\":{{\"expires_in\":{{\"expiration_date\":refresh_expires_in,\"refresh_token\":}}", Encoding.UTF8, "text/plain");
        //    var body = new StringContent("grant_type=client_credentials&client_id=efronteiras&client_secret=142ccbb2-2634-4887-b151-ca73e966317e");
        //    var response = client.PostAsync("https://hom.id.ms.gov.br/auth/realms/ms/protocol/openid-connect/token", body);

        //    return new JsonContent { ObjectType = "JSON" , Value = response};
        //}
    }
}
