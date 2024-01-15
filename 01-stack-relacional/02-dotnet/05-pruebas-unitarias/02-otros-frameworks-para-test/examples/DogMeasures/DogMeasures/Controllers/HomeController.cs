using DogMeasures.Models;
using DogMeasures.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DogMeasures.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] DogInfoRequest info)
        {
            if (!ModelState.IsValid)
            {
                return View(info);
            }
            try
            {
                var measures = new DogMeasuresService().CheckDogIdealWeight(info.Breed, info.Weight);
                return await Task.FromResult(View("MeasuresResults", measures));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al obtener la información sobre tu perro: {ex.Message}.");
                return View(info);
            }
        }
    }
}