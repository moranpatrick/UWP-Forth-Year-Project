using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieReviewApp
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

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            var review = new Reviews()
            {
                name = nameTb.Text,
                movie_title = movieTitleTb.Text,
                movie_review = movieReviewTb.Text
            };
            var reviewJson = JsonConvert.SerializeObject(review);

            var client = new HttpClient();
            var HttpContent = new StringContent(reviewJson);
            HttpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            await client.PostAsync("http://moviereviewwebapp.azurewebsites.net/api/Reviews", HttpContent);

            Frame.GoBack();
        }
    }
}
