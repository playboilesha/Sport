using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sport.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Sport.ViewModels;

namespace Sport.Controllers
{
   
    public class EditController : Controller
    {
        
        private ApplicationContext db;

        public EditController(ApplicationContext context)
        {
            db = context;
        }
        /*[HttpGet("Edit/EditCoach")]*/
        public IActionResult EditCoach()
        {
            var students = db.Coach.Include(s => s.User).ToList();
            return View(students);
        }
    }
}
