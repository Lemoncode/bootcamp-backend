namespace LibraryManagerWeb.DataAccess
{
    public class BookImage
    {

        public int BookImageId { get; set; }

        public required string AlternateText { get; set; }

        public int BookId { get; set; }

        public required Book Book { get; set; }
    }
}
