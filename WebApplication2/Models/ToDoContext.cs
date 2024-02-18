using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models
{
    public class ToDoContext: DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
                
        }
 
        public DbSet<User> Users { get; set; }
        public DbSet<OrderInformation> OrderInformation { get; set; }

        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<BookingInformation> BookingInformation { get; set; }
   
   
    }
}
