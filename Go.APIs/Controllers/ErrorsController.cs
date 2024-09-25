using Go.APIs.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Go.APIs.Controllers
{
    [ApiController]
    [Route("Errors/{Code}")]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult error(int Code)
        {
            switch (Code)
            {
                case 404:
                    return NotFound(new APIResponce(404));
                case 401:
                    return BadRequest(new APIResponce(401));
            }
            return StatusCode(Code);
        }
     }
}
