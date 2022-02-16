using FluentValidation.AspNetCore;
using HotelListing.Data;
using HotelListing.Filters;
using HotelListing.Models;
using HotelListing.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace HotelListing
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(service =>
            {
                service.User.RequireUniqueEmail = true;
                service.Password.RequiredLength = 8;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)
        {
            var jwtSettings = Configuration.GetSection("Jwt");
            var key = jwtSettings.GetSection("Key").Value;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = false,
                        ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    };
                });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
           {
               error.Run(async context =>
               {
                   context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                   context.Response.ContentType = "application/json";
                   var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                   if (contextFeature != null)
                   {
                       Log.Error($"Some thing wrong in the {contextFeature.Error}");

                       await context.Response.WriteAsync(new Error
                       {
                           StatusCode = context.Response.StatusCode,
                           Message = "Internal Server Error. Please try again"
                       }.ToString());
                   }
               });
           });
        }

        public static void ConfigureControllerAndFluentValidations(this IServiceCollection services)
        {

            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            })
            .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<HotelValidator>();
                options.RegisterValidatorsFromAssemblyContaining<UserValidator>();
            })
            .AddNewtonsoftJson(op =>
                op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }
    }
}
