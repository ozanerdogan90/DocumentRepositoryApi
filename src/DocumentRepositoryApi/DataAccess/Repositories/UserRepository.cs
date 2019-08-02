using DocumentRepositoryApi.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<bool> Add(User user);
        Task<User> Get(string email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DocumentContext _context;
        public UserRepository(DocumentContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(User user)
        {
            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<User> Get(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
