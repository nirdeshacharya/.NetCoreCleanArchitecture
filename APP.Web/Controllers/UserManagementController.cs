using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APP.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using APP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using APP.Services;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace APP.Web.Controllers
{
    public class UserManagementController : BaseController
    {
        private readonly ILogger logger;

        public UserManagementController
            (
                IServiceProvider serviceProvider,
                ILogger<UserManagementController> logger
            ) : base(serviceProvider)
        {
            this.logger = logger;
        }

       

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var response = new List<dynamic>();
            var users = await userService.GetUsersAsync();
            var records = users.ToList().Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                fullName = u.FirstName + " " + u.LastName,
                u.UserName,
                u.Email,
                u.TwoFactorEnabled,
                phone = u.PhoneNumber,
                createdDate = u.CreatedDate.ToString("M/dd/yyyy", CultureInfo.InvariantCulture),
                Role = u.UserRoles.ToList().FirstOrDefault().Role.Name,
                LastLoginTime = u.LastLoginTime == null ? null : (u.LastLoginTime ?? DateTime.Now).ToString("M/dd/yyy hh:mm tt"),
            });
            return Ok(records);

        }

        [HttpGet("/api/user/{roleName}")]
        public async Task<IActionResult> GetByRoals(string roleName)
        {
            var users = await userService.GetUsersByRole(roleName);

            return Ok(users);

        }
       

        [HttpPost]
        public async Task<IActionResult> Add(UserAddViewModel model)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var roles = roleManager.Roles.Select(x => new SelectListItem { Text = x.Name, Value = x.NormalizedName }).ToList();
            roles.Insert(0, new SelectListItem { Text = "Select Role", Value = "" });
            model.Role = roles;
            if (model.Email != null)
            {
                var userCheckWithEmail = await userManager.FindByEmailAsync(model.Email);

                if(userCheckWithEmail != null && !userCheckWithEmail.DeletedDate.HasValue)
                {
                    ModelState.AddModelError("Email", "Email already used please use different.");
                }
            }

            var userCheckWithUsername = await userManager.FindByNameAsync(model.UserName);

            if (userCheckWithUsername != null)
            {
                ModelState.AddModelError("Username", "Username already Used please use different.");
            }
            

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userEntity = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                TwoFactorEnabled = model.TwoFactorAuthentication,
                EmailConfirmed = true,
                CreatedDate = DateTime.Now,
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(userEntity, model.Password);

            if (result.Succeeded)
            {

                await userManager.AddToRoleAsync(userEntity, model.RoleName);
               
            }

            return Redirect("Index");
        }

        [HttpGet("/UserManagement/Update/{userId}")]
        public async Task<IActionResult> Update(long userId)
        {
            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
                var userService = serviceProvider.GetRequiredService<IUserService>();
                var roles = roleManager.Roles.Select(x => new SelectListItem { Text = x.Name, Value = x.NormalizedName }).ToList();
                var user = await userService.GetUser(userId);
                ViewData["UserDetail"] = user;
                var userRoles = await serviceProvider.GetRequiredService<UserManager<User>>().GetRolesAsync(user);



                var model = new UserUpdateViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.PhoneNumber,
                    RoleName = userRoles.FirstOrDefault(),
                    Email = user.Email
                };

                roles.Insert(0, new SelectListItem { Text = "Select Role", Value = "" });
                model.Role = roles;


                return View(model);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex.Message, ex);
                return BadRequest();
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return BadRequest();
            }         
        }

        [HttpPost("/UserManagement/Update/{userId}")]
        public async Task<IActionResult> Update(long userId, UserUpdateViewModel model)
        {
            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
                var userService = serviceProvider.GetRequiredService<IUserService>();
                var roles = roleManager.Roles.Select(x => new SelectListItem { Text = x.Name, Value = x.NormalizedName }).ToList();
                var user = await userService.GetUser(userId);
                ViewData["UserDetail"] = user;
                var userRoles = await serviceProvider.GetRequiredService<UserManager<User>>().GetRolesAsync(user);


                roles.Insert(0, new SelectListItem { Text = "Select Role", Value = "" });
                model.Role = roles;

                if (!ModelState.IsValid)
                    return View(model);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.Phone;

                if(String.IsNullOrEmpty(user.Email))
                {
                    user.Email = model.Email;
                }

                userService.UpdateUser(user);
                var assignedRoles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, assignedRoles.ToArray());
                await userManager.AddToRoleAsync(user, model.RoleName);



                

            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex.Message, ex);
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("/UserManagement/Permission/{userId}")]
        public async Task<IActionResult> Permission(long userId)
        {
            var user = await userService.GetUser(userId);
            ViewData["User"] = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
            
            return View();
        }

        [HttpGet("/UserManagement/Detail/{userId}")]
        public async Task<IActionResult> Detail(long userId)
        {

            User user = null;

            try
            {
                user = await userService.GetUser(userId);
            } catch(Exception ex)
            {
                logger.LogError(ex.Message,ex);
                return NotFound();
            }

            var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();


            

            var result = new 
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TwoFactor = user.TwoFactorEnabled,
                Phone = user.PhoneNumber,
                Role = role
            };

            return View(result);
        }

        [HttpGet("/UserManagement/Delete/{userId}")]
        public async Task<IActionResult> Delete(long userId)
        {
            var loggedInUser = await userManager.GetUserAsync(User);
            var user = await userService.GetUser(userId);


            if(user == null)
            {
                return NotFound();
            }

            if(loggedInUser.Id == user.Id)
            {
                return BadRequest();
            }

          


            user.DeletedDate = DateTime.Now;
            user.DeletedBy = loggedInUser.Id;
            user.UserName = user.UserName + "-" +user.Id;
            user.NormalizedUserName = user.UserName.ToUpper();
            userService.UpdateUser(user);

            return RedirectToAction("Index");
        }

        [HttpPost("/api/user/update/password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                var user = await userService.UpdatePasswordAsync(model.UserId, model.Password);

               

                return Ok();
            }
            catch(NullReferenceException ex)
            {
                logger.LogError(ex.Message, ex);
                return NotFound();
            } catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                ModelState.AddModelError(String.Empty, "Cannot update user password");
                return BadRequest(ModelState);
            }
            
        }
    }
}
