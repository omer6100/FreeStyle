﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Freestyle.Models
{
    public class Artist
    {
        public int Id { get; set; }

        [DisplayName("Artist's Name")]
        [Required(ErrorMessage = "Artist's Name is Required")]
        public string Name { get; set; }

        [DisplayName("Origin Country")]
        public string OriginCountry { get; set; }

        [DisplayName("Average Score")]
        public double AvgScore { get; set; }

        [DisplayName("Page Views")]
        public int PageViews { get; set; }

    }

}