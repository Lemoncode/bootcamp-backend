using System;
using System.Text.Json.Serialization;

namespace Lemoncode.Books.Application.Models
{
    public class NewAuthor
    {
        [JsonIgnore]
        public int? Id { get; set; } = default;
        public string Name { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public DateTime Birth { get; init; }
        public string CountryCode { get; init; } = string.Empty;
    }
}
