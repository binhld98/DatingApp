using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        return Ok(await _userRepository.GetMembersAsync());
    }

    [HttpGet("{userName}")]
    public async Task<ActionResult<MemberDto>> GetUser(string userName)
    {
        return await _userRepository.GetMembersByUserNameAsync(userName);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());

        if (user == null)
        {
            return NotFound();
        }

        _mapper.Map(memberUpdateDto, user);

        if (await _userRepository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Failed to update user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());

        if (user == null)
        {
            return NotFound();
        }

        // validate file
        if (
            string.IsNullOrEmpty(file.FileName) ||
            string.IsNullOrEmpty(Path.GetExtension(file.FileName))
        )
        {
            return BadRequest("Invalid file name");
        }

        string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
        if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
        {
            return BadRequest("Unsupported file type");
        }

        if (file.Length > 1024 * 1024)
        {
            return BadRequest("File size must be less than 1MB");
        }

        string[] allowedMineTypes = new string[] { "image/jpeg", "image/png", "image/gif" };

        if (!allowedMineTypes.Contains(file.ContentType))
        {
            return BadRequest("Unsupported mine types");
        }

        string url = await _photoService.AddPhotoAsync(file);

        var photo = new Photo()
        {
            Url = url,
            PublicId = Path.GetFileName(url),
            isMain = false
        };

        if (user.Photos.Count == 0)
        {
            photo.isMain = true;
        }

        user.Photos.Add(photo);

        if (await _userRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetUser), new { userName = user.UserName }, _mapper.Map<PhotoDto>(photo));
        }

        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());
        if (user == null)
        {
            return NotFound();
        }

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null)
        {
            return NotFound();
        }

        if (photo.isMain)
        {
            return BadRequest("This is already your main photo");
        }

        var currentMain = user.Photos.FirstOrDefault(x => x.isMain);
        if (currentMain != null)
        {
            currentMain.isMain = false;
        }

        photo.isMain = true;

        if (await _userRepository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Prolem when set main photo");
    }


    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());
        if (user == null)
        {
            return NotFound();
        }

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null)
        {
            return NotFound();
        }

        if (photo.isMain)
        {
            return BadRequest("Cannot delete main photo");
        }

        await _photoService.DeletePhotoAsync(photo.PublicId);

        user.Photos.Remove(photo);

        if (await _userRepository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Prolem when delete photo");
    }
}
