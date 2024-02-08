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
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace Sport.Controllers
{
    
    public class CoachController : Controller
    {
        private ApplicationContext db;
        public CoachController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet("Coach/Coach")]
        public async Task<IActionResult> Coach()
        {
            return View(await db.Coach.ToListAsync());
        }
        [HttpGet("Coach/CoachPage")]
        public IActionResult CoachPage(int userId)
        {
            Coach coach = db.Coach.Include(c=>c.User).FirstOrDefault(c => c.Id == userId);
            if (coach == null)
            {
                // Handle the case when coach with the given ID is not found
                return HttpNotFound();
            }
           /* var trainer = db.Coach.FirstOrDefault(t => t.Id == userId);

            if (trainer == null)
            {
                return NotFound();
            }*/

            ViewBag.TrainerId = coach.Id;
            ViewBag.TrainerName = coach.FirstName;
            
            var model = db.Comment.Where(c => c.coach.Id == userId).ToList();
            ViewBag.Comments = model;
            
            ViewBag.Coach = coach;
           
            return View();
        }

        private IActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }
       /* [HttpGet("Coach/Index")]
        public IActionResult Index(int id)
        {
            var trainer = db.Coach.FirstOrDefault(t => t.Id == id);
            
            if (trainer == null)
            {
                return NotFound();
            }
           
            ViewBag.TrainerId = trainer.Id;
            ViewBag.TrainerName = trainer.FirstName;
            var model = db.Comment.Where(c => c.coach.Id == id).ToList();
            ViewBag.Comments = model;
            
            return View();
        }*/

        [HttpPost("Coach/AddComment")]
        public async Task<IActionResult> AddComment(int id,Comment comment)
        {
            Coach coach1 = db.Coach.FirstOrDefault(c => c.Id == id);
            
            if (ModelState.IsValid)
            {
                Comment comment1 = new Comment() { coach = coach1, CreatedAt = DateTime.Now, Comments = comment.Comments, UserName = comment.UserName };
                
                // Add the new comment to the database
                await db.Comment.AddAsync(comment1);
                await db.SaveChangesAsync();

                return RedirectToAction("CoachPage", new { userId = id }); // Redirect to the desired page after saving
            }
            

            return View(comment);
        }
        [Authorize(Roles = "admin")]
        [HttpPost("Coach/DeleteComment")]
        public async Task<IActionResult> DeleteComment(int id,int isa)
        {
            Comment comment = db.Comment
         .Where(o => o.Id == id)
         .FirstOrDefault();

            db.Comment.Remove(comment);
            await db.SaveChangesAsync();
            return RedirectToAction("CoachPage", new { userId = isa });
        }
        /*
        private async Task NotifyNewComment(Comment comment)
        {
            var service = HttpContext.RequestServices.GetService(typeof(IHubContext<CommentsHub>)) as IHubContext<CommentsHub>;
            await service.Clients.All.SendAsync("ReceiveComment", comment.coach.Id, comment.Comments);
        }*/
    }
}
