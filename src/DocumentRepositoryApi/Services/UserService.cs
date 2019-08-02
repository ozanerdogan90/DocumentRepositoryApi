using DocumentRepositoryApi.DataAccess.Repositories;
using DocumentRepositoryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Services
{
    public interface IUserService
    {
        Task<bool> Register(User user);
        Task<User> Get(string email);
    }

    public class UserService : IUserService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepository;
        public UserService(IEncryptionService encryptionService, IUserRepository userRepository)
        {
            _encryptionService = encryptionService;
            _userRepository = userRepository;
        }

        public async Task<bool> Register(User user)
        {
            var userEntity = AutoMapper.Mapper.Map<DataAccess.Entities.User>(user);
            userEntity.Password = _encryptionService.Encrypt(user.Password);

            return await _userRepository.Add(userEntity);
        }

        public async Task<User> Get(string email)
        {
            var userEntity = await _userRepository.Get(email);
            if (userEntity == null)
                return null;

            var user = AutoMapper.Mapper.Map<User>(userEntity);
            user.Password = _encryptionService.Decrypt(userEntity.Password);
            return user;
        }
    }
}
