using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workout_Tracker.Models
{
    public class Workout
    {

        public int ID { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }

        public bool Completed { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public IList<Exercise> Exercises { get; set; }

        public Workout(int id) {
            Date = DateTime.Now;
            Notes = null;
            Completed = false;
            UserID = id;
        }

        public Workout() {

        }
    }
}
