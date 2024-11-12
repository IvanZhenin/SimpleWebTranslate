namespace RestTranslateService.Model
{
    public class TranslateRequest
    {
        public string SourceText { get; set; } = "";
        public string LanguageFrom { get; set; } = "";
        public string LanguageTo { get; set; } = "";
    }
}