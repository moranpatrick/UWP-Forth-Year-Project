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
using System.Diagnostics;
using MovieReviewClient.models;
using System.Runtime.Serialization.Json;
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieReviewClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewReviews : Page
    {
        public ViewReviews()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            TitleTextBlock.Text = "View Reviews";
            progressRing.IsActive = true;
            HttpClient client = new HttpClient();

            var review = e.Parameter as Result;
    
            if (review != null)
            {
                Debug.WriteLine(review.id);
                string url = String.Format("https://api.themoviedb.org/3/movie/{0}/reviews?api_key=497239b3014ce173cabf9cdd23f6b120&language=en-US&page=1", review.id);
                Debug.WriteLine(url);
                try
                {
                    Debug.WriteLine("NOT NULL");
                    var response = await client.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();

                    ReviewRoot data = JsonConvert.DeserializeObject<ReviewRoot>(result);
                    if (data.total_results == 0)
                    {
                        error.Visibility = Visibility.Visible;
                        error.Text = "Sorry No Reviews Posted Yet!";
                    }
                    else
                    {
                        var actualResult = data.results;

                        apiReviewList.ItemsSource = actualResult;

                        review_title.Visibility = Visibility.Visible;
                        main_title.Visibility = Visibility.Collapsed;
                        reviewsList.Visibility = Visibility.Collapsed;
                        apiReviewList.Visibility = Visibility.Visible;
                    }
                    progressRing.IsActive = false;

                }
                catch
                {
                    progressRing.IsActive = false;
                }
            }
            else
            {
                Debug.WriteLine("IS NULL");
                try
                {
                    var JsonResponse = await client.GetStringAsync("http://moviereviewwebapp20171206123555.azurewebsites.net/api/Reviews");
                    var reviewResult = JsonConvert.DeserializeObject<List<Review>>(JsonResponse);
                    reviewResult.Reverse();
                    reviewsList.ItemsSource = reviewResult;
                    progressRing.IsActive = false;
                }
                catch
                {
                    progressRing.IsActive = false;
                    error.Visibility = Visibility.Visible;
                    error.Text = "Error Retrieving Reviews";
                }
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Home.IsSelected)
            {
                Frame.Navigate(typeof(MainPage));
                TitleTextBlock.Text = "Home";
            }
            else if (Add.IsSelected)
            {
                Frame.Navigate(typeof(AddReview));
                TitleTextBlock.Text = "Create A Review";
            }
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
