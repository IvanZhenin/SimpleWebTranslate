using System.Text.Json;
using System.Text.RegularExpressions;
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

					using (var json = JsonDocument.Parse(result))
					{
						var translations = json.RootElement[0];
						var translatedText = string.Join(" ", translations.EnumerateArray().Select(t => t[0].GetString()));

						translatedText = Regex.Replace(translatedText, @"(?<=[.!?])\s+", " ");

						return translatedText;
					}
				}
				catch
				{
					return "*Ошибка*";
				}
			}
		}
	}
}