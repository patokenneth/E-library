using LibraryProject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.DataLayer
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //seed an admin user upon creation of the database
            var user = new User
            {
                Id = 1,
                Name = "Health Station",
                Email = "aba@gmail.com",
                Phone = "090123456789",
                NIN = "8512345678",
                Role = "Admin"
            };

            //builder.Entity("User", builder => { builder.HasData(user);});
            builder.Entity<User>().HasData(user);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Rent> Rents { get; set; }
    }
}
