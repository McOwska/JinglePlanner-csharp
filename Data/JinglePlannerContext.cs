using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JinglePlanner.Models;

namespace JinglePlanner.Data
{
    public class JinglePlannerContext : DbContext
    {
        public JinglePlannerContext (DbContextOptions<JinglePlannerContext> options)
            : base(options)
        {
        }

        public DbSet<JinglePlanner.Models.Recipe> Recipe { get; set; } = default!;

        public DbSet<JinglePlanner.Models.User> User { get; set; } = default!;

        public DbSet<JinglePlanner.Models.Guest> Guest { get; set; } = default!;

        public DbSet<JinglePlanner.Models.Party> Party { get; set; } = default!;

        public DbSet<JinglePlanner.Models.Dish> Dish { get; set; } = default!;

   
    }
}
