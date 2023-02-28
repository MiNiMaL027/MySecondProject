using List_Dal;
using List_Domain.Enums;
using List_Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBaseSeeder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
                .Build();

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer(config.GetConnectionString("DefaultConnection"))
                .Options;

            var dataBase = new ApplicationContext(dbContextOptions);

            dataBase.Database.Migrate();

            dataBase.Settings.Add(new Settings
            {
                AllowNotification = false,
                DefaultList = null
            });

            dataBase.CustomLists.Add(new CustomList
            {
                Name = "ToDoTasks",
                UserId = 1,
            });

            dataBase.ToDoTasks.Add(new ToDoTask()
            {
                Title = "Go to shop",
                Description = "I have to go shop at 9 pm",
                IsFavorite = false,
                DueToDate = DateTime.Now.AddDays(1),
                CreationDate = DateTime.Now,
                Importance = Importance.Normal,
                CustomListId = 1,
                UserId = 1,
            });

            dataBase.SaveChanges();      
        } 
    }
}