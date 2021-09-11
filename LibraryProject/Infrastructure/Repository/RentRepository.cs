using LibraryProject.DataLayer;
using LibraryProject.DTO;
using LibraryProject.Infrastructure.Interfaces;
using LibraryProject.Model;
using LibraryProject.ViewModel.RentViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repository
{
    public class RentRepository : IRent
    {
        private readonly Context context;

        public RentRepository(Context context)
        {
            this.context = context;
        }

        //Fetch the details (including checkout history) of a particular book
        public async Task<BookDetails> GetBookDetailsAsync(int bookid)
        {
            //check if the book has ever been rented.

            if(!context.Rents.Any(r => r.BookId == bookid))
            {
                BookDetails deets = await context.Books.Select(b => new BookDetails
                {
                    Title = b.Title,
                    ISBN = b.ISBN,
                    isAvailable = true,
                    PublicationYear = b.PublicationYear,
                    CoverPrice = b.CoverPrice,
                    Activity = null
                }).FirstOrDefaultAsync();

                return deets;
            }
            else
            {
                //Book has a checkout history

                IQueryable<Rent> RentCollection = context.Rents.Include(b => b.Book).Where(r => r.BookId == bookid);

                var activitydetails = new List<BookActivity>();

                foreach (var item in RentCollection.OrderByDescending(o => o.Id).ToList())
                {
                    activitydetails.Add(new BookActivity { Name = item.OperationName, CheckIn = item.ReturnDate, CheckOut = item.CheckoutDate });
                }


                    BookDetails details =  RentCollection.Select(r => new BookDetails
                                           {
                                                Title = r.Book.Title,
                                                ISBN = r.Book.ISBN,
                                                PublicationYear = r.Book.PublicationYear,
                                                CoverPrice = r.Book.CoverPrice,
                                                isAvailable = r.Book.IsAvailable,
                                                Activity = activitydetails

                                            }).FirstOrDefault();


                return details;
            }
             
        }

        public bool CheckOutBooks(string[] ISBN, CheckOutViewModel model)
        {
            using (var ctx = context.Database.BeginTransaction())
            {
                try
                {
                    //check if a user has account in the system. Register them, if not.
                    if (!IsUserExisting(model.Email, model.NIN))
                    {
                        User newuser = new User()
                        {
                            Name = model.Name,
                            NIN = model.NIN,
                            Email = model.Email,
                            Phone = model.Phone,
                            Role = "user"
                        };

                        context.Users.Add(newuser);
                        context.SaveChanges();
                    }

                    //Record the book activity in the Rent table using the book's ISBN.

                    foreach (var isbn in ISBN)
                    {
                        //Check that a book with that ISBN exists.
                        var present = context.Books.Any(b => b.ISBN == isbn);
                        
                        if (present)
                        {
                            Rent newactivity = new Rent
                            {
                                Fullname = model.Name,
                                Email = model.Email,
                                Phone = model.Phone,
                                NIN = model.NIN,
                                OperationName = "Check Out",
                                Penaltyfee = null,
                                CheckoutDate = DateTime.Now,
                                BookId = FindByISBN(isbn)
                            };

                            //update the status of the book accordingly
                            bool bookupdate = UpdateBookStatus(status: false, ISBN: isbn);

                            //persist in the data store
                            context.Rents.Add(newactivity);
                            context.SaveChanges();
                        }

                        else
                        {
                            //Rollback transaction and raise exception to a wrong ISBN
                            ctx.Rollback();
                            throw new Exception($"Invalid ISBN entered. Crosscheck {isbn}");
                        }
                       
                    }

                    ctx.Commit();

                    return true;
                }
                catch (Exception)
                {
                    ctx.Rollback();
                    return false;
                    //throw;
                }

            }
           
        }


        public bool CheckInBooks(string[] ISBN, string PhoneNumber)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in ISBN)
                    {
                        Rent rentobject = context.Rents.Include(b => b.Book).Where(r => r.Book.ISBN == item && r.Phone == PhoneNumber && r.OperationName == "Check Out").FirstOrDefault();

                        DateTime DefaultReturnDate = rentobject.ReturnDate;
                        DateTime NewReturnDate = DateTime.Now;

                        rentobject.ReturnDate = NewReturnDate;
                        rentobject.OperationName = "Check In";

                        //update the status of the book to available
                        bool updatebookstatus = UpdateBookStatus(true, item);


                        //check if the book was returned late.
                        bool status = IsBookReturnedLate(DefaultReturnDate, NewReturnDate);
                        if (status)
                            rentobject.Penaltyfee = DateManoeuvre.daydifference(DefaultReturnDate, NewReturnDate) * 200;

                        context.SaveChanges();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    //Rollback transaction if error occurs

                    transaction.Rollback();
                    return false;
                    //throw;
                }
            }
        }


        public bool IsUserExisting(string email, string NIN)
        {
            return context.Users.Any(u => u.Email == email && u.NIN == NIN);
        }

        public int FindByISBN(string isbn)
        {
            
            
            return context.Books.Where(b => b.ISBN == isbn).FirstOrDefault().Id;
            
        }

        public bool UpdateBookStatus(bool status, string ISBN)
        {
            var book = context.Books.Where(b => b.ISBN == ISBN).FirstOrDefault();
            book.IsAvailable = status;
            context.SaveChanges();

            return true;
        }

        public bool DidThisUserBorrow(string phonenumber, string ISBN)
        {
            return context.Rents.Include(b => b.Book).Any(r => r.Book.ISBN == ISBN && r.Phone == phonenumber);
        }

        public bool IsBookReturnedLate(DateTime returndate, DateTime currentdate)
        {
            if (currentdate > returndate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
