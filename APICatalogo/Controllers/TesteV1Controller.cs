using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/teste")]
    [ApiVersion("1.0")]
    public class TesteV1Controller : ControllerBase
    {

        [HttpGet]
        public string Index()
        {
            return "teste api v1";
        }
    }
}
