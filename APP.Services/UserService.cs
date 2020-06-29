using System;
using System.Collections.Generic;
using System.Text;
using APP.Data;
using APP.Data.Contexts;
using APP.Repo;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace APP.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext context;
        private readonly UserManager<User> userManager;
        private DbSet<User> entities;
        string errorMessage = string.Empty;

        public UserService(ApplicationContext context, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.context = context;
            entities = context.Set<User>();
        }

        public async Task<IdentityResult> AddUser(User user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (null == user || user.DeletedDate != null)
            {
                throw new KeyNotFoundException();
            }

            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (null == user || user.DeletedDate != null)
            {
                throw new KeyNotFoundException();
            }

            return user;
        }

        public async Task<User> GetUser(long id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (null == user || user.DeletedDate != null)
            {
                throw new KeyNotFoundException();
            }

            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var result= Task.Run(()=> userManager.Users.Where(x => x.DeletedDate == null).ToList());
            return await result;
        }

        public async Task<IList<User>> GetUsersByRole(string roleName)
        {
            var users = await userManager.GetUsersInRoleAsync(roleName);

            return users.Where(x => x.DeletedDate == null).ToList();
        }

        public List<User> GetWhere(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            context.SaveChanges();
        }

        public Task UpdateUserAsync(User user)
        {
            return context.SaveChangesAsync();
        }


        public async Task<User> UpdatePasswordAsync(long userId, string password)
        {
            var user = await this.GetUser(userId);

            if(user == null )
            {
                throw new NullReferenceException("User does not exists");
            }

            string resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var changePasswordResult = await userManager.ResetPasswordAsync(user, resetToken, password);

            if (!changePasswordResult.Succeeded)
            {
                throw new Exception("Cannot update password");                
            }

            return user;
        }
    }
}
