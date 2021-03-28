using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace StudentApiV2.Controllers
{
    [EnableCors("any")]
    public class BaseController : ControllerBase
    {
    }
}
