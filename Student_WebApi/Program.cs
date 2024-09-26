using Student_WebApi.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
//using NLog;

//using Entities;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Configuration;
//using Entities.DataTransferObjects;
//using Entities.Models;
//using Mapster;
//using Services;

namespace Student_WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            // Add services to the container.

            //try
            //{
            //    LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            //}
            //catch (Exception Error)
            //{
            //    Console.WriteLine(Error.Message);
            //}

            builder.Services.ConfigureIISIntegration();
            builder.Services.ConfigureLoggerService();

            builder.Services.ConfigureMsSqlContext(builder.Configuration);

            builder.Services.ConfigureRepositoryWrapper();
            builder.Services.ConfigureServiceLayerWrappers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((host) => true));
            });

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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
