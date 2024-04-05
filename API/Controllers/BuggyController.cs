using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    private readonly IFileService _fileService;

    public BuggyController(IFileService fileService)
    {
        _fileService = fileService;
    }
    
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

    [HttpPost("upload-file")]
    public async Task<ActionResult> UploadFile(IFormFile formFile)
    {
        var guid = Guid.NewGuid();
        var fileName = guid.ToString() + Path.GetExtension(formFile.FileName);

        using var content = new MemoryStream();
        formFile.CopyTo(content);

        await _fileService.UploadFromContentAsync(fileName, content.ToArray());

        return NoContent();
    }

    [HttpGet("download-file/{fileName}")]
    public async Task<ActionResult> DownloadFile(string fileName)
    {
        return File(await _fileService.DownloadToContent(fileName), "application/octet-stream");
    }
}
