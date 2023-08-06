using Microsoft.AspNetCore.Mvc;

namespace PlatformApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}