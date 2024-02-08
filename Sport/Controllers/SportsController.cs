using Microsoft.AspNetCore.Mvc;
using Sport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Controllers
{
    public class SportsController : Controller
    {
        private ApplicationContext db;
        public SportsController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet("Sports/Skate")]
        public IActionResult Skate()
        {
            string name = "Skate";
            Sports sp1 = db.Sports.FirstOrDefault(c => c.Name == name);
            if (sp1 == null)
            {
                // Handle the case when coach with the given ID is not found
                return HttpNotFound();
            }
            ViewBag.Name = sp1.Name;
            ViewBag.About = sp1.About;
            ViewBag.Image = sp1.Image;
            ViewBag.NameCharacteristic1 = sp1.NameCharacteristic1;
            ViewBag.AboutCharacteristic1 = sp1.AboutCharacteristic1;
            ViewBag.NameCharacteristic2 = sp1.NameCharacteristic2;
            ViewBag.AboutCharacteristic2 = sp1.AboutCharacteristic2;
            ViewBag.NameCharacteristic3 = sp1.NameCharacteristic3;
            ViewBag.AboutCharacteristic3 = sp1.AboutCharacteristic3;
            ViewBag.Year = sp1.Year;
            return View();
        }

        private IActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }

        [HttpGet("Sports/Fitness")]
        public IActionResult Fitness()
        {
            string name = "Fitness";
            Sports sp2 = db.Sports.FirstOrDefault(c => c.Name == name);
            if (sp2 == null)
            {
                // Handle the case when coach with the given ID is not found
                return HttpNotFound();
            }
            ViewBag.Name = sp2.Name;
            ViewBag.About = sp2.About;
            ViewBag.Image = sp2.Image;
            ViewBag.NameCharacteristic1 = sp2.NameCharacteristic1;
            ViewBag.AboutCharacteristic1 = sp2.AboutCharacteristic1;
            ViewBag.NameCharacteristic2 = sp2.NameCharacteristic2;
            ViewBag.AboutCharacteristic2 = sp2.AboutCharacteristic2;
            ViewBag.NameCharacteristic3 = sp2.NameCharacteristic3;
            ViewBag.AboutCharacteristic3 = sp2.AboutCharacteristic3;
            ViewBag.Year = sp2.Year;
            return View();
        }
        [HttpGet("Sports/Box")]
        public IActionResult Box()
        {
            string name = "Box";
            Sports sp3 = db.Sports.FirstOrDefault(c => c.Name == name);
            if (sp3 == null)
            {
                // Handle the case when coach with the given ID is not found
                return HttpNotFound();
            }
            ViewBag.Name = sp3.Name;
            ViewBag.About = sp3.About;
            ViewBag.Image = sp3.Image;
            ViewBag.NameCharacteristic1 = sp3.NameCharacteristic1;
            ViewBag.AboutCharacteristic1 = sp3.AboutCharacteristic1;
            ViewBag.NameCharacteristic2 = sp3.NameCharacteristic2;
            ViewBag.AboutCharacteristic2 = sp3.AboutCharacteristic2;
            ViewBag.NameCharacteristic3 = sp3.NameCharacteristic3;
            ViewBag.AboutCharacteristic3 = sp3.AboutCharacteristic3;
            ViewBag.Year = sp3.Year;
            return View();
        }
    }
}
