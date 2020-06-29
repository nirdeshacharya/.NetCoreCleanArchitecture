using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP.Services
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
           
            services.AddTransient<IUserService, UserService>();
          
            services.AddTransient<IUserLoginDetailService, UserLoginDetailService>();
           
            return services;
        }
    }
}
