using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lemoncode.Azure.Models;

namespace Lemoncode.Azure.Api.Data
{
    public class ApiDBContext : DbContext
    {
        public ApiDBContext (DbContextOptions<ApiDBContext> options)
            : base(options)
        {
        }
        public DbSet<Game> Game { get; set; }
    }
}
