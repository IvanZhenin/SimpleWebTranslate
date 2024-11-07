using Microsoft.EntityFrameworkCore;
using TranslateDataBase.Models;

namespace TranslateDataBase.Repositories
{
	public class TranslateBlockRepository
	{
		private readonly TranslateDbContext _context;

		public TranslateBlockRepository(TranslateDbContext context)
		{
			_context = context;
		}

		public async Task AddNewTranslateBlockAsync(string sourceText,
			string translateFromLanguage,
			string translateToLanguage,
			string resultText)
		{
			var translateBlock = new TranslateBlock()
			{
				SourceText = sourceText,
				TranslateFromLanguage = translateFromLanguage,
				TranslateToLanguage = translateToLanguage,
				ResultText = resultText,
			};

			await _context.TranslateBlocks.AddAsync(translateBlock);
			await _context.SaveChangesAsync();
		}
	}
}