using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Freestyle.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Album title is Required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Artist's Name is Required")]
        public string Artist { get; set; }

        public int ArtistId { get; set; }

        [Required(ErrorMessage = "Release Date is Required")]
        [DataType(DataType.DateTime, ErrorMessage = "Please Enter a Valid Date")]
        public DateTime ReleaseDate { get; set; }

        public double AvgScore { get; set; }
        public int PageViews { get; set; }
    }
}