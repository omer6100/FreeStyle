using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Freestyle.Models;


namespace Freestyle.Contexts
{
    public class AlbumContext : DbContext
    {
        public AlbumContext() : base("AlbumContext"){}

        public DbSet<Album> Albums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}