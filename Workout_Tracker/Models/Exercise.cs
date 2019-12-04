using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workout_Tracker.Models
{
    public class Exercise
    {
        public int ID { get; set; }

        public int Weight { get; set; }

        public string Name { get; set; }

        public bool Completed { get; set; }

        public int WorkoutID { get; set; }
        public Workout Workout { get; set; }

        public IList<ExerciseSet> Sets { get; set; }

    }
}
