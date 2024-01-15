namespace DogMeasures.Models
{
	public class DogFood
	{
		public enum Size
		{
			Small,
			Medium,
			Big,
			Huge
		}

		public string Breed { get; set; }


		public int FoodWeight { get; set; }

		public int TimesPerDay { get; set; }

		public Size BallSize { get; set; }

	}
}