
/*using Microsoft.EntityFrameworkCore;
using static WebAppRestApi.Models.SecurityService;
using WebAppRestApi.Models;
using WebRestApp.Models.Db;

 

namespace WebRestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

 

            // Add services to the container.

 

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

 

            var app = builder.Build();

 

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

 

            app.UseAuthorization();

 

 

            app.MapControllers();

 

            app.Run();
        }
    }
}*/
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebRestApp.Models;
using WebRestApp.Models.Db;
using static WebRestApp.Models.SecurityService;
//using static WebRestApp.Models.SecurityService;



namespace WebRestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(conf =>
            {
                conf.AddPolicy("policy1", pol => {



                    pol.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
                });
            });

            //Add authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:audience"],
                    ValidIssuer = builder.Configuration["Jwt:issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
                };
            });
            /*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    *//*ValidIssuer = "your-issuer", // Replace with your issuer
                    ValidAudience = "your-audience", // Replace with your audience*//*
                    ValidIssuer = builder.Configuration["Jwt:audience"], // Replace with your issuer
                    ValidAudience = builder.Configuration["Jwt:issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key")) // Replace with your secret key
                };
            });*/

            builder.Services.AddAuthorization(config => {
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                /*config.AddPolicy(Policies.Manager, Policies.ManagerPolicy());
                config.AddPolicy(Policies.Executive, Policies.ExecutivePolicy());*/
            });

            string connectionstring = "Server=localhost;database=MedicineDb;trusted_connection=yes;trustservercertificate=true";
            builder.Services.AddDbContext<MedicineDbContext>(config => config.UseSqlServer(connectionstring));

            //add medicinestockservice as a service for dependency injection
            builder.Services.AddTransient(typeof(MedicineStockService));
            builder.Services.AddTransient(typeof(SecurityService));
            builder.Services.AddTransient(typeof(DataService));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseCors("policy1");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}