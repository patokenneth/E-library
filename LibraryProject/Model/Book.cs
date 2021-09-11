using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublicationYear { get; set; }
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
    }
}
