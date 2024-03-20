using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("not-found")]
    public ActionResult GetNotFound()
    {
        return NotFound("user not found");
    }

    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest();
    }

    [HttpGet("unauthorized")]
    public ActionResult GetUnathorized()
    {
        return Unauthorized();
    }

    [HttpGet("forbidden")]
    public ActionResult GetForbidden()
    {
        return Forbid();
    }

    [HttpGet("server-error")]
    public ActionResult GetServerError()
    {
        throw new Exception();
    }
}
