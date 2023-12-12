using LibraryManagerWeb.DataAccess;
using LibraryManagerWeb.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagerWeb.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly LibraryContext _context;

		public HomeController(ILogger<HomeController> logger, LibraryContext context)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			var books = await _context.Books.OrderBy(b => b.Title).ThenBy(b => b.Author.Name).ThenBy(b => b.Author.LastName).ToListAsync();
			foreach (var b in books)
			{
				b.Title += " (modified)";
			}
			await _context.SaveChangesAsync();

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
