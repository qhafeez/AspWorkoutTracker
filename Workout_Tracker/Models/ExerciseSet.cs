using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workout_Tracker.Models
{
    public class ExerciseSet
    {

        public ExerciseSet()
        {

            Reps = 0;

        }
        public int ID { get; set; }

        //for the set maybe it could be set to zero to start
        public int Reps { get; set; }

        public int MaxReps { get; set; }

        public bool Completed { get; set; }

        public int ExerciseID { get; set; }
        public Exercise Exercise { get; set; }
    }
}