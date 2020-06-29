using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using APP.Data;
using APP.Repo;
using System.Linq;

namespace APP.Services
{
    public class UserLoginDetailService : IUserLoginDetailService
    {
        private readonly IRepository<UserLoginDetail> userLoginDetailRepository;

        public UserLoginDetailService(IRepository<UserLoginDetail> userLoginDetailRepository)
        {
            this.userLoginDetailRepository = userLoginDetailRepository;
        }

        public long AddUserLoginDetail(UserLoginDetail userLoginDetail)
        {
            return userLoginDetailRepository.Insert(userLoginDetail);
        }

        public Task<long> AddUserLoginDetailAsync(UserLoginDetail userLoginDetail)
        {
            return Task.Run(() =>
            {
                return userLoginDetailRepository.Insert(userLoginDetail);
            });
        }

        public List<UserLoginDetail> GetUserLoginDetail(long userId)
        {
            var userLoginDetails = userLoginDetailRepository
                                        .GetAll()
                                        .Where(x => x.UserId.Equals(userId));

            return userLoginDetails.ToList();
        }
    }
}
