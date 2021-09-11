using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.ViewModel.RentViewModel
{
    public class CheckInViewModel
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string[] ISBN { get; set; }
    }
}
