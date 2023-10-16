using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerWeb.Data
{
	public class Author
	{

		public int AuthorId { get; set; }

		public string Name { get; set; }

		public string LastName { get; set; }

		public List<Book> Books { get; set; } = new List<Book>();

	}
}
