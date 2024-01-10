using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	public class AuditEntry
	{

		public int AuditEntryId { get; set; }

		public DateTime Date { get; set; }

		public required string OPeration { get; set; }

		public string? ExtendedDescription { get; set; }
		
		public required string UserName { get; set; }

		public required string IpAddress { get; set; }

		public int CountryId { get; set; }

		public required Country Country { get; set; }

		public string? City { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public string? ISP { get; set; }

		public string? UserAgent { get; set; }

		public string? OperatingSystem { get; set; }
	}
}
