using LibraryProject.DTO;
using LibraryProject.Model;
using LibraryProject.ViewModel.RentViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Interfaces
{
    public interface IRent
    {
        Task<BookDetails> GetBookDetailsAsync(int bookid);
        bool CheckOutBooks(string[] ISBN, CheckOutViewModel model);
        bool CheckInBooks(string[] ISBN, string PhoneNumber);
    }
}
