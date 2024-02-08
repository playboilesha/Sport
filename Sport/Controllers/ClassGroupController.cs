using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sport.Models;
using Sport.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Controllers
{
    public class ClassGroupController : Controller
    {
        private ApplicationContext db;
        private UserManager<User> _userManager;

        public ClassGroupController(ApplicationContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "coach")]
        [HttpGet("ClassGroup/AddGroupClass")]
        public async Task<IActionResult> AddGroupClass(string status, int weekOffset)
        {
            var currentUserId = await db.Users
                .Where(x => x.UserName == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            User user = await _userManager.FindByIdAsync(currentUserId);
            Coach coach = db.Coach.FirstOrDefault(c => c.User.Id == currentUserId);
            string a = coach.Speciality.ToString();
            Sports sports = db.Sports.FirstOrDefault(c => c.Name == a);
            var hall = db.Hall.Where(c => c.Sports.Id == sports.Id).ToList();
            ViewBag.halls = hall;

            ViewBag.Status = status;
            // Calculate the start date of the current week (assuming week starts on Monday)


            return View();
        }

        [Authorize(Roles = "coach")]
        [HttpPost("ClassGroup/AddGroupClass")]
        public async Task<IActionResult> AddGroupClass(int hallId, DateTime date, TimeSpan time, Experience expirance)
        {
            string status;

            var currentUserId = await db.Users
                .Where(x => x.UserName == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            User user = await _userManager.FindByIdAsync(currentUserId);
            Coach coach = db.Coach.FirstOrDefault(c => c.User.Id == currentUserId);
            RaspisIndividual raspisind = db.RaspisIndividual.FirstOrDefault(c => c.Coach == coach && c.Date == date && c.Time == time);
            GroupClassesRaspis groupClassesRaspisprov = db.GroupClassesRaspis.FirstOrDefault(c => c.Date == date && c.StartTime == time);
            if (raspisind == null)
            {
                if (groupClassesRaspisprov == null)
                {
                    string a = coach.Speciality.ToString();
                    Sports sports = db.Sports.FirstOrDefault(c => c.Name == a);

                    Hall hall = db.Hall.FirstOrDefault(c => c.Id == hallId);
                    TimeSpan timeSpan2 = new TimeSpan(1, 00, 00);
                    TimeSpan end = time.Add(timeSpan2);

                    GroupClassesRaspis groupClassesRaspis = new GroupClassesRaspis() { Sports = sports, Coach = coach, Hall = hall, ReservedPlace = 0, Date = date, DayOfWeek = date.DayOfWeek, StartTime = time, EndTime = end, Experience = expirance, StatusGroup = StatusGroup.Good };

                    await db.AddAsync(groupClassesRaspis);
                    await db.SaveChangesAsync();
                    status = "все хорошо";
                }
                else
                {

                    status = "Уже есть занятие на это время";
                }
            }
            else
            {
                status = "Уже есть занятие на это время";
            }
            return RedirectToAction("AddGroupClass", new { status = status });
        }

        [Authorize(Roles = "coach")]
        [HttpGet("ClassGroup/CheckGroupClassRaspis")]
        public async Task<IActionResult> CheckGroupClassRaspis(string status, int weekOffset,DateTime currentdate)
        {
            var currentUserId = await db.Users
                .Where(x => x.UserName == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            User user = await _userManager.FindByIdAsync(currentUserId);
            Coach coach = db.Coach.FirstOrDefault(c => c.User.Id == currentUserId);
            string a = coach.Speciality.ToString();
            Sports sports = db.Sports.FirstOrDefault(c => c.Name == a);
            var hall = db.Hall.Where(c => c.Sports.Id == sports.Id).ToList();
            ViewBag.halls = hall;
            DateTime currentDate = DateTime.Now;
            ViewBag.Status = status;
            // Calculate the start date of the current week (assuming week starts on Monday)
            DateTime startDate = currentDate.AddDays(-(int)currentDate.DayOfWeek + (int)DayOfWeek.Monday);
            if (weekOffset == 1)
            {
                startDate = currentdate.AddDays(7);
            }
            if (weekOffset == -1)
            {
                startDate = currentdate.AddDays(-7);
            }
            DateTime date1 = new DateTime(2023, 11, 13);
            TimeSpan timeSpan1 = new TimeSpan(9, 00, 00);
            TimeSpan timeSpan2 = new TimeSpan(10, 00, 00);
            var group = db.GroupClassesRaspis.Include(c=>c.Hall).Include(c => c.Hall).Include(c => c.Sports).Where(c => c.Coach == coach).ToList();
            // Create a list of classes for each day of the week
            List<List<GroupClassesRaspis>> classes = new List<List<GroupClassesRaspis>>();
            for (int i = 0; i < 7; i++)
            {
                List<GroupClassesRaspis> dayClasses = new List<GroupClassesRaspis>();
                for (int i1 = 0; i1 < group.Count; i1++)
                {
                    dayClasses.Add(group[i1]);
                }
                // Add your classes for each day here
                classes.Add(dayClasses);
            }

            // Create the schedule view model
            ScheduleViewModel model = new ScheduleViewModel
            {
                StartDate = startDate,
                Classes = classes
            };
            return View(model);
        }
        [Authorize(Roles = "coach")]
        [HttpGet("ClassGroup/EditGroupClass")]
        public async Task<IActionResult> EditGroupClass(int idGroupClass)
        {
            GroupClassesRaspis groupClassesRaspis = await db.GroupClassesRaspis.Include(c => c.Coach).Include(c => c.Hall).Include(c => c.Sports).FirstOrDefaultAsync(c => c.Id == idGroupClass);

            ViewBag.id = idGroupClass;
            var classesGroup = db.ClassesGroup.Include(c => c.Students).ThenInclude(c => c.User).Include(c => c.GroupClassesRaspis).Where(c => c.GroupClassesRaspis.Id == idGroupClass).ToList();
            
            ViewBag.Students = classesGroup;
            
            return View(groupClassesRaspis);
        }

        [Authorize(Roles = "user")]
        [HttpGet("ClassGroup/CheckGroup")]
        public IActionResult CheckGroup(string status, int weekOffset, DateTime currentdate)
        {

            DateTime currentDate = DateTime.Now;
            ViewBag.Status = status;
            // Calculate the start date of the current week (assuming week starts on Monday)
            DateTime startDate = currentDate.AddDays(-(int)currentDate.DayOfWeek + (int)DayOfWeek.Monday);
            if (weekOffset == 1)
            {
                startDate = currentdate.AddDays(7);
            }
            if (weekOffset == -1)
            {
                startDate = currentdate.AddDays(-7);
            }
            DateTime date1 = new DateTime(2023, 11, 13);
            TimeSpan timeSpan1 = new TimeSpan(9, 00, 00);
            TimeSpan timeSpan2 = new TimeSpan(10, 00, 00);
            var group = db.GroupClassesRaspis.Include(c => c.Coach).Include(c => c.Hall).Include(c => c.Sports).ToList();
            // Create a list of classes for each day of the week
            List<List<GroupClassesRaspis>> classes = new List<List<GroupClassesRaspis>>();
            for (int i = 0; i < 7; i++)
            {
                List<GroupClassesRaspis> dayClasses = new List<GroupClassesRaspis>();
                for (int i1 = 0; i1 < group.Count; i1++)
                {
                    dayClasses.Add(group[i1]);
                }
                // Add your classes for each day here
                classes.Add(dayClasses);
            }

            // Create the schedule view model
            ScheduleViewModel model = new ScheduleViewModel
            {
                StartDate = startDate,
                Classes = classes
            };

            return View(model);

        }

        [Authorize(Roles = "user")]
        [HttpGet("ClassGroup/BookingGroup")]
        public async Task<IActionResult> BookingGroup(int idGroupClass, string status)
        {
            var currentUserId = await db.Users
               .Where(x => x.UserName == User.Identity.Name)
               .Select(x => x.Id)
               .FirstOrDefaultAsync();
            var group = await db.GroupClassesRaspis.Include(c => c.Coach).Include(c => c.Hall).Include(c => c.Sports).FirstOrDefaultAsync(c => c.Id == idGroupClass);
            ViewBag.Status = status;

            var stu = await db.Students.Include(c => c.User).Include(c => c.Sports).FirstOrDefaultAsync(c => c.User.Id == currentUserId && c.Sports.Id == group.Sports.Id);

            if (stu == null)
            {
                ViewBag.Expirance = "null";
            }
            else
            {
                ViewBag.Expirance = stu.Experience;
                ViewBag.About = stu.AboutExpireance;

            }
            return View(group);

        }
        [Authorize(Roles = "user")]
        [HttpPost("ClassGroup/BookingStudent")]
        public async Task<IActionResult> BookingStudent(Experience exp, string about, int idGroup, int idSport)
        {
            string status;
            var currentUserId = await db.Users
               .Where(x => x.UserName == User.Identity.Name)
               .Select(x => x.Id)
               .FirstOrDefaultAsync();
            User user = await _userManager.FindByIdAsync(currentUserId);
            Sports sport = await db.Sports.FirstOrDefaultAsync(c => c.Id == idSport);
            if (about != null)
            {
                Students student = db.Students.FirstOrDefault(c => c.User.Id == currentUserId && c.Sports.Id == idSport);
                GroupClassesRaspis groupClassesRaspis = await db.GroupClassesRaspis.Include(c => c.Coach).Include(c => c.Hall).Include(c => c.Sports).FirstOrDefaultAsync(c => c.Id == idGroup);
                if (student == null)
                {
                    Students stu = new Students() { Sports = sport, User = user, Experience = exp, AboutExpireance = about };
                    await db.AddAsync(stu);
                    await db.SaveChangesAsync();
                    if (groupClassesRaspis.ReservedPlace == groupClassesRaspis.Hall.CountPeople)
                    {
                        status = "Места заполнены";
                    }
                    else
                    {
                        if (stu.Experience == groupClassesRaspis.Experience)
                        {
                            ClassesGroup classes = db.ClassesGroup.Include(c => c.Students).Include(c => c.GroupClassesRaspis).FirstOrDefault(c => c.Students == stu && c.GroupClassesRaspis == groupClassesRaspis);
                            if (classes == null)
                            {
                                ClassesGroup classesGroup = new ClassesGroup() { GroupClassesRaspis = groupClassesRaspis, Status = Status.Good, Students = stu,Check = Check.None};
                                await db.AddAsync(classesGroup);
                                int a = groupClassesRaspis.ReservedPlace;
                                groupClassesRaspis.ReservedPlace = a + 1;
                                if (a + 1 == groupClassesRaspis.Hall.CountPeople)
                                {
                                    groupClassesRaspis.StatusGroup = StatusGroup.Filled;

                                }
                               
                                await db.SaveChangesAsync();
                                status = "вы записаны";
                            }
                            else
                            {
                                status = "Вы уже записаны";
                            }
                        }
                        else
                        {
                            status = "Не совпадает опыт занятия (изменить вы можете в профиле)";
                        }
                    }
                }
                else
                {
                    if (groupClassesRaspis.ReservedPlace == groupClassesRaspis.Hall.CountPeople)
                    {
                        status = "Места заполнены";
                    }
                    else
                    {
                        if (student.Experience == groupClassesRaspis.Experience)
                        {
                            ClassesGroup classes = await db.ClassesGroup.Include(c => c.Students).Include(c => c.GroupClassesRaspis).FirstOrDefaultAsync(c => c.Students == student && c.GroupClassesRaspis == groupClassesRaspis);
                            if (classes == null)
                            {
                                ClassesGroup classesGroup = new ClassesGroup() { GroupClassesRaspis = groupClassesRaspis, Status = Status.Good, Students = student };
                                await db.AddAsync(classesGroup);

                                int a = groupClassesRaspis.ReservedPlace;
                                groupClassesRaspis.ReservedPlace = a + 1;
                                if (a + 1 == groupClassesRaspis.Hall.CountPeople)
                                {
                                    groupClassesRaspis.StatusGroup = StatusGroup.Filled;
                                }
                                await db.SaveChangesAsync();
                                status = "вы записаны";
                            }
                            else
                            {
                                status = "Вы уже записаны";
                            }
                        }
                        else
                        {
                            status = "Не совпадает опыт занятия (изменить вы можете в профиле)";
                        }
                    }
                }
            }
            else
            {
                status = "Заполните данные";
            }
            return Ok(status);

        }


        [Authorize(Roles = "user")]
        [HttpPost("ClassGroup/StatusFilterUserGroup")]
        public IActionResult StatusFilterUserGroup(string Filter)
        {
            return RedirectToAction("UserCheckGroup", "ClassGroup", new { Filter });
        }

        [Authorize(Roles = "user")]
        [HttpGet("ClassGroup/UserCheckGroup")]
        public async Task<IActionResult> UserCheckGroup(string Filter)
        {
            if (Filter == "Предстоящее")
            {
                var currentUserId = await db.Users
                 .Where(x => x.UserName == User.Identity.Name)
                 .Select(x => x.Id)
                 .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                var classes = db.ClassesGroup
                    .Include(c => c.Students).ThenInclude(c => c.User)
                    .Include(c => c.GroupClassesRaspis).ThenInclude(c => c.Coach)
                    .Include(c => c.GroupClassesRaspis).ThenInclude(c => c.Hall)
                    .Where(c => c.Students.User == user && c.GroupClassesRaspis.Date.AddHours(c.GroupClassesRaspis.EndTime.Hours)>DateTime.Now && c.GroupClassesRaspis.StatusGroup == StatusGroup.Good || c.GroupClassesRaspis.StatusGroup == StatusGroup.Filled).ToList();
                ViewBag.rasspis = classes;
                ViewBag.Filter = "Предстоящее";
                return View();
            }
            else if (Filter == "Отменённые")
            {
                var currentUserId = await db.Users
                .Where(x => x.UserName == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                var classes = db.ClassesGroup
                    .Include(c => c.Students).ThenInclude(c => c.User)
                    .Include(c => c.GroupClassesRaspis).ThenInclude(c => c.Coach)
                    .Include(c => c.GroupClassesRaspis).ThenInclude(c => c.Hall)
                    .Where(c => c.Students.User == user && c.GroupClassesRaspis.Date.AddHours(c.GroupClassesRaspis.EndTime.Hours) > DateTime.Now && c.GroupClassesRaspis.StatusGroup == StatusGroup.Сancel  ).ToList();
                ViewBag.rasspis = classes;
                ViewBag.Filter = "Отменённые";
                return View();
            }
            else if (Filter == "Прошедшие")
            {
                var currentUserId = await db.Users
                .Where(x => x.UserName == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                var classes = db.ClassesGroup
                    .Include(c => c.Students).ThenInclude(c => c.User)
                    .Include(c => c.GroupClassesRaspis).ThenInclude(c => c.Coach)
                    .Include(c => c.GroupClassesRaspis).ThenInclude(c => c.Hall)
                    .Where(c => c.Students.User == user && c.GroupClassesRaspis.Date.AddHours(c.GroupClassesRaspis.EndTime.Hours) < DateTime.Now && c.GroupClassesRaspis.StatusGroup == StatusGroup.Сancel).ToList();
                ViewBag.rasspis = classes;
                ViewBag.Filter = "Прошедшие";
                return View();
            }
            else
            {
                var currentUserId = await db.Users
                  .Where(x => x.UserName == User.Identity.Name)
                  .Select(x => x.Id)
                  .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                var classes = db.ClassesGroup
                    .Include(c => c.Students).ThenInclude(c => c.User)
                    .Include(c => c.GroupClassesRaspis).ThenInclude(c => c.Coach)
                    .Include(c => c.GroupClassesRaspis).ThenInclude(c => c.Hall)
                    .Where(c => c.Students.User == user && c.GroupClassesRaspis.Date.AddHours(c.GroupClassesRaspis.EndTime.Hours) > DateTime.Now && c.GroupClassesRaspis.StatusGroup == StatusGroup.Good || c.GroupClassesRaspis.StatusGroup == StatusGroup.Filled).ToList();
                ViewBag.rasspis = classes;
                return View();
            }
        }
       

        [Authorize(Roles = "coach")]
        [HttpPost("ClassGroup/RemoveGroupClass")]
        public async Task<IActionResult> RemoveGroupClass(int idRaspis)
        {
            var currentUserId = await db.Users
                .Where(x => x.UserName == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            User user = await _userManager.FindByIdAsync(currentUserId);
            GroupClassesRaspis groupClassesRaspis = db.GroupClassesRaspis.FirstOrDefault(c => c.Id == idRaspis);
            var userraspis = db.ClassesGroup.Include(c => c.GroupClassesRaspis).Where(c => c.GroupClassesRaspis == groupClassesRaspis).ToList();
            groupClassesRaspis.StatusGroup = StatusGroup.Сancel;
            for (int i = 0; i < userraspis.Count; i++)
            {
                userraspis[i].Status = Status.Cancel;
            }
            await db.SaveChangesAsync();

            return RedirectToAction("CheckGroupClassRaspis", "ClassGroup");
          
        }

        [Authorize(Roles = "user")]
        [HttpPost("ClassGroup/CancelGroupClass")]
        public async Task<IActionResult> CancelGroupClass(int id)
        {
            var currentUserId = await db.Users
                .Where(x => x.UserName == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            User user = await _userManager.FindByIdAsync(currentUserId);
            ClassesGroup userClass = db.ClassesGroup.Include(c => c.GroupClassesRaspis).Include(c => c.Students).ThenInclude(c => c.User).FirstOrDefault(c => c.Id == id );
            GroupClassesRaspis groupClassesRaspis = db.GroupClassesRaspis.FirstOrDefault(c => c.Id == userClass.GroupClassesRaspis.Id);
            //ClassesGroup userraspis = db.ClassesGroup.Include(c => c.GroupClassesRaspis).Include(c=>c.Students).ThenInclude(c=>c.User).FirstOrDefault(c => c.GroupClassesRaspis == groupClassesRaspis && c.Students.User == user);
            db.ClassesGroup.Remove(userClass);
            groupClassesRaspis.ReservedPlace = groupClassesRaspis.ReservedPlace - 1;
            await db.SaveChangesAsync();
            var classes = db.ClassesGroup
                .Include(c => c.Students).ThenInclude(c => c.User)
                .Include(c => c.GroupClassesRaspis).ThenInclude(c => c.Coach)
                .Include(c => c.GroupClassesRaspis).ThenInclude(c => c.Hall)
                .Where(c => c.Students.User == user && c.GroupClassesRaspis.Date.AddHours(c.GroupClassesRaspis.EndTime.Hours) > DateTime.Now && c.GroupClassesRaspis.StatusGroup == StatusGroup.Good || c.GroupClassesRaspis.StatusGroup == StatusGroup.Filled).ToList();
            ViewBag.rasspis = classes;
            return View("UserCheckGroup");
        }
        [Authorize(Roles = "coach")]
        [HttpPost("ClassGroup/checkUser")]
        public async Task<IActionResult> checkUser(int idStu, int idGroup, string status,int id)
        {
            Check check = (Check)Enum.Parse(typeof(Check), status);
            ClassesGroup classesGroup = db.ClassesGroup.Include(c => c.Students).FirstOrDefault(c => c.Id == idGroup && c.Students.Id == idStu);
            classesGroup.Check = check;
            await db.SaveChangesAsync();
            return RedirectToAction("EditGroupClass", new { idGroupClass = id });
        }
    }
}
