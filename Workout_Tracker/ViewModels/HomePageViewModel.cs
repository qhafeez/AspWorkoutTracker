using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout_Tracker.Models;

namespace Workout_Tracker.ViewModels
{
    public class HomePageViewModel
    {
        public bool OnGoingWorkout;


        public string Name;
        public List<Exercise> ExercisesToDisplay;

       
    }
}
