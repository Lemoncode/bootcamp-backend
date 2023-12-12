using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	public class LibraryContext : DbContext
	{

		public DbSet<Author> Authors { get; set; }

		public DbSet<Book> Books { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public LibraryContext(DbContextOptions<LibraryContext> options)
			: base(options)
		{
		}
	}
}
