using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieReviewWebApp.Models
{
    public class Review
    {
        public int id { get; set; }
        public string name { get; set; }
        public string movie_title { get; set; }
        public int rating { get; set; }
        public string movie_review { get; set; }
    }
}