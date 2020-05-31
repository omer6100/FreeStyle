using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Freestyle.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public int UserId { get; set; }

        public string AlbumTitle { get; set; }

        public int AlbumId { get; set; }
        public string Text { get; set; }
        public double Score { get; set; }
    }
} 