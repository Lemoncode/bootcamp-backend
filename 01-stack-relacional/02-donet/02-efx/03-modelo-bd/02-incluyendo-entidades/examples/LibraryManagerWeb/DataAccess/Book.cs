using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	public class Book
	{

		public int BookId { get; set; }

		public int AuthorId { get; set; }

		public required Author Author { get; set; }

		public required string Title { get; set; }

		public string? Sinopsis { get; set; }

        public DateTime LoadedDate { get; set; }

		public DateTime CreationDateUtc { get; set; }

		public decimal AVerage { get; set; }

		public required BookImage BookImage { get; set; }
        public required Publisher Publisher { get; set; }

        public required List<Tag> Tags { get; set; }
	}
}
