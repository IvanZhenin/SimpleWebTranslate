using Microsoft.EntityFrameworkCore;
using TranslateDataBase.Repositories;
using TranslateDataBase;
using Grpc.Net.Client;
using GrpcServiceTest;

namespace TranslateWebClient
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient("RestTranslateService", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7194/");
            });

            builder.Services.AddSingleton(services =>
            {
                var grpcChannel = GrpcChannel.ForAddress("https://localhost:7028");
                return new Translate.TranslateClient(grpcChannel);
            });

            // Add services to the container.
            builder.Services.AddRazorPages();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
	}
}