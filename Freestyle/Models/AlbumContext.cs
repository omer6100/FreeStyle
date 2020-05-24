using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace Freestyle.Models
{
    public class AlbumContext : DbContext
    {
        //public AlbumContext(DbContextOptions<AlbumContext> options) : base(options){}
        public DbSet<Album> Albums { get; set; }

    }
}