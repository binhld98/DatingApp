﻿using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
    {
        _context = context;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await _UserExists(registerDto.UserName))
        {
            return BadRequest("UserName is taken");
        }

        using var hmac = new HMACSHA512();

        var user = _mapper.Map<AppUser>(registerDto);
        user.UserName = registerDto.UserName.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PaswordSalt = hmac.Key;

        _context.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            UserName = user.UserName,
            AccessToken = _tokenService.CreateToken(user),
            PhotoUrl = null,
            KnownAs = user.KnownAs,
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

        if (user == null)
        {
            return Unauthorized("User not found");
        }

        using var hmac = new HMACSHA512(user.PaswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password");
            }
        }

        return new UserDto
        {
            UserName = user.UserName,
            AccessToken = _tokenService.CreateToken(user),
            PhotoUrl = user.Photos?.FirstOrDefault(x => x.isMain)?.Url,
            KnownAs = user.KnownAs,
        };
    }

    private async Task<bool> _UserExists(string userName)
    {
        return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
    }
}
