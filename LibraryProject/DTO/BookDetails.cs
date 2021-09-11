using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.DTO
{
    public class BookDetails
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublicationYear { get; set; }
        public decimal CoverPrice { get; set; }
        public bool isAvailable { get; set; }
        public IList<BookActivity> Activity { get; set; }
    }

   
}
