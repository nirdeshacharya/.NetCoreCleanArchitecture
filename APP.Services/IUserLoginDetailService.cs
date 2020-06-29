using APP.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APP.Services
{
    public interface IUserLoginDetailService
    {
        long AddUserLoginDetail(UserLoginDetail userLoginDetail);
        Task<long> AddUserLoginDetailAsync(UserLoginDetail userLoginDetail);
        List<UserLoginDetail> GetUserLoginDetail(long userId);
    }
}
