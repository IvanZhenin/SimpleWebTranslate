using Microsoft.AspNetCore.Mvc;
using RestTranslateService.Model;
using TranslateDataBase.Repositories;
using TranslationLibrary;

namespace RestTranslateService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslateController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ITranslateService _translateService;
        private readonly TranslateBlockRepository _repository;

        public TranslateController(ILogger<TranslateController> logger,
            ITranslateService translateService,
            TranslateBlockRepository repository)
        {
            _logger = logger;
            _translateService = translateService;
            _repository = repository;
        }

        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

                return Ok(new { ResultText = translatedText });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"StatusCode 500: Сервис перевода недоступен для заявки с языка '{request.LanguageFrom}' на '{request.LanguageTo}' язык");
                return StatusCode(500, "Ошибка при обработке перевода");
            }
        }
    }
}