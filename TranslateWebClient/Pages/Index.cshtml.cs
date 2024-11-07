using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TranslateDataBase.Repositories;

namespace TranslateWebClient.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly TranslateBlockRepository _repository;

		public IndexModel(ILogger<IndexModel> logger, TranslateBlockRepository repository)
		{
			_logger = logger;
			_repository = repository;
		}

		[BindProperty]
		[Required(ErrorMessage = "Пожалуйста, введите текст для перевода.")]
		public string SourceText { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Пожалуйста, выберите исходный язык.")]
		public string LanguageFrom { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "Пожалуйста, выберите язык перевода.")]
		public string LanguageTo { get; set; }

		public string TranslatedText { get; set; }

		public void OnGet()
		{

		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				var channel = GrpcChannel.ForAddress("https://localhost:7028");
				var client = new Translate.TranslateClient(channel);

				var request = new TranslateRequest
				{
					SourceText = SourceText,
					LanguageFrom = LanguageFrom,
					LanguageTo = LanguageTo
				};
				var reply = await client.GetTranslateAsync(request);

				TranslatedText = reply.ResultText;
			}
			catch (RpcException ex)
			{
				_logger.LogError(ex, "Translation service is unavailable");
				TranslatedText = "Translation service is unavailable";
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An unexpected error occurred during translation");
				TranslatedText = "An unexpected error occurred";
			}

			return Page();
		}
	}
}
