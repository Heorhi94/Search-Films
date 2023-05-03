using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

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



        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text)) return;
            ResultsListBox.Items.Clear();
            SearchButton.IsEnabled = false;
            var worker = new BackgroundWorker();
            worker.DoWork += (s, args) =>
            {
                var searchTerm = args.Argument as string;
                var results = SearchMovies(searchTerm);
                args.Result = results;
            };
            worker.RunWorkerCompleted += (s, args) =>
            {
                var results = args.Result as List<string>;
                if (results != null)
                {
                    foreach (var result in results)
                    {
                        ResultsListBox.Items.Add(result);
                    }
                }
                SearchButton.IsEnabled = true;
              /*  foreach (var result in results)
                {
                    ResultsListBox.Items.Add(result);
                }
                SearchButton.IsEnabled = true;*/
            };
            worker.RunWorkerAsync(SearchTextBox.Text);
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

    }
}
