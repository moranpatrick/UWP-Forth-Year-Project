﻿using System;
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
    public sealed partial class ViewReviews : Page
    {
        public ViewReviews()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            HttpClient client = new HttpClient();
            var JsonResponse = await client.GetStringAsync("http://moviereviewwebapp20171206123555.azurewebsites.net/api/Reviews");
            //var JsonResponse = await client.GetStringAsync("http://localhost:52985/api/Reviews"); 

            var reviewResult = JsonConvert.DeserializeObject<List<Review>>(JsonResponse);

            reviewsList.ItemsSource = reviewResult;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void reviewsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var review = reviewsList.SelectedItem as Review;
            Frame.Navigate(typeof(EditReviewxaml), review);
        }
    }
}