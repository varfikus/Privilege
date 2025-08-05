using Microsoft.EntityFrameworkCore;
using PrivilegeAPI.Context;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Helpers;
using PrivilegeAPI.Interfaces;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using System;

namespace PrivilegeAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CollectionResult<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Login = u.Login,
                    Password = u.Password
                }).ToListAsync();

            return new CollectionResult<UserDto> { Data = users };
        }

        public async Task<BaseResult<UserDto>> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return new BaseResult<UserDto> { ErrorCode = 1, ErrorMessage = "User not found" };

            return new BaseResult<UserDto>
            {
                Data = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Login = user.Login,
                    Password = user.Password
                }
            };
        }

        public async Task<BaseResult<UserDto>> CreateAsync(UserDto dto)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Login == dto.Login);
            if (existing != null)
            {
                return new BaseResult<UserDto> { ErrorCode = 1, ErrorMessage = "Login already exists" };
            }

            var user = new User
            {
                Name = dto.Name,
                Login = dto.Login,
                Password = HashPasswordHelper.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            dto.Id = user.Id;
            dto.Password = "";
            return new BaseResult<UserDto> { Data = dto };
        }

        public async Task<BaseResult<UserDto>> UpdateAsync(UserDto dto)
        {
            var user = await _context.Users.FindAsync(dto.Id);
            if (user == null)
                return new BaseResult<UserDto> { ErrorCode = 1, ErrorMessage = "User not found" };

            user.Name = dto.Name;
            user.Login = dto.Login;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.Password = HashPasswordHelper.HashPassword(dto.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            dto.Password = "";
            return new BaseResult<UserDto> { Data = dto };
        }

        public async Task<BaseResult<UserDto>> DeleteAsync(UserDto dto)
        {
            var user = await _context.Users.FindAsync(dto.Id);
            if (user == null)
                return new BaseResult<UserDto> { ErrorCode = 1, ErrorMessage = "User not found" };

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new BaseResult<UserDto> { Data = dto };
        }
    }
}
