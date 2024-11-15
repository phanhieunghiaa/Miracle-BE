using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Data.Entity;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        #region Register User
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var temp = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return temp;
        }

        public async Task<User> GetUserById(string userId)
        {
           return await _context.Users.FirstOrDefaultAsync(sc =>sc.Id.Equals(userId));
        }
        #endregion

        #region Sub code

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
