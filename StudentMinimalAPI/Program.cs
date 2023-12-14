using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Models;
using Repositories;

namespace DemoMinimumAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // added next 2 lines for Swagger integration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("ProjectDatabase");

            WebApplication app = builder.Build();

            // added next 2 lines for Swagger integration
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapGet("/", () => "Hello World!");

            app.MapGet("/students", (HttpContext context) =>
            {
                StudentRepository repository = new StudentRepository(connectionString);
                List<Student> students = repository.GetAll();   // or through <=> Service layer <=> DataAccess layer

                context.Response.StatusCode = StatusCodes.Status200OK;

                return students;
            });

            app.MapGet("/students/{id}", (int id, HttpContext context) =>
            {
                StudentRepository repository = new StudentRepository(connectionString);
                Student student = repository.GetById(id);
                
                context.Response.StatusCode = StatusCodes.Status200OK;

                return System.Threading.Tasks.Task.FromResult(student);
            });

            app.MapPost("/students", async (HttpContext context) =>
            {
                var student = await context.Request.ReadFromJsonAsync<Student>();

                StudentRepository repository = new StudentRepository(connectionString);
                repository.Add(student);

                context.Response.StatusCode = StatusCodes.Status201Created;
                return student;

            });

            app.MapPut("/students/{id}", async (HttpContext context) =>
            {
                var student = await context.Request.ReadFromJsonAsync<Student>();

                StudentRepository repository = new StudentRepository(connectionString);
                repository.Add(student);
                context.Response.StatusCode = StatusCodes.Status204NoContent;
            });

            app.MapDelete("/students/{id}", (int id, HttpContext context) =>
            {
                StudentRepository repository = new StudentRepository(connectionString);
                repository.Delete(id);

                context.Response.StatusCode = StatusCodes.Status202Accepted;
            });


            app.Run();
        }
    }
}