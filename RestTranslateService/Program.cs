using TranslationLibrary;
using Microsoft.EntityFrameworkCore;
using TranslateDataBase.Repositories;
using TranslateDataBase;

namespace RestTranslateService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<TranslateDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnect")));
            builder.Services.AddScoped<ITranslateService, GoogleTranslateService>();
            builder.Services.AddScoped<TranslateBlockRepository>();
            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

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
