using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserById(string userId);
        Task<List<User>> GetAllUsers();
        Task AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(User user);
    }
}
