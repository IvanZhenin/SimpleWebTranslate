namespace TranslationLibrary
{
	public interface ITranslateService
	{
		public Task<string> TranslateTextAsync(string text, string fromLanguage, string toLanguage);
	}
}