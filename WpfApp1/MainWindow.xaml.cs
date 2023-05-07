using IMDbApiLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
  
    public partial class MainWindow : Window
    {
        //k_hye43l7u
        //k_y76947gh
        //k_eknpnu11
        //k_lv26p9yv
        private readonly ApiLib _api = new ApiLib("k_eknpnu11");
        private Task _currentTask;
        private CancellationTokenSource _cancellationTokenSource;
       

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                _currentTask?.Wait();

                resultsListBox.ItemsSource = null;
                titleLabel.Content = "";
                image.Source = null;
                progressBar.Value = 0;

                var result = await _api.SearchMovieAsync(searchTextBox.Text);
                var movies = result.Results.Select(r => new Movie { Id = r.Id, Title = r.Title }).ToList();
                resultsListBox.ItemsSource = movies;
                resultsListBox.DisplayMemberPath = "Title";
                resultsListBox.SelectedValuePath = "Id";
                progressBar.Maximum = movies.Count;

                _cancellationTokenSource = new CancellationTokenSource();
                _currentTask = ProcessSearchResultsAsync(movies, _cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async Task ProcessSearchResultsAsync(List<Movie> movies, CancellationToken cancellationToken)
        {
            var progress = new Progress<int>(value =>
            {
                progressBar.Value = value;
                infTask.Content = $"Processing {value}/{movies.Count} movies...";
            });

            try
            {
                for (int i = 0; i < movies.Count; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var movie = movies[i];
                    await LoadMovieAsync(movie);

                    ((IProgress<int>)progress).Report(i + 1);

                    await Task.Delay(1000);
                }

                infTask.Content = "Processing complete.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async Task LoadMovieAsync(Movie movie)
        {
            var titleData = await _api.TitleAsync(movie.Id);
            var bitmap = new BitmapImage();
            if (titleData.Image != null)
            {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(titleData.Image);
                bitmap.EndInit();
            }
            else
            {
                bitmap = new BitmapImage(new Uri("https://dummyimage.com/100x100/000000/ffffff.png&text=No+Image"));
            }
            movie.IMG = bitmap;
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            MessageBox.Show("Stop loading");
        }


        private async void resultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                if (resultsListBox.SelectedValue != null)
                {
                    var id = resultsListBox.SelectedValue.ToString();
                    var titleData = await _api.TitleAsync(id);
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(titleData.Image);
                    bitmap.EndInit();
                    image.Source = bitmap;
                    titleLabel.Content = titleData.FullTitle;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


    }

    public class SearchMovieResult
    {
        public int TotalResults { get; set; }
        public List<Movie> Results { get; set; }
    }

    public class Movie

    {
        public string Id { get; set; }
        public string Title { get; set; }
        public BitmapImage IMG { get; set; }

    }

}

