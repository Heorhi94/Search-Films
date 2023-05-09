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
        //k_hye43l7u no
        //k_y76947gh no
        //k_eknpnu11 no
        //k_lv26p9yv
        private readonly ApiLib _api = new ApiLib("k_lv26p9yv");
        private Task _currentTask;
        private CancellationTokenSource _cancellationTokenSource;
        List<Movie> movies;
        List<Movie> downloads;
        private int currentMovieIndex = 0;



        public MainWindow()
        {
            InitializeComponent();
        }


        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {

            if (movies != null)
            {
                movies.Clear();
            }
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
                downloads = new List<Movie>(); // Инициализация списка downloads
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
                    downloads.Add(movies[i]); // Добавить загруженный ранее элемент в список downloads
                    ((IProgress<int>)progress).Report(i + 1);
                    await LoadMovieAsync(downloads[i]);
                    await Task.Delay(1000);
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

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            MessageBox.Show("Pause loading");
        }


        private async void resultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                progressBar.Minimum = currentMovieIndex; // Установите минимальное значение progressBar на сохраненной позиции
                progressBar.Maximum = movies.Count; // Обновите максимальное значение progressBar
                _currentTask = ProcessSearchResultsAsync(_cancellationTokenSource.Token);
            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cancellationTokenSource != null && _cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();
            }
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

     /*       // Wyczyść listy i zresetuj indeks filmu
            movies.Clear();
            downloads.Clear();
            currentMovieIndex = 0;

            // Wyczyść wyświetlane informacje
            resultsListBox.ItemsSource = null;
            titleLabel.Content = "";
            image.Source = null;
            progressBar.Value = 0;*/

            // Zresetuj zadanie aktualnego procesu
            _currentTask = null;

            MessageBox.Show("Loading stopped");
            infTask.Content = "Searc stopping";
        }

    }



    public class Movie

    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string IMG { get; set; }

        public BitmapImage Image { get; set; }

    }

}

