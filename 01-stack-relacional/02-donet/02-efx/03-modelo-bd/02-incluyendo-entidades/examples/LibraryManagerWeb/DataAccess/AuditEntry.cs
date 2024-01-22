using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.DataAccess
{
	/// <summary>
	///  Clase para incluir entradas de auditoría con respecto a las operaciones realizadas en nuestra biblioteca.
	/// </summary>
	[Comment("Clase para incluir entradas de auditoría con respecto a las operaciones realizadas en nuestra biblioteca.")]
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
