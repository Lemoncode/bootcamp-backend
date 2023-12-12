using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	public class PhisicalLibrary
	{

		public int PhisicalLibraryId { get; set; }

		public required string Name { get; set; }

		public required string Address { get; set; }

		public required string City { get; set; }

		public int CountryId { get; set; }

		public required Country Country { get; set; }

	}
}
