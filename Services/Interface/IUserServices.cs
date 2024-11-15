using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserServices
    {
        Task<bool> AddUserAsync(string email, string fullName, string userName, string password);
        Task<bool> ConfirmUserAsync(string email, string code);
        Task<User> GetUserByEmail(string email);
        Task<User> AuthenticateAsync(string email, string password);
        Task<User> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(UpdateUserDTO updateUserDTO, string userId);
        Task<List<User>> GetAllUsers();
        Task<bool> UpdateUserStatusAsync(string ustatus, string userId);
        Task<bool> AddUserWithoutRegisterAsync(string email, string fullName, string userName, string password, string uRole);
    }
}
