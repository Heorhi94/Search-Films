using IMDbApiLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var api = new ApiLib("k_y76947gh");
            var result = await api.SearchTitleAsync(SearchTextBox.Text);

            ResultsListBox.ItemsSource = result.Results;
            ResultsListBox.DisplayMemberPath = "Title";
            ResultsListBox.SelectedValuePath = "Id";

        }

        private async Task<List<string>> SearchMovies(string searchTerm)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"http://www.omdbapi.com/?s={searchTerm}&apikey=yourapikey");
            var results = new List<string>();
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(responseString);
                var searchResults = data["Search"];
                foreach (var result in searchResults)
                {
                    var title = result["Title"].ToString();
                    results.Add(title);
                }
            }
            return results;
        }

        private async void ResultsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var api = new ApiLib("k_y76947gh");
            string id = ResultsListBox.SelectedValue.ToString();
            var titleData = await api.TitleAsync(id);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(titleData.Image);
            bitmap.EndInit();
            image.Source = bitmap;
            titleLabel.Content = titleData.FullTitle;
        }
    }
}
