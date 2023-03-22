using Hangfire;
using Hangfire.SqlServer;
using List_Dal;
using List_Dal.Interfaces;
using List_Dal.Repositories;
using List_Domain.Models;
using List_Service.BackgroundTasks;
using List_Service.Filters;
using List_Service.Interfaces;
using List_Service.Mapper;
using List_Service.Services;
using List_Service.Services.ValidOptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

namespace MySecondProject
{
    public class Program
    {
        public static void Main(string[] args) 
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(AppMappingProfile).Assembly);

            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers(options =>
            options.Filters.Add(typeof(NotImplExceptionFilterAttribute)))
                .AddOData(options => options.Select().OrderBy().Filter().SkipToken().SetMaxTop(10));

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDependencyInjectionServices();
     
                builder.Services.AddSwaggerGen(options =>
            {
                //options.AddSecurityDefinition("JWT Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "This is a JWT bearer authentication scheme",
                //    In = ParameterLocation.Header,
                //    Scheme = "Bearer",
                //    Type = SecuritySchemeType.Http
                //});

                //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme{
                //            Reference = new OpenApiReference{
                //                Id = "JWT Bearer",
                //                Type = ReferenceType.SecurityScheme
                //            }
                //        }, new List<string>()
                //    }
                //});
                options.OperationFilter<AddODataQueryOptionParametersOperationFilter>();
            });

            builder.Services.AddScoped<ILoginService, LoginService>();        
            
            var connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection, b =>
                b.MigrationsAssembly("List_Dal")),
                ServiceLifetime.Transient);

            builder.Services.AddHangfire(hangfire =>                                       
            {
                hangfire.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                hangfire.UseSimpleAssemblyNameTypeSerializer();
                hangfire.UseRecommendedSerializerSettings();
                hangfire.UseColouredConsoleLogProvider();
                hangfire.UseSqlServerStorage(
                    connection,
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    });

                var server = new BackgroundJobServer(new BackgroundJobServerOptions
                {
                    ServerName = "hangfire",
                });

                RecurringJob.AddOrUpdate<TaskRetentionPoleBackgroundTask>(x => x.Run(), Cron.Daily);
                RecurringJob.AddOrUpdate<ListRetentionPoleBackgroundTask>(x => x.Run(), Cron.Daily);
                RecurringJob.AddOrUpdate<UserRetentionPoleBackgroundTask>(x => x.Run(), Cron.Daily);
            });
          
            builder.Services.AddIdentity<User,IdentityRole<int>>(options => options.SignIn.RequireConfirmedAccount = true)
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

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
           
            app.MapControllers();

            app.UseHangfireDashboard();

            app.Run();
        }
    }
}