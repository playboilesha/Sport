using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sport.ViewModels;
using Sport.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Sport.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext db;

        public AccountController(RoleManager<IdentityRole> roleManager,UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            db = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.UserName, Year = model.Year };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    List<string> roles = new List<string>() {"user"};
                    
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var allRoles = _roleManager.Roles.ToList();
                 
                    // получаем список ролей, которые были добавлены
                    var addedRoles = roles.Except(userRoles);
                    await _userManager.AddToRolesAsync(user, addedRoles);
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var raspis = db.GroupClassesRaspis.Where(c => c.Date.AddHours(c.EndTime.Hours) < DateTime.Now).ToList();
                var Indraspis = db.RaspisIndividual.Where(c => c.Date.AddHours(c.Time.Hours + 1) < DateTime.Now).ToList();
                
                for (int i = 0; i < raspis.Count; i++)
                {
                    raspis[i].StatusGroup = StatusGroup.Сancel;
                }
                for (int i = 0; i < Indraspis.Count; i++)
                {
                    Indraspis[i].Status1 = Status1.Over;
                }
                await db.SaveChangesAsync();
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result =
                    await _signInManager.PasswordSignInAsync(user.UserName, model.Password,model.RememberMe, false);
                //var result =
                //   await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                
                    if (result.Succeeded)
                    {
                        // проверяем, принадлежит ли URL приложению
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            var roles = await _userManager.GetRolesAsync(user);

                            if(roles[0] == "user")
                            {
                                Students students = db.Students.Include(c=>c.User).FirstOrDefault(c => c.User.Id == user.Id);
                                if(students == null)
                                {
                                    Students students1 = new Students() { User = user };
                                    await db.AddAsync(students1);
                                    await db.SaveChangesAsync();
                                }
                            }

                            return RedirectToAction("MainAdmin", "Home");



                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин ");
                }
            }
            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Main()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet("Account/About")]
        public async Task<IActionResult> AboutAsync()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            var sports1 = db.Sports.ToList();
            var sp = db.Students.ToList();
            ViewBag.sp = sports1;
            Students students = db.Students.FirstOrDefault(c => c.User == user);
            ViewBag.student = students;

            
            //ClassesIndividual classesIndividual = db.ClassesIndividual.FirstOrDefault(b => b.Students == students);
            //var raspisIndividual1 = db.RaspisIndividual.ToList();
            //RaspisIndividual raspisIndividual = db.RaspisIndividual.FirstOrDefault(c => c.Id == classesIndividual.RaspisIndividual.Id);
            //Sports sports = db.Sports.FirstOrDefault(j => j.Id == raspisIndividual.Sports.Id);


            return View(user);
        }

        [HttpGet("Account/AboutCoach")]
        public async Task<IActionResult> AboutCoach()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Coach coach = await db.Coach.FirstOrDefaultAsync(c => c.User.Id == user.Id);
            return View(coach);
        }
        [HttpGet("Account/GetExpirance")]
        public async Task<IActionResult> GetExpirance(int sport)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            var sports1 = db.Sports.ToList();
           
           
            var students = db.Students.FirstOrDefault(c => c.User == user && c.Sports.Id == sport);
            ViewBag.student = students;

            /*var mappedOptions = students.Select(o => new { exp = o.Experience.ToString(), aexp = o.AboutExpireance.ToString() });*/

            if (students == null)
            {
                // Return the mapped options as a JSON response
                return Ok("null");
            }
            else
            {
                return Json(new { exp = students.Experience.ToString(), aexp = students.AboutExpireance });
            }



            
        }
        [HttpGet("Account/UpdateInfo")]
        public async Task<IActionResult> UpdateInfoAsync(string name, string lastname, string phone)
        {
            bool isValid = false;
            string statusname = "1";
            string statuslastname = "1";
            string statusphone = "1";
            string pattern = @"^\+[0-9]{12}$";
            if(name == null)
            {
                statusname = "0";
            }
            if(lastname == null)
            {
                statuslastname ="0";
            }
            if(phone == null)
            {
                statusphone = "0";
            }
            if(name == null || lastname == null || phone == null)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (name != null)
                {
                    if (user != null)
                    {
                        user.FirstName = name;
                    }
                }
                if (lastname != null)
                {
                    if (user != null)
                    {
                        user.LastName = lastname;
                    }
                }
                if (phone != null)
                {
                    if (user != null)
                    {
                        user.PhoneNumber = phone;
                    }
                }

                return Json(new { success = isValid, statusname = statusname, statuslastname = statuslastname, statusphone = statusphone, message = "введите данные" });
               
               
                 
                
               
                
            }
          
           
            else
            {
                if (Regex.IsMatch(phone, pattern))
                {
                    isValid = true;
                    User user = await _userManager.FindByNameAsync(User.Identity.Name);
                    if (user != null)
                    {
                        user.FirstName = name;
                        user.LastName = lastname;
                        user.PhoneNumber = phone;
                        await db.SaveChangesAsync();
                    }
                        return Json(new { success = isValid, statusname = statusname, statuslastname = statuslastname, statusphone = statusphone, message = "данные записаны" });
                }
                else
                {
                    statusphone = "0";
                    return Json(new { success = isValid, statusname = statusname, statuslastname = statuslastname, statusphone = statusphone, message = "не верный телефон" });
                }
            }
            
            
            
        }

        [HttpPost]
        public IActionResult VerifyPhoneNumber(string phoneNumber)
        {
            // Perform necessary verification logic, such as checking if the phone number matches a specific pattern or is registered on your system
            bool isValid = false;
            if (phoneNumber == null)
            {
                return Json(new { success = isValid });
            }
            else
            {
                string pattern = @"^\+[0-9]{12}$";
                if (Regex.IsMatch(phoneNumber, pattern))
                {
                    isValid = true;
                }



                // Return a JSON response indicating the verification result
                return Json(new { success = isValid });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateExpiranceAsync(int sportId, Experience exp, string aboutexp)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Sports sports = db.Sports.FirstOrDefault(c => c.Id == sportId);
            Students students = db.Students.Include(c => c.User).Include(c=>c.Sports).FirstOrDefault(c => c.User == user && c.Sports.Id == sportId);
            if (students == null)
            {
                if (aboutexp != null)
                {
                    Students stu = new Students() { User = user, Sports = sports, Experience = exp, AboutExpireance = aboutexp };
                    await db.AddAsync(stu);
                    await db.SaveChangesAsync();
                    return Ok(exp.ToString());
                }
                else
                {
                    return Ok("null");
                }
            }
            else
            {
                if (aboutexp != null)
                {
                    students.AboutExpireance = aboutexp;
                    students.Experience = exp;
                    await db.SaveChangesAsync();
                    return Ok(exp.ToString());
                }
                else
                {
                    return Ok("null");
                }
            }
                
            
        }



    }


    



}
