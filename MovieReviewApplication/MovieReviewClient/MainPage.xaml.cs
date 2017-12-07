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
using Windows.UI.Core;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MovieReviewClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // A string array of hard coded movie titles for use with the auto suggestion box
        private string[] suggestions = new string[] {"The Godfather", "Home Alone", "Die Hard", "Inglorious Bastards", "Jaws", "The Matrix", "Blade",
        "American Beauty", "Gladiator", "A Beautiful Mind", "Chicago", "", "The Lord of the Rings", "Million Dollar Baby", "The Departed", "No Country for Old Men",
        "Pulp Fiction", "Goodfellas", "Slumdog Millionaire", "The King's Speech", "The Artist", "Argo", "The Hurt Locker", "Titanic", "The English Patient", "Forrest Gump", "Schindler's List", "The Silence of the Lambs", "Out of Africa", "Gran Torino",};

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
            else if (ShowReviews.IsSelected)
            {
                TitleTextBlock.Text = "User Reviews";
                Frame.Navigate(typeof(ViewReviews));
            }
        }

        private void ListBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            listMovies.Visibility = Visibility.Visible;
            listMovies.Visibility = Visibility.Visible;
            listMovies.Visibility = Visibility.Visible;
            listMovies.Visibility = Visibility.Visible;

            search_results.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //When main page is loaded the home listbox item will be shown
            Home.IsSelected = true;
            listMovies.Visibility = Visibility.Visible;
            search_results.Visibility = Visibility.Collapsed;

            main_title.Visibility = Visibility.Visible;
            search_title.Visibility = Visibility.Collapsed;

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

            var actualresult = data.results;
            
            foreach (var r in actualresult)
            {
                r.poster_path = String.Format("https://image.tmdb.org/t/p/w500{0}", r.poster_path);
            }
   
            listMovies.ItemsSource = actualresult; 
        }

        //Search Box Event Handler
        private void MyAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            UserSearchQuery(args.QueryText);           
        }

        private async void UserSearchQuery(String q)
        {           
            string url = String.Format("https://api.themoviedb.org/3/search/movie?api_key=497239b3014ce173cabf9cdd23f6b120&language=en-US&query={0}", q + "&page=1&include_adult=false");
            
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

            var actualresult = data.results;
            
            foreach (var r in actualresult)
            {             
                r.poster_path = String.Format("https://image.tmdb.org/t/p/w500{0}", r.poster_path);
            }
            
            search_results.ItemsSource = actualresult;

            // Display Search Results Results
            listMovies.Visibility = Visibility.Collapsed;
            listMovies.Visibility = Visibility.Collapsed;
            listMovies.Visibility = Visibility.Collapsed;
            listMovies.Visibility = Visibility.Collapsed;

            main_title.Visibility = Visibility.Collapsed;
            search_title.Visibility = Visibility.Visible;

            search_results.Visibility = Visibility.Visible;
   
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
