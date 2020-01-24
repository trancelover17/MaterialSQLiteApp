using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Diagnostics;
using System;
using Microsoft.EntityFrameworkCore;

namespace MaterialDesign 
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Albums[] albums;

        public MainWindow()
        {
            InitializeComponent();            
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch s = Stopwatch.StartNew();

            using (var db = new AlbumsContext())
            {
                albums = db.Albums.OrderBy(b => b.AlbumId).ToArrayAsync().Result;
                MyGrid.ItemsSource = albums;
            }
            s.Stop();
            var ts = s.Elapsed;

            time_text.Text = $"Time taken is: {String.Format("{0:00}:{1:00}", ts.Seconds, ts.Milliseconds / 10)}";
        }
        private void CycleBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar1.Value = 0;
            ProgressBar1.Minimum = 0;
            ProgressBar1.Maximum = MyGrid.Items.Count - 1;

            foreach (var item in albums)
            {
                item.Title = $"+++ {item.Title}";
                ProgressBar1.Value++;
                MyGrid.Items.Refresh();
            }
        }
    }
}
