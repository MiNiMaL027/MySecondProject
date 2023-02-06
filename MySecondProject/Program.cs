using AutoMapper;
using List_Dal;
using List_Dal.Interfaces;
using List_Dal.Repositories;
using List_Service.Mapper;
using List_Domain.ModelDTO; // видали лишнє
using List_Domain.Models;
using List_Service.Interfaces;
using List_Service.Services;
using List_Service.Services.ValidOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySecondProject.Filters;
using System.Web.Http;

namespace MySecondProject
{
    public class Program
    {
        public static void Main(string[] args) // розділи це все пустими лінійками по логічних групах
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAutoMapper(typeof(AppMappingProfile).Assembly); //this will scan entire assembly for profiles
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers(options=>
            options.Filters.Add(typeof(NotImplExceptionFilterAttribute)))
                .AddOData(options => options.Select().OrderBy().Filter().SkipToken().SetMaxTop(10));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();
            builder.Services.AddScoped<ICustomListRepository, CustomListRepository>();
            builder.Services.AddScoped<ICustomListService, CustomListService>();
            builder.Services.AddScoped<IToDoTaskService, ToDoTaskService>();
            builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<ValidOptions>();
    
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("JWT Bearer", new OpenApiSecurityScheme
                {
                    Description = "This is a JWT bearer authentication scheme",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "JWT Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
                options.OperationFilter<AddODataQueryOptionParametersOperationFilter>();
            });
            builder.Services.AddScoped<IAutorizeRepository, AutorizeRepository>();
            builder.Services.AddScoped<IAutorizeService, AutorizeService>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = AuthOptions.ISSUER,
                       ValidateAudience = true,
                       ValidAudience = AuthOptions.AUDIENCE,
                       ValidateLifetime = true,
                       IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                       ValidateIssuerSigningKey = true,
                   };
            });          
            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection,b => b.MigrationsAssembly("MySecondProject")));

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

            app.Run();
        }
    }
}