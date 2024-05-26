using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//using Data.Repositories;
using Service.Services;

using Repository.UnitOfWork;
using Service.Commons;
using Repository.IRepositories;
using Repository.Repositories;
using Service.IServices;
using Microsoft.Extensions.Options;
using System.Reflection;

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

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            string dbContext = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<FuminiHotelManagementContext>(options =>
            {
                options.UseSqlServer(dbContext);
            });
            
            builder.Services.AddSession();
            //builder.Services.AddScoped<CustomerRepository>();
            builder.Services.AddDistributedMemoryCache();


            builder.Services.AddAutoMapper(typeof(AutoMapperService));

            // service

            builder.Services.AddScoped<ICustomerService, CustomerService>(); // Register ICustomerService and CustomerService

            //Repo

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();
            builder.Services.AddScoped<IBookingReservationRepository, BookingReservationRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            builder.Services.AddScoped<IRoomInformationRepository, RoomInformationRepository>();

            //============Configure CORS============//
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://fta-black.vercel.app")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });



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
