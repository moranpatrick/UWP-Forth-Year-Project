using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System.Net.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieReviewClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditReviewxaml : Page
    {
        private Review review = new Review();

        public EditReviewxaml()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var review = e.Parameter as Review;

            name.Text = review.name;
            movie_title.Text = review.movie_title;
            movie_rating.Text = review.rating.ToString();
            movie_review.Text = review.movie_review;
        }

        //Delete
        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            await client.DeleteAsync("http://moviereviewwebapp20171206123555.azurewebsites.net/api/Reviews/" + review.id);
            Frame.GoBack();
        }

        // Edit
        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            review.name = name.Text;
            review.movie_title = movie_title.Text;
            review.rating = int.Parse(movie_rating.Text);
            review.movie_review = movie_review.Text;

            var reviewJson = JsonConvert.SerializeObject(review);

            var HttpContent = new StringContent(reviewJson);
            HttpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            await client.PutAsync("http://moviereviewwebapp20171206123555.azurewebsites.net/api/Reviews/" + review.id, HttpContent);

            Frame.GoBack();
        }

    }
}
