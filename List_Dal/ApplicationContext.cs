using List_Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using List_Domain.Models.NotDbEntity;
using Microsoft.AspNetCore.Identity;

namespace List_Dal
{
    public class ApplicationContext :  IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<CustomList> CustomLists { get; set; }

        public DbSet<ToDoTask> ToDoTasks { get; set; }

        public DbSet<Settings> Settings { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {

        }     
    }
}