using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Workout_Tracker.Models;
using Workout_Tracker.Data;
using Microsoft.EntityFrameworkCore;
using Workout_Tracker.Objects;
using Workout_Tracker.ViewModels;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;
using Workout_Tracker.Attributes;


namespace Workout_Tracker.Controllers
{

    [SessionTimeoutAttribute]
    public class WorkoutController : Controller
    {
        private ApplicationDbContext context; 


        public WorkoutController(ApplicationDbContext dbContext) {
            context = dbContext;
        }

         public IActionResult Index()
         {
            //id is the workout id

            string userId = HttpContext.Session.GetString("userId");
            
            //string userId = Request.Cookies["UserID"];
            //int converted = Convert.ToInt32(userId);

            //check to see if a current workout exists
            //if it does, proceed with the code below
            Workout currentWorkout = context.Workouts.Where(c => c.User.GoogleIdString == userId && c.Completed == false).SingleOrDefault();

            List<Exercise> exerciseList;

            if (currentWorkout != null && currentWorkout.ID != 0)
            {

                exerciseList = context.Exercises.Where(c => c.Workout.Completed == false && c.Workout.ID == currentWorkout.ID).Include(e => e.Workout).Include(e => e.Sets).ToList();
            }
            else
            {
                //this creates the new workout
                CreateNewWorkout(userId);


                currentWorkout = context.Workouts.Where(c => c.User.GoogleIdString == userId && c.Completed == false).SingleOrDefault();

                //this has to fetch the current workout.  there is only one current workout per user at any given time and is denoted
                //by the completed property.  false means it is incomplete and is the current workout
                exerciseList = context.Exercises.Where(c => c.Workout.Completed == false && c.Workout.User.GoogleIdString == userId).Include(e => e.Workout).Include(e => e.Sets).ToList();

            }

            //else use createNewWorkout method and then proceed with code below



            List<ExerciseType> exercises = context.ExerciseTypes.ToList();

            string notes = context.Workouts.Where(c => c.Completed == false && c.User.GoogleIdString == userId).Select(c => c.Notes).SingleOrDefault();


            WorkoutViewModel workoutViewModel = new WorkoutViewModel()
            {
                
                Workout = currentWorkout,

                exerciseList = exerciseList,

                addExerciseViewModel = new AddExerciseViewModel() {

                    ExercisesTypes = new List<SelectListItem>()
                },

                notesViewModel = new NotesViewModel() {
                    Notes = notes
                }

            };

            foreach (ExerciseType ex in exercises)
            {
                workoutViewModel.addExerciseViewModel.ExercisesTypes.Add(new SelectListItem
                {
                    Value = ex.ID.ToString(),
                    Text = ex.Name

                });

            }
                                                                              
            return  View("Workout",workoutViewModel);
         }


        /*
         * no longer needed as this functionality was moved to the index method
         * 
        public IActionResult WorkoutSelector() {

            IList<ExerciseType> exerciseTypes = context.ExerciseTypes.ToList();

            AddExerciseViewModel addExerciseViewModel1 = new AddExerciseViewModel(exerciseTypes);

            return Ok(exerciseTypes);

            //return View("_WorkoutSelector", addExerciseViewModel1);
        }
        */

        public void CreateNewWorkout(string id)
        {
            int userId = context.Users.Where(c => c.GoogleIdString == id).Select(c=>c.ID).SingleOrDefault();

            Workout newWorkout = new Workout(userId);
            context.Workouts.Add(newWorkout);
            context.SaveChanges();
        }

        public JsonResult GetLastAddedExercise() {

            //string userId = Request.Cookies["UserID"];
            string userId = HttpContext.Session.GetString("userId");


            int workoutId = context.Workouts.Where(c => c.Completed == false && c.User.GoogleIdString == userId).Select(c => c.ID).SingleOrDefault();

            Exercise last = context.Exercises.Where(c => c.WorkoutID == workoutId && c.Workout.User.GoogleIdString == userId).Include(c => c.Sets).Last();

            return Json(new { exercise = last });
        }
        
        [HttpPost]
        public JsonResult WorkoutSelector([FromBody] AddExerciseViewModel model)
        {

            if (ModelState.IsValid) {



                Exercise newExercise = new Exercise
                {

                    Weight = model.Weight,

                    Name = context.ExerciseTypes.Where(e => e.ID == model.ExerciseTypeID).Single().Name,

                    WorkoutID = context.Workouts.Where(e => e.Completed == false).Select(c => c.ID).SingleOrDefault(),

                };

                context.Exercises.Add(newExercise);
                context.SaveChanges();


                for (var i = 0; i < model.Sets; i++) {
                    ExerciseSet newExerciseSet = new ExerciseSet
                    {

                        MaxReps = model.Reps,
                        ExerciseID = context.Exercises.Last().ID

                    };
                    context.Exercise_Sets.Add(newExerciseSet);
                }
                context.SaveChanges();

                return GetLastAddedExercise();
            }


            return Json(new { success = false, issue = model, errors = ModelState.Values.Where(i => i.Errors.Count > 0) }); 

            //return View("_WorkoutSelector", addExerciseViewModel1);
        }


        [HttpPost]
        public IActionResult addRep([FromBody] ExerciseAndSet model)
        {
            //id is the setid

            Exercise exercise = context.Exercises.Where(c => c.ID == model.ExerciseID).Single();
            //Exercise exercise = context.Exercises.Where(c => c.ID == model.ExerciseID).Single();

            ExerciseSet exerciseSet = context.Exercise_Sets.Where(c => c.ID == model.SetID).Single();

            //once a rep is added, the set is marked as completed
            exerciseSet.Completed = true;

            context.SaveChanges();


            if (exerciseSet.Reps >= 1)
            {
                //if the current reps are equal to the max, set it back to zero
                exerciseSet.Reps--;

            }
            else
            {
                //else increase reps by one
                exerciseSet.Reps = exerciseSet.MaxReps;
            }


            //this code is checking the number of completed sets for the specified exercise
            int completedCount = context.Exercise_Sets.Where(c => c.ExerciseID == model.ExerciseID && c.Completed == true).Count();
            int setsCount = context.Exercise_Sets.Where(c => c.ExerciseID == model.ExerciseID).Count();

            if (completedCount == setsCount)
            {
                //if completed count matched the number of sets in the exercise, the exercise is considered completed
                exercise.Completed = true;
            }

            context.SaveChanges();


            List<object> data = new List<object>();
            data.Add(exercise);
            data.Add(exerciseSet);

            return Ok(data);
        }

        [HttpPost]
        public JsonResult addNotes([FromBody] NotesViewModel model) {

            if (ModelState.IsValid)
            {

                //the workout id will be the id of the current workout
                var row = context.Workouts.Where(c => c.ID == 1).SingleOrDefault();

                row.Notes = model.Notes;

                context.SaveChanges();

                return Json(new { hello = true });
            }
            else
            {

                return Json(new { success = false, issue = model, errors = ModelState.Values.Where(i => i.Errors.Count > 0) });
            }
        }

        [HttpPost]
        public IActionResult CompleteWorkout()
        {

            //when completed this will get the current workout by retreiving the current 
            //workout that corresponds to the userID in the cookies
            Workout currentWorkout = context.Workouts.Where(c => c.Completed == false).SingleOrDefault();

            currentWorkout.Completed = true;
            context.SaveChanges();

            //this will return to the logged-in home page 
            return RedirectToAction("Index", "Home");
        }



    }
}