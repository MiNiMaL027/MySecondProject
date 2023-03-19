using List_Dal;
using List_Service.Mapper;
using List_Service.Services.ValidOptions;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using MySecondProject.Filters;

namespace MySecondProjectWebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(AppMappingProfile).Assembly);

            builder.Services.AddControllersWithViews(options =>
            options.Filters.Add(typeof(NotImplExceptionFilterAttribute)))
                .AddOData(options => options.Select().OrderBy().Filter().SkipToken().SetMaxTop(10));

            builder.Services.AddSingleton<ValidOptions>();

            var connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection, b =>
                b.MigrationsAssembly("List_Dal")),
                ServiceLifetime.Transient);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}