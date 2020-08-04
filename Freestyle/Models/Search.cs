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
        public double score { get; set; }
        public string genreCounry { get; set; } 


    }
}