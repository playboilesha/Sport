using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Sport.Models;
using Sport.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Sport.Controllers
{
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        private ApplicationContext _context;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ApplicationContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Index() => View(_roleManager.Roles.ToList());
        [Authorize(Roles = "admin")]
        public IActionResult Create() => View();
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UserList()
        {
           
           return View(_userManager.Users.ToList()); 
        }
        [Authorize(Roles = "admin")]
        public IActionResult Coach()
        {
            string roleName = "coach";
          
            var usersInRole =   _userManager.GetUsersInRoleAsync(roleName).Result;
            ViewBag.coach = usersInRole;
            return View();
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditCoach(string userId)
        {
            string roleName = "coach";

            var usersInRole = _userManager.GetUsersInRoleAsync(roleName).Result;
            Coach coach = await _context.Coach.FirstOrDefaultAsync(c => c.User.Id == userId);
            return View(coach);
        }
        [Authorize(Roles = "admin")]
        [HttpPost("Roles/UpdateInfoCoach")]
        public async Task<IActionResult> UpdateInfoCoach(string name, string lastname, string about, Speciality sport, string nameuser,string img,string age)
        {
            bool isNumber = int.TryParse(age, out int numericValue);
            string status;
            var currentUserId = await _context.Users
               .Where(x => x.UserName == nameuser)
               .Select(x => x.Id)
               .FirstOrDefaultAsync();
            if(name!= null && lastname != null && about != null && img!= null&& isNumber==true)
            {
                Coach coach = await _context.Coach.FirstOrDefaultAsync(c => c.User.Id == currentUserId);
                coach.FirstName = name;
                coach.LastName = lastname;
                coach.Image = img;
                coach.Speciality = sport;
                coach.About = about;
                coach.Age = Convert.ToInt32(age);
                await _context.SaveChangesAsync();
                status = "Данные записаны";
            }
            else
            {
                if(isNumber == false)
                {
                    status = "Введите число!";
                }
                else{
                    status = "Введите данные";
                }
                
            }
            return Ok(status);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string userId)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles

                };
                return View(model);
            }

            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roleFirst = await _userManager.GetRolesAsync(user);
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);
                Coach coach = new Coach() {User = user };
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                var roleSecond = await _userManager.GetRolesAsync(user);
                Coach CheckCoach = _context.Coach.FirstOrDefault(c => c.User == user);
                if (roleSecond.Contains("coach"))
                {
                   

                   await _context.Coach.AddAsync(coach);
                   await _context.SaveChangesAsync();
                }
                if(roleFirst.Contains("coach")&& roleSecond != roleFirst && CheckCoach.FirstName == null)
                {
                    
                   var coachRemove = _context.Coach.Include(s => s.User).FirstOrDefault(s => s.User.Id == userId);
                    _context.Coach.Remove(coachRemove);
                    await _context.SaveChangesAsync();

                }
                   

                return RedirectToAction("UserList");
            }
           

            return NotFound();
        }
    }
}
