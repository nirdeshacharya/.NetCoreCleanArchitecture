using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APP.Services;
using Microsoft.AspNetCore.Authorization;

namespace APP.Web.Controllers
{
    [Route("api/[controller]")]
    public class UserLoginDetailController : Controller
    {
        private readonly IUserLoginDetailService userLoginDetailService;

        public UserLoginDetailController(IUserLoginDetailService userLoginDetailService)
        {
            this.userLoginDetailService = userLoginDetailService;
        }

        public IActionResult Index(long userId)
        {
            var userLoginDetails = userLoginDetailService.GetUserLoginDetail(userId).OrderByDescending(x => x.CreatedDate);

            return Ok(userLoginDetails.Select(x => x.CreatedDate.ToString("M/dd/yyy hh:mm tt")));
        }
    }
}