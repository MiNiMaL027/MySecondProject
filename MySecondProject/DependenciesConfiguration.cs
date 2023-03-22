using List_Dal.Interfaces;
using List_Dal.Repositories;
using List_Domain.Models;
using List_Service.Interfaces;
using List_Service.Services.ValidOptions;
using List_Service.Services;

namespace MySecondProject
{
    public static class DependenciesConfiguration
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {
           services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();
           services.AddScoped<ICustomListRepository, CustomListRepository>();
           services.AddScoped<ISettingsRepository, SettingsRepository>();
           
           services.AddScoped<ICustomListService, CustomListService>();
           services.AddScoped<IToDoTaskService, ToDoTaskService>();
           services.AddScoped<ISettingsService, SettingsService>();
           services.AddScoped<IUserService, UserService>();
           services.AddScoped<ISendEmailService, SendEmailService>();
           
           services.AddScoped<IChekAuthorization<ToDoTask>, ToDoTaskRepository>();
           services.AddScoped<IChekAuthorization<CustomList>, CustomListRepository>();
           services.AddScoped<IChekAuthorization<Settings>, SettingsRepository>();
           
           services.AddScoped<IAutorizationService<ToDoTask>, AutorizationService<ToDoTask>>();
           services.AddScoped<IAutorizationService<CustomList>, AutorizationService<CustomList>>();
           services.AddScoped<IAutorizationService<Settings>, AutorizationService<Settings>>();
           
           services.AddSingleton<ValidOptions>();
        }  
    }
}
