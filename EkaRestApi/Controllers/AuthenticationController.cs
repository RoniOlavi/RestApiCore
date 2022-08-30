using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiCore.Services.Interfaces;
using RestApiCore.Models;

namespace RestApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        // Frontend Login
        [HttpPost]
        public ActionResult Post([FromBody] Credentials tunnukset)
        {
            var loggedUser = _authenticateService.Authenticate(tunnukset.UserName, tunnukset.Password);

            if (loggedUser == null)
                return BadRequest(new { message = "Käyttäjätunnus tai salasana on virheellinen!" });

            return Ok(loggedUser); // Palautus frontendiin (sis. vain loggedUser luokan mukaiset kentät)
        }
    }
}