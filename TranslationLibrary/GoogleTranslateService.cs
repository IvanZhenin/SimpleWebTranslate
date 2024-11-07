using System.Web;

namespace TranslationLibrary
{
	public class GoogleTranslateService : ITranslateService
	{
		public async Task<string> TranslateTextAsync(string text, string fromLanguage, string toLanguage)
		{
			if (String.IsNullOrEmpty(text))
			{
				return string.Empty;
			}

			var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(text)}";

			using (var httpClient = new HttpClient())
			{
				try
				{
					var result = await httpClient.GetStringAsync(url);
					result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
					return result;
				}
				catch
				{
					return "*Ошибка*";
				}
			}
		}
	}
}