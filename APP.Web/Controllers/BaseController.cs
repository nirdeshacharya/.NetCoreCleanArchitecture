using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using APP.Data;
using APP.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace APP.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<User> userManager;
        protected readonly RoleManager<Role> roleManager;
        protected readonly IUserService userService;
        protected readonly IServiceProvider serviceProvider;
        protected User loggedInUser;

        public BaseController
            (
                IServiceProvider serviceProvider
            )
        {
            this.serviceProvider = serviceProvider;
            this.userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            this.userService = serviceProvider.GetRequiredService<IUserService>();
            this.roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(User.Identity.IsAuthenticated)
            {
               
                   
            }
        }
    }
}
