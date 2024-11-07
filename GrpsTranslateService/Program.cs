using GrpsTranslateService.Services;
using Microsoft.EntityFrameworkCore;
using TranslateDataBase.Repositories;
using TranslateDataBase;

namespace GrpsTranslateService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<TranslateDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnect")));
			builder.Services.AddScoped<TranslateBlockRepository>();
			builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

			// Add services to the container.
			builder.Services.AddGrpc();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			app.MapGrpcService<TranslateService>();
			app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

			app.Run();
		}
	}
}