using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Controllers
{
    public class HallController : Controller
    {
        private ApplicationContext db;

        public HallController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet("Hall/Hall")]
        public async Task<IActionResult> Hall()
        {
            // Fetch data from first table
           

           /* // Merge the data
            var mergedData = (from t1 in hall
                              join t2 in sports on t1.Sports.Id equals t2.Id
                              select new { NameSport = t2.Name,Name = t1.Name,Image = t1.Image, CountPeople = t1.CountPeople,About = t1.About }).ToList();

            ViewBag.MergedData = mergedData;

         
            */
           
           
            return View(await db.Hall.ToListAsync());
        }
    }
}
