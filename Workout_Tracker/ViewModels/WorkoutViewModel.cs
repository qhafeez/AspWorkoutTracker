using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workout_Tracker.ViewModels;
using Workout_Tracker.Models;


namespace Workout_Tracker.ViewModels
{
    public class WorkoutViewModel
    {
        public Workout Workout { get; set; }
        public List<Exercise> exerciseList { get; set; }
        public AddExerciseViewModel addExerciseViewModel { get; set; }

        public NotesViewModel notesViewModel { get; set; }

    }
}
