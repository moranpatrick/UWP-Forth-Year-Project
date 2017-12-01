using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieReviewWebApp.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string movie_title { get; set; }
        public string movie_review { get; set; }
    }
}