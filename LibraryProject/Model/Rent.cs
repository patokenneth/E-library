using LibraryProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Model
{
    public class Rent
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NIN { get; set; }
        //Whether checkout or check in
        public string OperationName { get; set; }
        public decimal? Penaltyfee { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime ReturnDate { get; set; } = DateManoeuvre.Businessdays(DateTime.Now, 10);
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
