using AutoMapper;
using DocumentRepositoryApi.DataAccess.Repositories;
using DocumentRepositoryApi.Models;
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
        private readonly IMapper _mapper;
        public UserService(IEncryptionService encryptionService, IUserRepository userRepository, IMapper mapper)
        {
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> Register(User user)
        {
            var userEntity = _mapper.Map<DataAccess.Entities.User>(user);
            userEntity.Password = _encryptionService.Encrypt(user.Password);

            return await _userRepository.Add(userEntity);
        }

        public async Task<User> Get(string email)
        {
            var userEntity = await _userRepository.Get(email);
            if (userEntity == null)
                return null;

            var user = _mapper.Map<User>(userEntity);
            user.Password = _encryptionService.Decrypt(userEntity.Password);
            return user;
        }
    }
}
