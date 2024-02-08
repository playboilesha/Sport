using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Controllers
{
    public class AboutController : Controller
    {
        private ApplicationContext db;

        public AboutController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet("About/About")]
        public async Task<IActionResult> About()
        {
            return View(await db.Klub.ToListAsync());
        }

        [Authorize(Roles = "admin")]
        [HttpGet("About/EditAbout")]
        public async Task<IActionResult> EditAbout()
        {
            var sport = db.Sports.ToList();
            ViewBag.sp = sport;
            Sports sp = db.Sports.FirstOrDefault(c => c.Id == 1);
            ViewBag.Year = sp.Year;
            ViewBag.About = sp.About;
            return View(await db.Klub.FirstOrDefaultAsync());
        }


       
        


        [Authorize(Roles = "admin")]
        [HttpPost("About/EditAboutSport")]
        public IActionResult EditAboutSport(int sportId)
        {
            Sports sport = db.Sports.FirstOrDefault(c => c.Id == sportId);
            return Json(new { about = sport.About.ToString(), year = sport.Year });
        }
        [Authorize(Roles = "admin")]
        [HttpPost("About/EditAbout")]
        public async Task<IActionResult> EditAbout(string name, string email, string year, string about, string tel, string vk)
        {
            if (name != null && email != null && year != null && about != null && tel != null && vk != null)
            {
                if (tel.Count() == 12 || tel.Count() == 11 || tel.Count() == 13)
                {
                    if (int.TryParse(year, out int numericValue))
                    {
                        Klub klub = db.Klub.FirstOrDefault();

                        klub.About = about;
                        klub.Email = email;
                        klub.Name = name;
                        klub.Vk = vk;
                        klub.Telephone = tel;
                        klub.Year = year;
                        await db.SaveChangesAsync();
                        return Ok("good");
                    }

                    return Ok("chislo");
                }
                else
                {
                    return Ok("n");
                }


            }

            else
            {
                return Ok("null");
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPost("About/Edit")]
        public async Task<IActionResult> Edit(int sportId, string sportabout, string sportyear)
        {

            if (sportabout != null && sportyear != null && sportId != 0 )
            {
                
                    if (int.TryParse(sportyear, out int numericValue))
                    {
                    Sports sports = db.Sports.FirstOrDefault(c => c.Id == sportId);
                    sports.About = sportabout;
                    sports.Year = numericValue;
                      await db.SaveChangesAsync();
                        return Ok("good");
                    }

                    return Ok("chislo");
              


            }

            else
            {
                return Ok("null");
            }

        }
    }
}
