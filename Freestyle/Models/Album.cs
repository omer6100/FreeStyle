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
        public string Artist { get; set; }
        public int ArtistId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double AvgScore { get; set; }
    }
}