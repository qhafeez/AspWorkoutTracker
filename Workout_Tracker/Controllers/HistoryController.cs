using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Workout_Tracker.Data;
using Workout_Tracker.Models;
using Workout_Tracker.Objects;
using Workout_Tracker.Attributes;

namespace Workout_Tracker.Controllers
{
    [SessionTimeoutAttribute]
    public class HistoryController : Controller
    {

        private ApplicationDbContext context;


        public HistoryController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index( )
        {

            //string userId = Request.Cookies["UserID"];
            string userId = HttpContext.Session.GetString("userId");
            //int converted = Convert.ToInt32(userId);

            List<Workout> workouts = context.Workouts.Where(c => c.Completed == true && c.User.GoogleIdString == userId).Include(c => c.Exercises).ToList();

            return View(workouts);
        }

        public IActionResult CompletedWorkout(int id)
        {
            //string userId = Request.Cookies["UserID"];
            string userId = HttpContext.Session.GetString("userId");
            

            List<Exercise>completedWorkoutExercises = context.Exercises.Where(c => c.WorkoutID == id && c.Workout.User.GoogleIdString == userId).Include(c => c.Workout).Include(c => c.Sets).ToList();

            if (completedWorkoutExercises.Count == 0) {
                return RedirectToAction("Index");
            }

            return View(completedWorkoutExercises);

        }


        [HttpPost]
        public IActionResult DeleteWorkout(int id)
        {

             
            Workout workoutToDelete = context.Workouts.Where(c => c.ID == id).Single();

            context.Remove(workoutToDelete);
            context.SaveChanges();
            
    return RedirectToAction("Index");
        }
    }
}