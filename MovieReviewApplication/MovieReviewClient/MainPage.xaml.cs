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
using MovieReviewClient.models;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MovieReviewClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // A string array of hard coded movie titles for use with the auto suggestion box
        private string[] suggestions = new string[] {"The Godfather", "Home Alone", "Die Hard", "Inglorious Bastards", "Jaws", "The Matrix", "Blade" };

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Home.IsSelected)
            {
                TitleTextBlock.Text = "Home";
                load_data();
            }
            else if (Add.IsSelected)
            {
                TitleTextBlock.Text = "Add A Movie Review";
                Frame.Navigate(typeof(AddReview));
            }
        }

        private void ListBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            movie_panel1.Visibility = Visibility.Visible;
            movie_panel2.Visibility = Visibility.Visible;
            movie_panel3.Visibility = Visibility.Visible;
            movie_panel4.Visibility = Visibility.Visible;

            search_results.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //When main page is loaded the home listbox item will be shown
            Home.IsSelected = true;
        }


        async void load_data()
        {
            //get a list of top rated movies from the api
            string url = "https://api.themoviedb.org/3/movie/popular?api_key=497239b3014ce173cabf9cdd23f6b120&language=en-US&page=1";
            HttpClient client = new HttpClient();

            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            /* Convert that string into an object graph so I can easily access elements of the json anywhere
            * DataContractJsonSerializer object used to serialize and deserialize json data*/
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            //Memory stream allows data to flow - Json is UTF8
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            //Get data out of the serializer
            var data = (RootObject)serializer.ReadObject(memoryStream);
            
            /* Show Four Of The Latest Movies */
            //Movie 1
            string image = String.Format("https://image.tmdb.org/t/p/w500{0}", data.results[0].poster_path);
            movie_image1.Source = new BitmapImage(new Uri(image, UriKind.Absolute));

            title1.Text = data.results[0].title;
            rating1.Text = ((double)data.results[0].vote_average).ToString() + "/10";
            overview1.Text = data.results[0].overview;

            //Movie 2
            string image2 = String.Format("https://image.tmdb.org/t/p/w500{0}", data.results[1].poster_path);
            movie_image2.Source = new BitmapImage(new Uri(image2, UriKind.Absolute));

            title2.Text = data.results[1].title;
            rating2.Text = ((double)data.results[1].vote_average).ToString() + "/10";
            overview2.Text = data.results[1].overview;

            //Movie 3
            string image3 = String.Format("https://image.tmdb.org/t/p/w500{0}", data.results[2].poster_path);
            movie_image3.Source = new BitmapImage(new Uri(image3, UriKind.Absolute));

            title3.Text = data.results[2].title;
            rating3.Text = ((double)data.results[2].vote_average).ToString() + "/10";
            overview3.Text = data.results[2].overview;

            //Movie 4
            string image4 = String.Format("https://image.tmdb.org/t/p/w500{0}", data.results[3].poster_path);
            movie_image4.Source = new BitmapImage(new Uri(image4, UriKind.Absolute));

            title4.Text = data.results[3].title;
            rating4.Text = ((double)data.results[3].vote_average).ToString() + "/10";
            overview4.Text = data.results[3].overview;

        }

        //Search Box Event Handler
        private void MyAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

            UserSearchQuery(args.QueryText);           
        }



        private async void UserSearchQuery(String q)
        {           
            string url = String.Format("https://api.themoviedb.org/3/search/movie?api_key=497239b3014ce173cabf9cdd23f6b120&language=en-US&query={0}", q + "&page=1&include_adult=false");
            Debug.WriteLine(url);
            //get a list of top rated movies from the api
            
            HttpClient client = new HttpClient();

            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            /* Convert that string into an object graph so I can easily access elements of the json anywhere
            * DataContractJsonSerializer object used to serialize and deserialize json data*/
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            //Memory stream allows data to flow - Json is UTF8
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            //Get data out of the serializer
            var data = (RootObject)serializer.ReadObject(memoryStream);

            // Display Search Results Results
            movie_panel1.Visibility = Visibility.Collapsed;
            movie_panel2.Visibility = Visibility.Collapsed;
            movie_panel3.Visibility = Visibility.Collapsed;
            movie_panel4.Visibility = Visibility.Collapsed;

            search_results.Visibility = Visibility.Visible;
   
            string search_img = String.Format("https://image.tmdb.org/t/p/w500{0}", data.results[3].poster_path);
            searched_image.Source = new BitmapImage(new Uri(search_img, UriKind.Absolute));
            searched_result_title.Text = data.results[0].title;
            searched_result_rating.Text = ((double)data.results[0].vote_average).ToString() + "/10";
            searched_result_overview.Text = data.results[0].overview; 
        }

        //Movie Search Suggestions
        private void MyAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

        }

        private void MyAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if(args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if(sender.Text.Length > 1)
                {
                    sender.ItemsSource = this.GetSuggestions(sender.Text);
                }
                else
                {
                    sender.ItemsSource = new string[] { "No Movie Suggestions" };
                }
            }
        }

        private string[] GetSuggestions(string text)
        {
            string[] result = null;
            result = suggestions.Where(x => x.Contains(text)).ToArray();
            return result;
        }
    }
    
}
