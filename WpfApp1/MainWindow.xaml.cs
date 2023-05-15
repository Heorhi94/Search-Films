using IMDbApiLib;
using MovieSearch;
using MovieSearch.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        //k_hye43l7u no
        //k_y76947gh no
        //k_eknpnu11 no
        //k_lv26p9yv no
       // k_76o581d7
        private readonly ApiLib _api = new("k_76o581d7");
        private Task _currentTask;
        private CancellationTokenSource _cancellationTokenSource;
        List<Movie> movies;
        List<Movie> downloads;
        private int currentMovieIndex = 0;
        


        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Search()
        {
            movies?.Clear();
            try
            {
                _cancellationTokenSource?.Cancel();
                _currentTask?.Wait();

                resultsListBox.ItemsSource = null;
                titleLabel.Content = "";
                image.Source = null;
                progressBar.Value = 0;
                
                var result = await _api.SearchMovieAsync(searchTextBox.Text);
                movies = result.Results.Select(r => new Movie { Id = r.Id, Title = r.Title, IMG = r.Image }).ToList();
                downloads = new List<Movie>();
                resultsListBox.ItemsSource = movies;
                resultsListBox.DisplayMemberPath = "Title";
                resultsListBox.SelectedValuePath = "Id";
                progressBar.Minimum = 0;
                progressBar.Maximum = movies.Count;

                _cancellationTokenSource = new CancellationTokenSource();
                _currentTask = ProcessSearchResultsAsync(_cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private async Task ProcessSearchResultsAsync(CancellationToken cancellationToken)
        {
            var progress = new Progress<int>(value =>
            {
                progressBar.Value = value;
                infTask.Content = $"Processing {value}/{movies.Count} movies...";
            });

            try
            {
                for (int i = currentMovieIndex; i < movies.Count; i++)
                {
                    downloads.Add(movies[i]);
                    ((IProgress<int>)progress).Report(i + 1);
                    await LoadMovieAsync(downloads[i]);
                    await Task.Delay(1000, cancellationToken);
                    currentMovieIndex = i + 1;
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }

                infTask.Content = $"Processing paused. Downloaded {currentMovieIndex}/{movies.Count} movies";
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
            movie.Image = bitmap;
            progressBar.Dispatcher.Invoke(() => progressBar.Value++);
            titleLabel.Content = titleData.FullTitle;
        }

        private async void SelectItem()
        {
            try
            {
                if (resultsListBox.SelectedIndex != -1)
                {
                    int res = resultsListBox.SelectedIndex;
                    var id = downloads[res].Id;
                    var titleData = await _api.TitleAsync(id);
                    var bitmap = new BitmapImage();
                    if (titleData.Image != null)
                    {
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(downloads[res].IMG);
                        bitmap.EndInit();
                    }

                    else
                    {
                        bitmap = new BitmapImage(new Uri("https://dummyimage.com/100x100/000000/ffffff.png&text=No+Image"));
                    }
                    image.Source = bitmap;
                    titleLabel.Content = titleData.FullTitle;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void Stop()
        {
            if (_cancellationTokenSource != null && _cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();
            }
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            _currentTask = null;

            MessageBox.Show("Loading stopped");
            infTask.Content = "Search stopped";
        }

        private  void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           Search();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            MessageBox.Show("Pause");
        }

        private void ResultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectItem();

        }

        private void СontinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                progressBar.Minimum = currentMovieIndex; 
                progressBar.Maximum = movies.Count;
                _currentTask = ProcessSearchResultsAsync(_cancellationTokenSource.Token);
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
           Stop();
        }

        private void AdvancedSearchButton_Click(object sender, RoutedEventArgs e)
        {
            AdvancedSearch advancedSearch = new();
            advancedSearch.Show();
            
        }
    }
}

