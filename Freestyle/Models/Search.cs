using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Freestyle.Models
{
    public class Search
    {
       
        [Key]
        public string type { get; set; }
        public string primaryName { get; set; }
        public string secondaryName { get; set; } // for album (the artist); for review (username); 

        [Range(0, 10)]
        public double ScoreLowerBound { get; set; }


        [Range(0,10)]
        public double ScoreUpperBound { get; set; }

        public string GenreCountry { get; set; } 


    }
}