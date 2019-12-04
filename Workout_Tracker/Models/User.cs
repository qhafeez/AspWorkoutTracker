using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workout_Tracker.Models
{
    public class User
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string GoogleIdString { get; set; }


        IList<Workout> Workouts { get; set; }

    }
}
