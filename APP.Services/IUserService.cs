using APP.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APP.Services
{
    public interface IUserService
    {
        Task<IdentityResult> AddUser(User user, string password);
        Task<User> GetUserByUserName(string userName);
        Task<User> GetUserByEmail(string email);
        void UpdateUser(User user);
        Task UpdateUserAsync(User user);
        Task<User> GetUser(long id);
        Task<List<User>> GetUsersAsync();
        Task<IList<User>> GetUsersByRole(string roleName);
        List<User> GetWhere(Expression<Func<User, bool>> predicate);
        Task<User> UpdatePasswordAsync(long userId, string password);
    }
}
