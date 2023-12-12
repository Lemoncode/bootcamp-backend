using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	public class BookFile
	{

		public int BookFileId { get; set; }

		public int BookId { get; set; }

		public required Book Book { get; set; }

		public required string InternalFilePath { get; set; }

		public required BookFormat Format { get; set; }
	}
}
