using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/teste")]
    [ApiController]
    [ApiVersion("2.0")]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Teste api v2";
        }
    }
}
