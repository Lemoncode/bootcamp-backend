using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	public class Author
	{

		public int AuthorId { get; set; }

		[MaxLength(200)]
		public required string Name { get; set; }

		public required string LastName { get; set; }

		public string DisplayName { get; set; } = default!;

		[NotMapped]
		public DateTime LoadedDate { get; set; }

		public List<Book> Books { get; set; } = new();

	}
}
