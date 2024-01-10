using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	public class Country
	{

		public int CountryId { get; set; }

		public required string NativeName { get; set; }

		public required string EnglishName { get; set; }
	}
}
