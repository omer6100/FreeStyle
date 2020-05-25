using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Freestyle.Models
{
    public class Review
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public Album Album { get; set; }
        public string Text { get; set; }
        public double Score { get; set; }
    }
} 