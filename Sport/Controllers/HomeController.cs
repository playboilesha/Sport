using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sport.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Sport.Controllers
{
    public class HomeController : Controller
    {
      
      
     
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
       

       
       
      
       
        
        
        
       
        public IActionResult MainAdmin()
        {
            return View();
        }
        

        [HttpPost("Home/GetIMT")]
        public IActionResult GetIMT(string bmi_weight,string bmi_hight)
        {
            string status;
            float imt = 0;
            string rez;
            if (float.TryParse(bmi_weight, out float bmi_weight1) && float.TryParse(bmi_hight, out float bmi_hight1))
            {
                imt = (bmi_weight1/(bmi_hight1 * bmi_hight1)) * 10000;
                if (imt >= 0 && imt <=  16)
                {
                    rez = imt + " (Острый дефицит массы)";
                    status = "g";
                }
               
                else if (imt >= 16.1 && imt <= 18.5)
                {
                    rez = imt + " (Недостаточная масса тела)";
                    status = "g";
                }
                else if (imt >= 18.6 && imt <= 25)
                {
                    rez = imt + " (Норма)";
                    status = "g";
                }
                else if (imt >= 25.1 && imt <= 30)
                {
                    rez = imt + " (Избыточная масса тела)";
                    status = "g";
                }
                else if (imt >= 30.1 && imt <= 35)
                {
                    rez = imt + " (Ожирение первой степени)";
                    status = "g";
                }
                else if (imt >= 35.1 && imt <= 40)
                {
                    rez = imt + " (Ожирение второй степени)";
                    status = "g";
                }
                else if (imt >= 40.1 )
                {
                    rez = imt + " (Ожирение третьей степени)";
                    status = "g";
                }
                else
                {
                    rez = "0";
                    status = "ne";
                }
              

                
            }
            else
            {
                rez = "0";
                status = "str";
            }
            return Json(new { imt = rez, status = status });
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
