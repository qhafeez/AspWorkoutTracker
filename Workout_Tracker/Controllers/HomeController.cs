using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Workout_Tracker.Models;
using Workout_Tracker.Attributes;
using Workout_Tracker.ViewModels;
using Workout_Tracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;

namespace Workout_Tracker.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext context;

        public HomeController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }


        [HomPageCookieCheck]
        public IActionResult Index()
        {
            //inside here it must check to see whether the user is logged in and render the appropriate view
            //or maybe the filter will do that

            return View();
        }

        [SessionTimeoutAttribute]
        public IActionResult HomePage() {

            string userId = HttpContext.Session.GetString("userId");

            string userName = context.Users.Where(c => c.GoogleIdString == userId).Select(c => c.Name).FirstOrDefault();
            Workout ongoingWorkout = context.Workouts.Where(c => c.Completed == false && c.User.GoogleIdString == userId).Include(c=>c.User).SingleOrDefault();
            
            List<Exercise> workoutToDisplay;

            if (ongoingWorkout != null)
            {
                workoutToDisplay = context.Exercises.Where(c => c.Workout.Completed == false && c.Workout.User.GoogleIdString == userId).Include(c => c.Workout).Include(c => c.Sets).ToList();
            }
            else
            {
                int mostRecentWorkoutID = context.Workouts.Where(c => c.Completed == true && c.User.GoogleIdString == userId).OrderByDescending(c => c.Date).Select(c => c.ID).FirstOrDefault();
                workoutToDisplay = context.Exercises.Where(c => c.Workout.ID == mostRecentWorkoutID).Include(c => c.Workout).Include(c => c.Sets).ToList();
            }
           

            //get workoutID of the most recent completed workout
/*
            if (workoutToDisplay.Count == 0) {
                int mostRecentWorkoutID = context.Workouts.Where(c => c.Completed == true && c.User.GoogleIdString == userId).OrderByDescending(c => c.Date).Select(c => c.ID).FirstOrDefault();

                if (mostRecentWorkoutID != 0)
                {
                    workoutToDisplay = context.Exercises.Where(c => c.Workout.ID == mostRecentWorkoutID).ToList();
                }

            }
            */

            HomePageViewModel homePageViewModel = new HomePageViewModel()
            {
                ExercisesToDisplay = workoutToDisplay,
                Name = userName

            };

            homePageViewModel.OnGoingWorkout = ongoingWorkout == null ? false : true;
            
            return View("HomePage", homePageViewModel);
        }

       

        /*
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        */
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
