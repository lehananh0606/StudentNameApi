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
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

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
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RoleClaimType = "Role",
                    NameClaimType = ClaimTypes.NameIdentifier
                };
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
            builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
            builder.Services.AddScoped<IBookingDetailService, BookingDetailService>();
            builder.Services.AddScoped<IRoomInformationService, RoomInformationService>();
            builder.Services.AddScoped<IBookingReservationService, BookingReservationService>();
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
