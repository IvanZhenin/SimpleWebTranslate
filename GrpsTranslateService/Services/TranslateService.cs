using Grpc.Core;
using GrpcServiceTest;
using GrpsTranslateService;
using TranslateDataBase.Repositories;
using TranslationLibrary;

namespace GrpsTranslateService.Services
{
	public class TranslateService : Translate.TranslateBase
	{
		private readonly ILogger<TranslateService> _logger;
		private readonly ITranslateService _translateService;
		private readonly TranslateBlockRepository _repository;
		public TranslateService(ILogger<TranslateService> logger, 
			ITranslateService translateService,
            TranslateBlockRepository repository)
		{
			_logger = logger;
			_translateService = translateService;
			_repository = repository;
		}

		public override async Task<TranslateReply> GetTranslate(TranslateRequest request, ServerCallContext context)
		{
			_logger.LogInformation($"Создание перевода с языка '{request.LanguageFrom}' на '{request.LanguageTo}' язык");

			try
			{
				var translatedText = await _translateService.TranslateTextAsync(
					request.SourceText,
					request.LanguageFrom,
					request.LanguageTo);

				var addNewBlock = await _repository.AddNewTranslateBlockAsync(
					request.SourceText,
					request.LanguageFrom,
					request.LanguageTo,
					translatedText);

				_logger.LogInformation($"Успешный перевод текста с языка '{request.LanguageFrom}' на '{request.LanguageTo}' язык");
				_logger.LogInformation(addNewBlock);
				
				return new TranslateReply { ResultText = translatedText };
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, $"HttpRequestException: Сервис перевода недоступен для заявки с языка '{request.LanguageFrom}' на '{request.LanguageTo}' язык");

				throw new RpcException(new Status(StatusCode.Unavailable, "Сервис перевода недоступен"), ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Произошла внутренняя ошибка сервера при попытке перевода с языка '{request.LanguageFrom}' на '{request.LanguageTo}' язык");

				throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка сервера"), ex.Message);
			}
		}
	}
}
