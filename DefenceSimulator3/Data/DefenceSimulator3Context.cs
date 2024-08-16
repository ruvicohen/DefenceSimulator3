using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DefenceSimulator3.Models;
using static DefenceSimulator3.Models.Enums;

namespace DefenceSimulator3.Data
{
    public class DefenceSimulator3Context : DbContext
    {
        public DefenceSimulator3Context (DbContextOptions<DefenceSimulator3Context> options)
            : base(options)
        {
            DatabaseSeeder.Seed(this);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



        }







        public DbSet<DefenceSimulator3.Models.Origin> Origin { get; set; } = default!;
        public DbSet<DefenceSimulator3.Models.Threat> Threat { get; set; } = default!;
        public DbSet<DefenceSimulator3.Models.Weapon> Weapon { get; set; } = default!;
        public DbSet<DefenceSimulator3.Models.WeaponDefence> WeaponDefence { get; set; } = default!;
    }
}
