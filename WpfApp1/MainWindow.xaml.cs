

using IMDbApiLib;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //k_hye43l7u
        //k_y76947gh
        //k_eknpnu11
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

                // Clear UI elements
                resultsListBox.ItemsSource = null;
                titleLabel.Content = "";
                image.Source = null;
                progressBar.Value = 0;

                var result = await _api.SearchMovieAsync(searchTextBox.Text);
                var movies = result.Results.Select(r => new Movie { Id = r.Id, Title = r.Title }).ToList();
                resultsListBox.ItemsSource = movies;
                resultsListBox.DisplayMemberPath = "Title";
                resultsListBox.SelectedValuePath = "Id";
                progressBar.Maximum = result.Results.Count;
                // Process results
                _cancellationTokenSource = new CancellationTokenSource();
                _currentTask = ProcessSearchResultsAsync(movies, _cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void resultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Cancel any previous task
                _cancellationTokenSource?.Cancel();

                // Get movie details and display them
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


        private async Task ProcessSearchResultsAsync(List<Movie> movies, CancellationToken cancellationToken)
        {
            // Set up progress reporting
            var progress = new Progress<int>(value =>
            {
                progressBar.Value = value;
                infTask.Content = $"Processing {value}/{movies.Count} movies...";
            });

            try
            {
                int i = 0;
                while (i < movies.Count && i < 25)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var movie = movies[i];
                    Task loadTask = LoadMovieAsync(movie);

                    // Add item to list box when image loaded
                    await loadTask.ContinueWith(async t =>
                    {
                        await Task.Delay(1000);
                        Dispatcher.Invoke(() =>
                        {
                            ((IList<Movie>)resultsListBox.ItemsSource).Add(movie);
                        });
                    }, cancellationToken);


                    i++;
                    ((IProgress<int>)progress).Report(i);

                    // Delay for 1 second
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
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(titleData.Image);
            bitmap.EndInit();
            movie.IMG = bitmap;
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Cancel current task
            _cancellationTokenSource?.Cancel();
        }

        private async void stopButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();

            // Stop loading items into list box
            _cancellationTokenSource = new();
        }

        private void resumeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTask == null || _currentTask.IsCompleted)
            {
                // Clear UI elements
                resultsListBox.ItemsSource = null;
                titleLabel.Content = "";
                image.Source = null;
                progressBar.Value = 0;

                var result = _api.SearchMovieAsync(searchTextBox.Text).Result;
                var movies = result.Results.Select(r => new Movie { Id = r.Id, Title = r.Title }).ToList();
                resultsListBox.ItemsSource = movies;
                resultsListBox.DisplayMemberPath = "Title";
                resultsListBox.SelectedValuePath = "Id";
                progressBar.Maximum = result.Results.Count;

                _cancellationTokenSource = new CancellationTokenSource();
                _currentTask = ProcessSearchResultsAsync(movies, _cancellationTokenSource.Token);
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
        // другие свойства фильма
        public BitmapImage IMG { get; set; }

    }

}

