using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Freestyle.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Artist Artist { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}