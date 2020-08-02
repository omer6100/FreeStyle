using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public int UserId { get; set; }

        [DisplayName("Reviewer")]
        public string Username { get; set; }

        public int AlbumId { get; set; }

        [DisplayName("Album Title")]
        public string AlbumTitle { get; set; }

        [Required(ErrorMessage = "A Review cannot be Empty")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required(ErrorMessage = "Please Enter a Score")]
        [Range(0, 10, ErrorMessage = "Please Enter a Score from 1-10")]
        public int Score { get; set; }
    }
} 