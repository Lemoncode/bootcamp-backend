using System;
using System.Collections.Generic;

namespace Lemoncode.Books.Domain
{
    public class AuthorEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime Birth { get; set; }
        public string CountryCode { get; set; }
        public List<BookEntity> Books { get; set; }
    }
}
