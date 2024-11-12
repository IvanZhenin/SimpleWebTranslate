using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using TranslateDataBase.Repositories;

namespace TranslateWebClient.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Translate.TranslateClient _grpcClient;
        public IndexModel(ILogger<IndexModel> logger,
            IHttpClientFactory httpClientFactory,
            Translate.TranslateClient grpcClient)
		{
			_logger = logger;
            _httpClientFactory = httpClientFactory;
            _grpcClient = grpcClient;
        }

        [BindProperty]
        [Required(ErrorMessage = "����������, ������� ������ ��������.")]
        public string ChoosedService { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "����������, ������� ����� ��� ��������.")]
		public string SourceText { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "����������, �������� �������� ����.")]
		public string LanguageFrom { get; set; }

		[BindProperty]
		[Required(ErrorMessage = "����������, �������� ���� ��������.")]
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
				if (ChoosedService == "gRPC")
				{
					var request = new TranslateRequest
					{
						SourceText = SourceText,
						LanguageFrom = LanguageFrom,
						LanguageTo = LanguageTo
					};

					var reply = await _grpcClient.GetTranslateAsync(request);
					TranslatedText = reply.ResultText;
				}
				else if (ChoosedService == "REST")
				{
					var restRequest = new
					{
						SourceText = SourceText,
						LanguageFrom = LanguageFrom,
						LanguageTo = LanguageTo
					};

					var client = _httpClientFactory.CreateClient("RestTranslateService");
					var response = await client.PostAsJsonAsync("api/translate/translate", restRequest);

					if (response.IsSuccessStatusCode)
					{
						var result = await response.Content.ReadFromJsonAsync<TranslateResponse>();
						TranslatedText = result?.ResultText;
					}
					else
					{
						throw new Exception("������ ��� ������ REST API");
					}
				}
			}
			catch (RpcException ex)
			{
				_logger.LogError(ex, "������ �������� ����������");
				TranslatedText = "*������ �������� ����������*";
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "��������� �������������� ������ �� ����� ��������");
				TranslatedText = "*��������� �������������� ������ �� ����� ��������*";
			}

			return Page();
		}
	}

    public class TranslateResponse
    {
        public string ResultText { get; set; }
    }
}