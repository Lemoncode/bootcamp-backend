using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemoncode.Books.Application.Models
{
    public class AuthorInfo
    {
        public int Id { get; set; }
        public string FullName { get; init; } = string.Empty;
        public DateTime Birth { get; init; }
        public string CountryCode { get; init; } = string.Empty;
        public IEnumerable<BookSummaryInfo> Books { get; init; } = Enumerable.Empty<BookSummaryInfo>();
    }
}
