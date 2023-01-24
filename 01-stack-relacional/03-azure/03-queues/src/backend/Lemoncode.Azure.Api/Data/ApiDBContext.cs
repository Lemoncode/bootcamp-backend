using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lemoncode.Azure.Api.Models;

namespace Lemoncode.Azure.Api.Data
{
    public class ApiDBContext : DbContext
    {
        public ApiDBContext (DbContextOptions<ApiDBContext> options)
            : base(options)
        {
        }
        public DbSet<Lemoncode.Azure.Api.Models.Game> Game { get; set; }
    }
}
