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
    public sealed partial class AddReview : Page
    {
        public AddReview()
        {
            this.InitializeComponent();
        }


        //view all button
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ViewReviews));
        }

        //Cancel
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        // Add Review
        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            var review = new Review()
            {
                name = name.Text,
                movie_title = movie_title.Text,
                rating = int.Parse(movie_rating.Text),
                movie_review = movie_review.Text
            };
            var reviewJson = JsonConvert.SerializeObject(review);

            var client = new HttpClient();
            var HttpContent = new StringContent(reviewJson);
            HttpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            await client.PostAsync("http://moviereviewwebapp20171206123555.azurewebsites.net/api/Reviews", HttpContent);

            Frame.Navigate(typeof(ViewReviews));
        }
    }
}
