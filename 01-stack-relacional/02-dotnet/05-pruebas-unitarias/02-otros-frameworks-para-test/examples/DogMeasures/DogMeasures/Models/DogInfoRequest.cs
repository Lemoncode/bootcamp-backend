using System.ComponentModel.DataAnnotations;

namespace DogMeasures.Models
{
	public class DogInfoRequest
	{

		[Required(ErrorMessage = "Debes indicar la raza de tu perro.")]
		[Display(Name = "Raza del perro:")]
		public string Breed { get; set; }

		[Required(ErrorMessage = "Debes indicar el peso actual de tu perro.")]
		[Display(Name = "Peso de tu perro:")]
		[Range(minimum: 0.1, maximum: 100, ErrorMessage = "El peso debe ser mayor que 0 y menor que 100.")]
		public decimal Weight { get; set; }
	}
}
