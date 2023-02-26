using List_Dal.Interfaces;
using List_Dal.Repositories;
using List_Domain.Models;
using List_Service.Filters;
using List_Service.Interfaces;
using List_Service.Services.ValidOptions;
using List_Service.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using List_Dal;
using List_Service.Mapper;
using Microsoft.AspNetCore.Identity;

namespace MySecondProjectWEB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(AppMappingProfile).Assembly);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews(options =>
            options.Filters.Add(typeof(NotImplExceptionFilterAttribute)))
                .AddOData(options => options.Select().OrderBy().Filter().SkipToken().SetMaxTop(10));

            builder.Services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();
            builder.Services.AddScoped<ICustomListRepository, CustomListRepository>();
            builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();

            builder.Services.AddScoped<ICustomListService, CustomListService>();
            builder.Services.AddScoped<IToDoTaskService, ToDoTaskService>();
            builder.Services.AddScoped<ISettingsService, SettingsService>();

            builder.Services.AddScoped<IChekAuthorization<ToDoTask>, ToDoTaskRepository>();
            builder.Services.AddScoped<IChekAuthorization<CustomList>, CustomListRepository>();
            builder.Services.AddScoped<IChekAuthorization<Settings>, SettingsRepository>();

            builder.Services.AddScoped<IAutorizationService<ToDoTask>, AutorizationService<ToDoTask>>();
            builder.Services.AddScoped<IAutorizationService<CustomList>, AutorizationService<CustomList>>();
            builder.Services.AddScoped<IAutorizationService<Settings>, AutorizationService<Settings>>();

            builder.Services.AddSingleton<ValidOptions>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddTransient<ILoginService, LoginService>();
         
            var connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection, b =>
                b.MigrationsAssembly("List_Dal")),
                ServiceLifetime.Transient);

            builder.Services.AddIdentity<User, IdentityRole<int>>(options => options.SignIn.RequireConfirmedAccount = true)
          .AddEntityFrameworkStores<ApplicationContext>();


            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                options.User.RequireUniqueEmail = false;

                options.Lockout.AllowedForNewUsers = false;

                options.SignIn.RequireConfirmedAccount = false;
            });

            builder.Services.ConfigureApplicationCookie(config =>
            {

            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Login}/{id?}");

            app.Run();
        }
    }
}