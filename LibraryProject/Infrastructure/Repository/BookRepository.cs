using LibraryProject.DataLayer;
using LibraryProject.Infrastructure.Interfaces;
using LibraryProject.Model;
using LibraryProject.ViewModel.BookViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repository
{
    public class BookRepository : IBook
    {
        private readonly Context context;

        public BookRepository(Context context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await context.Books.OrderBy(b => b.PublicationYear).ToListAsync();
        }

        public bool CreateBook(CreateBookViewModel model)
        {
            Book newbook = null;
            newbook = new Book
            {
                Title = model.Title,
                ISBN = model.ISBN,
                PublicationYear = model.PublicationYear,
                CoverPrice = model.CoverPrice,
                IsAvailable = model.IsAvailable
            };

            try
            {
                context.Books.Add(newbook);
                context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
                //throw;
            }



        }

        public async Task<IEnumerable<Book>> SearchBookAsync(string searchvalue = "", bool status = false)
        {

            IList<Book> books = await context.Books.Where(b => b.Title == searchvalue || b.ISBN == searchvalue || b.IsAvailable == status).ToListAsync();
            return books;
        }

        
    }
}
