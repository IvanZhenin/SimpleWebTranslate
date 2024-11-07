using Grpc.Core;
using GrpcServiceTest;
using GrpsTranslateService;
using TranslationLibrary;

namespace GrpsTranslateService.Services
{
	public class TranslateService : Translate.TranslateBase
	{
		private readonly ILogger<TranslateService> _logger;
		private readonly ITranslateService _translateService;
		public TranslateService(ILogger<TranslateService> logger)
		{
			_logger = logger;
			_translateService = new GoogleTranslateService();
		}

		public override async Task<TranslateReply> GetTranslate(TranslateRequest request, ServerCallContext context)
		{
			_logger.LogInformation("Создание перевода с языка '{LanguageFrom}' на '{LanguageTo}' язык",
				request.LanguageFrom, request.LanguageTo);

			try
			{
				var translatedText = await _translateService.TranslateTextAsync(
					request.SourceText,
					request.LanguageFrom,
					request.LanguageTo);

				_logger.LogInformation("Успешный перевод с языка '{LanguageFrom}' на '{LanguageTo}' язык",
					request.LanguageFrom, request.LanguageTo);

				return new TranslateReply { ResultText = translatedText };
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "HttpRequestException: Сервис перевода недоступен для заявки с языка '{LanguageFrom}' на '{LanguageTo}' язык",
					request.LanguageFrom, request.LanguageTo);

				throw new RpcException(new Status(StatusCode.Unavailable, "Сервис перевода недоступен"), ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Произошла внутренняя ошибка сервера при попытке перевода с языка '{LanguageFrom}' на '{LanguageTo}' язык");

				throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка сервера"), ex.Message);
			}
		}
	}
}
