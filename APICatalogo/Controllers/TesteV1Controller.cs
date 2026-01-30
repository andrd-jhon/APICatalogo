using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/teste")]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TesteV1Controller : ControllerBase
    {

        [HttpGet]
        public string Index()
        {
            return "teste api v1";
        }
    }
}
