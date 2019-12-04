using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Workout_Tracker.Models;

namespace Workout_Tracker.ViewModels
{
    public class AddExerciseViewModel
    {
       
        [Display(Name ="Weight")]
        [Required(ErrorMessage="You must enter a weight.")]
        [Range(1, 1000, ErrorMessage = "The value must be between 0 and 1000")]
        public int Weight { get; set; }


        [Required(ErrorMessage = "You must select an exercise")]
        public int ExerciseTypeID { get; set; }

        [Range(1, 50, ErrorMessage = "The value must be between 0 and 50")]
        public int Reps { get; set; }

        //this sets will be used to create the sets in the Exercise_Sets table
        [Required(ErrorMessage = "You must select the number of sets")]
        public int Sets { get; set; }


        //workoutID will come from the cookie that is set when the workout is created
        public int WorkoutID { get; set; }

        //public int ExerciseTypeID { get; set; }

             

        public List<SelectListItem> ExercisesTypes { get; set; }

        public AddExerciseViewModel(IEnumerable<ExerciseType> exercises) {

            ExercisesTypes = new List<SelectListItem>();

            foreach (ExerciseType ex in exercises)
            {
                ExercisesTypes.Add(new SelectListItem
                {
                    Value = ex.ID.ToString(),
                    Text = ex.Name
                                       
                });
              
            }

            var types = ExercisesTypes;

        }

        public AddExerciseViewModel() {

        }

    }
}
