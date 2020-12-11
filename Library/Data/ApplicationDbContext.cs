using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Usage> Usages { get; set; }
        public DbSet<UsageStatus> UsageStatuses { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }









    }
}
