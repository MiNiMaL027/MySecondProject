using List_Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace List_Dal
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<User> Users { get; set; }

        public DbSet<CustomList> CustomLists { get; set; }

        public DbSet<ToDoTask> ToDoTasks { get; set; }

        public DbSet<Settings> Settings { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            
        }     
    }
}