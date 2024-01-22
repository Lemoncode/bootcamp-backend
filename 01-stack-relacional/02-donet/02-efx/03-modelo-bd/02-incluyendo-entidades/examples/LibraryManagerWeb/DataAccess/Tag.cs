namespace LibraryManagerWeb.DataAccess
{
    public class Tag
    {

        public required string Name { get; set; }

        public required List<Book> Books { get; set; }
    }
}
