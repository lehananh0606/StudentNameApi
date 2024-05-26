using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectAPI.Services;
using StudentNameApi.Interface;
using Data.Repositories;

namespace StudentNameApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API Name", Version = "v1" });
            });

            string dbContext = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<FuminiHotelManagementContext>(options =>
            {
                options.UseSqlServer(dbContext);
            });
            builder.Services.AddScoped<ICustomerService, CustomerService>(); // Register ICustomerService and CustomerService

            builder.Services.AddSession();
            builder.Services.AddScoped<CustomerRepository>();
            builder.Services.AddDistributedMemoryCache();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
