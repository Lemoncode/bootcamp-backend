using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	public class PhisicalLibrary
	{

		public int PhisicalLibraryId { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }

		public string City { get; set; }

		public int CountryId { get; set; }

		public Country Country { get; set; }

	}
}
