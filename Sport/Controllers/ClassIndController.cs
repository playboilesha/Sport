using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sport.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Sport.Controllers
{
    public class ClassIndController : Controller
    {
        private ApplicationContext db;
        private UserManager<User> _userManager;

        public ClassIndController(ApplicationContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "user")]
        [HttpGet("ClassInd/ClassInd")]
        public IActionResult ClassInd()
        {

            return View();
        }
        [HttpPost("ClassInd/AboutStudent1")]
        public async Task<IActionResult> AboutStudent1(int expirance, string aboutExpirance,string sport)
        {

            if(expirance != 0 && aboutExpirance != null && sport != null)
            {
                string userName = User.Identity.Name;
                var currentUserId = await db.Users
                   .Where(x => x.UserName == userName)
                   .Select(x => x.Id)
                   .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);

                Sports sportt = db.Sports.FirstOrDefault(c => c.Name == sport);
                Students students = new Students() { User = user, AboutExpireance = aboutExpirance, Experience = (Experience)expirance, Sports = sportt };
                await db.AddAsync(students);
                await db.SaveChangesAsync();
                return Ok("nice");
            }

            else
            {
                return Ok("bad");
            }
        }

        [HttpGet]
        public IActionResult GetCoach(string selectedValue)
        {

            Speciality spec = (Speciality)Enum.Parse(typeof(Speciality), selectedValue);


            var options = db.Coach.Where(m => m.Speciality == spec).ToList();

            // Map your model to a simpler representation if needed (to avoid sending unnecessary data)
            var mappedOptions = options.Select(o => new { value = o.Id, label = o.FirstName });

            // Return the mapped options as a JSON response
            return Json(mappedOptions);



            // Retrieve the corresponding data from the model based on the selected value

        }
        [HttpGet]

        public IActionResult GetDate(int CoachId)
        {

            var date = db.RaspisIndividual.Where(c => c.Coach.Id == CoachId && c.Status1 == Status1.Good).Distinct().ToList();

            var distinctIdentities = db.RaspisIndividual.Include(c=>c.Coach).Where(c => c.Coach.Id == CoachId && c.Status1 == Status1.Good).Select(m => m.Date).Distinct().ToList();
            // Map your model to a simpler representation if needed (to avoid sending unnecessary data)
            var mappedOptions = distinctIdentities.Select(o => new { value = o.Date, label = o.Date.ToShortDateString() });

            // Return the mapped options as a JSON response
            return Json(mappedOptions);



            // Retrieve the corresponding data from the model based on the selected value

        }
        [HttpGet]

        public IActionResult GetTime(DateTime Date, int CoachId)
        {



            var time = db.RaspisIndividual.Include(c => c.Coach).Where(c => c.Date == Date && c.Status1 == Status1.Good && c.Coach.Id == CoachId).ToList();


            // Map your model to a simpler representation if needed (to avoid sending unnecessary data)
            var mappedOptions = time.Select(o => new { value = o.Time.ToString(), label = o.Time.ToString() });

            // Return the mapped options as a JSON response
            return Json(mappedOptions);



            // Retrieve the corresponding data from the model based on the selected value

        }
        public async Task<IActionResult> CheckStudent(string sport, string coachId, DateTime date, TimeSpan time)
        {
            string userName = User.Identity.Name;
            var currentUserId = await db.Users
               .Where(x => x.UserName == userName)
               .Select(x => x.Id)
               .FirstOrDefaultAsync();
            var stu = db.Students.ToList();
            Sports sportt = db.Sports.FirstOrDefault(c => c.Name == sport);
            Students students = await db.Students.FirstOrDefaultAsync(c => c.User.Id == currentUserId && c.Sports == sportt);
            if (students == null)
            {
                return Ok("null");
            }
            else
            {
                RaspisIndividual raspisidividual = db.RaspisIndividual.FirstOrDefault(c => c.Time == time && c.Date == date);
                ClassesIndividual classesIndividual = new ClassesIndividual() { Students = students, RaspisIndividual = raspisidividual, Status = Status.Wait };
                if (raspisidividual != null)
                {
                    await db.AddAsync(classesIndividual);



                    raspisidividual.Status1 = Status1.Resorved; // Assuming you have a column/property named "ColumnToBeUpdated" in the "Rows" table/entity.

                    await db.SaveChangesAsync();
                }

                string response = "good";// Replace with your actual response
                return Ok(response);
            }
        }
        [Authorize(Roles = "coach")]
        [HttpGet("ClassInd/AddClassesInd")]
        public async Task<IActionResult> AddClassesInd(string status)
        {
            var currentUserId = await db.Users
              .Where(x => x.UserName == User.Identity.Name)
              .Select(x => x.Id)
              .FirstOrDefaultAsync();
            User user = await _userManager.FindByIdAsync(currentUserId);

            Coach coach = db.Coach.FirstOrDefault(c => c.User.Id == currentUserId);

            string a = coach.Speciality.ToString();
            Sports sports = db.Sports.FirstOrDefault(c => c.Name == a);
            var hall = await db.Hall.Where(c => c.Sports.Id == sports.Id).ToListAsync();
            ViewBag.halls = hall;
            var raspis = await db.RaspisIndividual.Where(c => c.Coach == coach).ToListAsync();


            ViewBag.raspisIndividual = raspis;
            ViewBag.Status = status;
            return View();
        }
        [Authorize(Roles = "coach")]
        [HttpPost]
        public async Task<IActionResult> AddClassesInd(string CoachName, DateTime date, TimeSpan time, string selectedValue)
        {
            var currentUserId = await db.Users
                .Where(x => x.UserName == CoachName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            User user = await _userManager.FindByIdAsync(currentUserId);

            Coach coach = db.Coach.Include(c=>c.User).FirstOrDefault(c => c.User.Id == currentUserId);
           
           
            string a = coach.Speciality.ToString();
            Sports sports = db.Sports.FirstOrDefault(c => c.Name == a);
            Hall hall = db.Hall.Include(c => c.Sports).FirstOrDefault(c => c.Sports.Id == sports.Id);
            var halls = await db.Hall.Include(c=>c.Sports).Where(c => c.Sports.Id == sports.Id).ToListAsync();
            ViewBag.halls = halls;

            RaspisIndividual raspisIndividual1 = new RaspisIndividual() { Coach = coach, Sports = sports, Hall = hall, Date = date, Time = time, Status1 = Status1.Good };

            RaspisIndividual raspisproverka = db.RaspisIndividual.FirstOrDefault(c => c.Time == time && c.Date == date && c.Coach.Id == coach.Id);
            GroupClassesRaspis groupClassesRaspis = db.GroupClassesRaspis.Include(c => c.Coach).FirstOrDefault(c => c.StartTime == time && c.Date == date && c.Coach.Id == coach.Id);
            string status;
            if (raspisproverka == null && groupClassesRaspis == null)
            {
                status = "Все супер";
                await db.AddAsync(raspisIndividual1);
                await db.SaveChangesAsync();


            }
            else
            {
                status = "Такой уже существует";
            }
            return RedirectToAction("AddClassesInd", new { status = status });


            // Redirect or return success message
            // ...

            // Handle validation errors if any
            // ...
        }
        [Authorize(Roles = "coach")]
        [HttpPost("ClassInd/StatusFilter")]
        public IActionResult StatusFilter(string Filter)
        {
            return RedirectToAction("SelectClassesInd", "ClassInd", new { Filter });     
        }


        [Authorize(Roles = "coach")]
        [HttpGet("ClassInd/SelectClassesInd")]
        public async Task<IActionResult> SelectClassesInd(string Filter)
        {
            if (Filter == "Все")
            {
                var currentUserId = await db.Users
                    .Where(x => x.UserName == User.Identity.Name)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                Coach coach = db.Coach.FirstOrDefault(c => c.User.Id == currentUserId);
                string a = coach.Speciality.ToString();
                Sports sports = db.Sports.FirstOrDefault(c => c.Name == a);
                var hall = await db.Hall.Where(c => c.Sports.Id == sports.Id).ToListAsync();
                var user1 = await db.Users.ToListAsync();
                var stu = db.Students.FirstOrDefault();
                var raspis = await db.RaspisIndividual.Where(c => c.Coach == coach).ToListAsync();
                var ras = await db.ClassesIndividual
                   .Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall)
                   .Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach)
                   .Include(c => c.Students).ThenInclude(c => c.User)
                   .Where(h => h.RaspisIndividual.Coach == coach && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours + 1) > DateTime.Now).ToListAsync();
                ViewBag.rasspis = ras;
                ViewBag.Filter = "Все";
                return View();
            }
            else if (Filter == "Ожидание")
            {
                var currentUserId = await db.Users
                    .Where(x => x.UserName == User.Identity.Name)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                Coach coach = db.Coach.Include(c => c.User).FirstOrDefault(c => c.User.Id == currentUserId);
                var raspis = await db.RaspisIndividual.Where(c => c.Coach == coach).ToListAsync();
                var ras = await db.ClassesIndividual
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall)
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach)
                    .Include(c => c.Students).ThenInclude(c => c.User)
                    .Where(h => h.RaspisIndividual.Coach == coach && h.Status == Status.Wait && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours + 1) > DateTime.Now).ToListAsync();
                ViewBag.Filter = "Ожидание";
                ViewBag.rasspis = ras;
                return View();
            }
            else if (Filter == "Отменённое")
            {
                var currentUserId = await db.Users
                    .Where(x => x.UserName == User.Identity.Name)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                Coach coach = db.Coach.Include(c => c.User).FirstOrDefault(c => c.User.Id == currentUserId);
                var raspis = await db.RaspisIndividual.Where(c => c.Coach == coach).ToListAsync();
                var ras = await db.ClassesIndividual
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall)
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach)
                    .Include(c => c.Students).ThenInclude(c => c.User)
                    .Where(h => h.RaspisIndividual.Coach == coach && h.Status == Status.Cancel && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours + 1) > DateTime.Now).ToListAsync();
                ViewBag.Filter = "Отменённое";
                ViewBag.rasspis = ras;
                return View();
            }
            else if (Filter == "Принятое")
            {
                var currentUserId = await db.Users
                    .Where(x => x.UserName == User.Identity.Name)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                Coach coach = db.Coach.Include(c => c.User).FirstOrDefault(c => c.User.Id == currentUserId);
                var raspis = await db.RaspisIndividual.Where(c => c.Coach == coach).ToListAsync();
                var ras = await db.ClassesIndividual
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall)
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach)
                    .Include(c => c.Students).ThenInclude(c => c.User)
                    .Where(h => h.RaspisIndividual.Coach == coach && h.Status == Status.Good && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours + 1) > DateTime.Now).ToListAsync();
                ViewBag.Filter = "Принятое";
                ViewBag.rasspis = ras;
                return View();
            }
            else if (Filter == "Прошедшее")
            {
                var currentUserId = await db.Users
                    .Where(x => x.UserName == User.Identity.Name)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                Coach coach = db.Coach.Include(c => c.User).FirstOrDefault(c => c.User.Id == currentUserId);
                var raspis = await db.RaspisIndividual.Where(c => c.Coach == coach).ToListAsync();
                var ras = await db.ClassesIndividual
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall)
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach)
                    .Include(c => c.Students).ThenInclude(c => c.User)
                    .Where(h => h.RaspisIndividual.Coach == coach  && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours+1) < DateTime.Now).ToListAsync();
                ViewBag.Filter = "Прошеднее";
                ViewBag.rasspis = ras;
                return View();
            }
            else
            {
                ViewBag.Fiter = "Все";
                var currentUserId = await db.Users
                    .Where(x => x.UserName == User.Identity.Name)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                Coach coach = db.Coach.FirstOrDefault(c => c.User.Id == currentUserId);
                string a = coach.Speciality.ToString();
                Sports sports = db.Sports.FirstOrDefault(c => c.Name == a);
                var hall = await db.Hall.Where(c => c.Sports.Id == sports.Id).ToListAsync();
                var user1 = await db.Users.ToListAsync();
                var stu = db.Students.FirstOrDefault();
                var raspis = await db.RaspisIndividual.Where(c => c.Coach == coach).ToListAsync();
                ///////////////////
                var ras = await db.ClassesIndividual
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall)
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach)
                    .Include(c => c.Students).ThenInclude(c => c.User)
                    .Where(h => h.RaspisIndividual.Coach == coach && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours + 1) > DateTime.Now ).ToListAsync();
                ViewBag.Filter = "Все";
                ViewBag.rasspis = ras;
                return View();
            }
            
        }


        [Authorize(Roles = "coach")]
        [HttpPost("ClassInd/UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(int id,Status status)
        {
            var classind = await db.ClassesIndividual.FirstOrDefaultAsync(c => c.Id == id);
            classind.Status = status;
            await db.SaveChangesAsync();
            return RedirectToAction("SelectClassesInd");
        }

        [Authorize(Roles = "user")]
        [HttpPost("ClassInd/StatusFilterIndUser")]
        public IActionResult StatusFilterIndUser(string Filter)
        {
            return RedirectToAction("CheckInd", "ClassInd", new { Filter });
        }


        [Authorize(Roles = "user")]
        [HttpGet("ClassInd/CheckInd")]
        public async Task<IActionResult> CheckInd(string Filter)
        {
            if (Filter == "Все")
            {
                var currentUserId = await db.Users
                 .Where(x => x.UserName == User.Identity.Name)
                 .Select(x => x.Id)
                 .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                var ras = await db.ClassesIndividual
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall)
                    .Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach)
                    .Include(c => c.Students).ThenInclude(c => c.User)
                    .Where(h => h.Students.User == user && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours) > DateTime.Now)
                    .ToListAsync();
                ViewBag.Filter = "Все";
                ViewBag.rasspis = ras;
                return View(); ;
            }
            else if (Filter == "Ожидание")
            {
                var currentUserId = await db.Users
                 .Where(x => x.UserName == User.Identity.Name)
                 .Select(x => x.Id)
                 .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                var ras = await db.ClassesIndividual.Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall).Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach).Include(c => c.Students).ThenInclude(c => c.User).Where(h => h.Students.User == user && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours) > DateTime.Now && h.Status == Status.Wait).ToListAsync();
                ViewBag.Filter = "Ожидание";
                ViewBag.rasspis = ras;
                return View(); ;
            }
            else if (Filter == "Отменённое")
            {
                var currentUserId = await db.Users
               .Where(x => x.UserName == User.Identity.Name)
               .Select(x => x.Id)
               .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                var ras = await db.ClassesIndividual.Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall).Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach).Include(c => c.Students).ThenInclude(c => c.User).Where(h => h.Students.User == user && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours) > DateTime.Now && h.Status == Status.Cancel).ToListAsync();
                ViewBag.Filter = "Отменённое";
                ViewBag.rasspis = ras;
                return View();
            }
            else if (Filter == "Принятое")
            {
                var currentUserId = await db.Users
               .Where(x => x.UserName == User.Identity.Name)
               .Select(x => x.Id)
               .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                var ras = await db.ClassesIndividual.Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall).Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach).Include(c => c.Students).ThenInclude(c => c.User).Where(h => h.Students.User == user && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours) > DateTime.Now &&  h.Status == Status.Good).ToListAsync();
                ViewBag.Filter = "Принятое";
                ViewBag.rasspis = ras;
                return View();     
            }
            else if (Filter == "Прошедшие")
            {
                var currentUserId = await db.Users
               .Where(x => x.UserName == User.Identity.Name)
               .Select(x => x.Id)
               .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                var ras = await db.ClassesIndividual.Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall).Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach).Include(c => c.Students).ThenInclude(c => c.User).Where(h => h.Students.User == user && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours + 1) < DateTime.Now || h.RaspisIndividual.Status1 == Status1.Over).ToListAsync();
                ViewBag.Filter = "Прошедшие";
                ViewBag.rasspis = ras;
                return View();            
            }
            else
            {
                var currentUserId = await db.Users
               .Where(x => x.UserName == User.Identity.Name)
               .Select(x => x.Id)
               .FirstOrDefaultAsync();
                User user = await _userManager.FindByIdAsync(currentUserId);
                /*var coach = await db.Coach.ToListAsync();
                Students students = db.Students.FirstOrDefault(c => c.User.Id == currentUserId);
                var hall = await db.Hall.ToListAsync();
                var user1 = await db.Users.ToListAsync();
                var stu = db.Students.FirstOrDefault();
                var raspis = await db.RaspisIndividual.ToListAsync();*/
                ///////////////////

                var ras = await db.ClassesIndividual.Include(c => c.RaspisIndividual).ThenInclude(c => c.Hall).Include(c => c.RaspisIndividual).ThenInclude(c => c.Coach).Include(c => c.Students).ThenInclude(c => c.User).Where(h => h.Students.User == user && h.RaspisIndividual.Date.AddHours(h.RaspisIndividual.Time.Hours) > DateTime.Now).ToListAsync();



                ViewBag.Filter = "Все";
                ViewBag.rasspis = ras;



                return View();
            }

          
        }
    }
}
