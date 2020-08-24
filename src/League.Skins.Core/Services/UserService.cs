using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using League.Skins.Core.Mappers;
using League.Skins.Data.Models;
using League.Skins.Data.Repositories;
using League.Skins.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace League.Skins.Core.Services
{
    public static class UserServiceServiceExtensions
    {
        public static void RegisterUserServices(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<UserMapper>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<UserService>();
        }
    }

    public class UserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserRepository _userRepository;
        private readonly UserMapper _userMapper;

        public UserService(IPasswordHasher<User> passwordHasher, UserRepository userRepository, UserMapper userMapper)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _userMapper = userMapper;
        }

        public async Task<ServiceResponse> Login(UserLoginRequest loginRequest)
        {
            var user = await _userRepository.GetByLogin(loginRequest.Login);
            if (user == null)
                return ServiceResponse.Error(ErrorServiceCodes.WrongLoginOrPassword, "User not found.");

            var verificationResult =
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);
            switch (verificationResult)
            {
                case PasswordVerificationResult.Failed:
                    return ServiceResponse.Error(ErrorServiceCodes.WrongLoginOrPassword, "Wrong password.");
                case PasswordVerificationResult.Success:
                    return ServiceResponse<UserResponse>.Ok(_userMapper.ToResponse(user));
                case PasswordVerificationResult.SuccessRehashNeeded:
                    user.PasswordHash = _passwordHasher.HashPassword(user, loginRequest.Password);
                    await _userRepository.Update(user);
                    goto case PasswordVerificationResult.Success;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<ServiceResponse> Register(UserRegisterRequest registerRequest)
        {
            if (await _userRepository.GetByLogin(registerRequest.Login) != null)
                ServiceResponse.Error(ErrorServiceCodes.EntityAlreadyExists, "User with given login already exists.");

            var user = new User
            {
                Login = registerRequest.Login, CreationTime = DateTime.UtcNow, Editors = new List<User>(),
                Editable = new List<User>()
            };
            var hash = _passwordHasher.HashPassword(user, registerRequest.Password);
            user.PasswordHash = hash;
            await _userRepository.Create(user);
            return ServiceResponse.Ok();
        }

        public async Task<ServiceResponse> Relate(string userId, UserRelateRequest relateRequest)
        {
            var user = await _userRepository.GetById(userId);
            var editor = await _userRepository.GetByLogin(relateRequest.Login);
            if (editor == null)
                return ServiceResponse.Error(ErrorServiceCodes.EntityNotFound, $"User {relateRequest.Login} couldn't be found.");
            if (user.Id == editor.Id)
                return ServiceResponse.Error(ErrorServiceCodes.SelfRelation, $"You cannot relate user {userId} to himself.");
            
            user.Editors.Add(editor);
            editor.Editable.Add(user);
            await _userRepository.Update(user);
            await _userRepository.Update(editor);

            return ServiceResponse<UserResponse>.Ok(_userMapper.ToResponse(user));
        }
    }
}
