using Microsoft.EntityFrameworkCore;

namespace TranslateDataBase.Models
{
	[PrimaryKey(nameof(Id))]
	public class TranslateBlock
	{
		public TranslateBlock()
		{
			Id = Guid.NewGuid();
			TranslateDate = DateTime.Now;
		}

		public Guid Id { get; set; }
		public DateTime TranslateDate { get; set; }
		public string SourceText { get; set; } = string.Empty;
		public string TranslateFromLanguage { get; set; } = string.Empty;
		public string TranslateToLanguage { get; set; } = string.Empty;
		public string ResultText { get; set; } = string.Empty;
	}
}