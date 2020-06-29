using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APP.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController
           (
               IServiceProvider serviceProvider
           ) : base(serviceProvider)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}