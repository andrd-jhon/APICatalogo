using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/teste")]
    [ApiController]
    [ApiVersion(3)]
    [ApiVersion(4)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TesteV3Controller : ControllerBase
    {
        [MapToApiVersion(3)]
        [HttpGet]
        public string GetVersionV3()
        {
            return "versão 3";
        }

        [MapToApiVersion(4)]
        [HttpGet]
        public string GetVersionV4()
        {
            return "versão 4";
        }
    }
}
