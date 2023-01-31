using List_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace List_Dal
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CustomList> CustomLists { get; set; }
        public DbSet<ToDoTask> ToDoTasks { get; set; }
        public DbSet<EmailConfirmationCode> EmailConfirmationCodes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ToDoTask task1 = new ToDoTask();
            //task1.Id = 1;
            //task1.Title = "Shop";
            //task1.Description = "I have to go shopping";
            //task1.CreationDate = DateTime.Now;
            //task1.DueToDate = (DateTime.Now + TimeSpan.FromDays(4)).Date;
            //task1.IsDeleted = false;
            //task1.IsCompleted = false;
            //task1.IsFavorite = false;
            //task1.Importance = Importance.Low;
            //task1.CustomListId = null;
            //task1.UserId = 1;

            //ToDoTask task2 = new ToDoTask();
            //task2.Id = 2;
            //task2.Title = "Learning";
            //task2.Description = "I have to learn English";
            //task2.CreationDate = DateTime.Now;
            //task2.DueToDate = (DateTime.Now + TimeSpan.FromDays(15)).Date;
            //task2.IsDeleted = false;
            //task2.IsCompleted = false;
            //task2.IsFavorite = false;
            //task2.Importance = Importance.Normal;
            //task2.CustomListId = null;
            //task2.UserId = 1;

            //ToDoTask task3 = new ToDoTask();
            //task3.Id = 3;
            //task3.Title = "AspNet";
            //task3.Description = "I have to learn Asp.Net Core";
            //task3.CreationDate = DateTime.Now;
            //task3.DueToDate = (DateTime.Now + TimeSpan.FromDays(10)).Date;
            //task3.IsDeleted = false;
            //task3.IsCompleted = false;
            //task3.IsFavorite = true;
            //task3.Importance = Importance.High;
            //task3.CustomListId = null;
            //task3.UserId = 1;

            //CustomList list = new CustomList();
            //list.Id = 1;
            //list.IsDeleted = false;
            //list.Name = "List 1";
            //list.UserId = 1;

            //ToDoTask task4 = new ToDoTask();
            //task4.Id = 4;
            //task4.Title = "CustomList Task1";
            //task4.Description = "Description";
            //task4.CreationDate = DateTime.Now - TimeSpan.FromDays(1);
            //task4.DueToDate = DateTime.Now;
            //task4.IsDeleted = false;
            //task4.IsCompleted = false;
            //task4.IsFavorite = true;
            //task4.Importance = Importance.High;
            //task4.CustomListId = 1;
            //task4.UserId = 1;

            //User u = new User()
            //{
                //Id = 1,
                //Name = "Andriy",
                //Email = "smeldoc@gmail.com",
                //Password = "PIrgcQjPgUpuyF8l+7CEo2bT+eebTyKYc+f1fDoGjLs="
            //};
            //modelBuilder.Entity<User>().HasData(u);
            //modelBuilder.Entity<CustomList>().HasData(list);
            //modelBuilder.Entity<ToDoTask>().HasData(task1, task2, task3, task4);
        }
    }
}