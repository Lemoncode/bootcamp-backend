using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	[Table("Countries")]
	public class Country
	{

		public int CountryId { get; set; }

		public required string NativeName { get; set; }

		public required string EnglishName { get; set; }
	}
}
