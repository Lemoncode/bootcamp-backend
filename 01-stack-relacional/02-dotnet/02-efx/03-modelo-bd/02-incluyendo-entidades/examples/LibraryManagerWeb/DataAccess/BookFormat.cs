using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	public class BookFormat
	{

		public int BookformatId { get; set; }

		public required string Name { get; set; }
	}
}
