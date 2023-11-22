using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	public class Book
	{

		public int BookId { get; set; }

		public int AuthorId { get; set; }

		public Author Author { get; set; }

		public string Title { get; set; }

		public string Sinopsis { get; set; }

	}
}
