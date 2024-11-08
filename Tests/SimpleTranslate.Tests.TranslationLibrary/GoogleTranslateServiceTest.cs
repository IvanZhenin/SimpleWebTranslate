using TranslationLibrary;

namespace SimpleTranslate.Tests.TranslationLibrary
{
	public class GoogleTranslateServiceTest
	{
		private ITranslateService googleService = new GoogleTranslateService();

		[Fact]
		public void Translate_HelloWorld_With_EN_to_RU()
		{
			string expected = googleService.TranslateTextAsync("Hello world!", "EN", "RU").Result;
			string actual = "Привет, мир!";

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void Translate_HelloWorld_and_WhereIsYou_With_EN_to_RU()
		{
			string expected = googleService.TranslateTextAsync("Hello world! Where is you?", "EN", "RU").Result;
			string actual = "Привет, мир! Где ты?";

			Assert.Equal(expected, actual);
		}


		[Fact]
		public void Translate_Hello_comma_World_With_EN_to_RU()
		{
			string expected = googleService.TranslateTextAsync("Hello, world!", "EN", "RU").Result;
			string actual = "Привет, мир!";

			Assert.Equal(expected, actual);
		}
	}
}