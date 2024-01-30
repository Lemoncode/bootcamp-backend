using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISS.Entities
{
    internal class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }

        public bool IsAdmin { get; set; }

    }
}
