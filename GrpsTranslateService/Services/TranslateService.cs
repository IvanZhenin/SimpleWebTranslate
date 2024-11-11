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
			_logger.LogInformation($"�������� �������� � ����� '{request.LanguageFrom}' �� '{request.LanguageTo}' ����");

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

				_logger.LogInformation($"�������� ������� ������ � ����� '{request.LanguageFrom}' �� '{request.LanguageTo}' ����");
				_logger.LogInformation(addNewBlock);
				
				return new TranslateReply { ResultText = translatedText };
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, $"HttpRequestException: ������ �������� ���������� ��� ������ � ����� '{request.LanguageFrom}' �� '{request.LanguageTo}' ����");

				throw new RpcException(new Status(StatusCode.Unavailable, "������ �������� ����������"), ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"��������� ���������� ������ ������� ��� ������� �������� � ����� '{request.LanguageFrom}' �� '{request.LanguageTo}' ����");

				throw new RpcException(new Status(StatusCode.Internal, "��������� ���������� ������ �������"), ex.Message);
			}
		}
	}
}
