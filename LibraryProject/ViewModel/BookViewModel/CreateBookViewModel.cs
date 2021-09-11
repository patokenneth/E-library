using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.ViewModel.BookViewModel
{
    public class CreateBookViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string PublicationYear { get; set; }
        [Required]
        public decimal CoverPrice { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
    }
}
