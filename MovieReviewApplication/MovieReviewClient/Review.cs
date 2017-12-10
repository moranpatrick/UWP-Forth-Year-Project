using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewClient
{
    class Review
    {
        public int id { get; set; }
        public string name { get; set; }
        public string movie_title { get; set; }
        public int rating { get; set; }
        public string movie_review { get; set; }
    }

    public class ListReviews
    {
        public string id { get; set; }
        public string author { get; set; }
        public string content { get; set; }
        public string url { get; set; }
    }

    public class ReviewRoot
    {
        public int id { get; set; }
        public int page { get; set; }
        public List<ListReviews> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }


}
