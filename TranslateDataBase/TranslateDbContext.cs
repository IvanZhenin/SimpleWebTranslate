using Microsoft.EntityFrameworkCore;
using TranslateDataBase.Models;

namespace TranslateDataBase
{
	public class TranslateDbContext : DbContext
	{
		public TranslateDbContext(DbContextOptions<TranslateDbContext> options)
			: base(options)
		{
		}

		public DbSet<TranslateBlock> TranslateBlocks { get; set; }
	}
}