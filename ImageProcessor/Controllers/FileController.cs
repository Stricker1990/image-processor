using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessor.Controllers
{
    [ApiController]
    [Route("files")]
    public class FileController : ControllerBase
    {
        [HttpPost]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
