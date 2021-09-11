using LibraryProject.Model;
using LibraryProject.ViewModel.BookViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Interfaces
{
    public interface IBook
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        bool CreateBook(CreateBookViewModel model);
        Task<IEnumerable<Book>> SearchBookAsync(string searchValue = "", bool status = false);


    }
}
