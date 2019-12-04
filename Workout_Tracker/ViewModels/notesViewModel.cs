using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Workout_Tracker.ViewModels
{
    public class NotesViewModel
    {

        [Required(ErrorMessage="You forgot to add some notes.")]
        public string Notes { get; set; }

    }
}
