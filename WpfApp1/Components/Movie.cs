using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MovieSearch.Components
{
    public class Movie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string IMG { get; set; }

        public BitmapImage Image { get; set; }

    }
}
