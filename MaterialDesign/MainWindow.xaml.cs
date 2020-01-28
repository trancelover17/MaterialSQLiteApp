using System.Windows;
using System.Linq;
using System.Diagnostics;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace MaterialDesign
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

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch s = Stopwatch.StartNew();

            using (var db = new DatabaseContext())
            {
                var query = from album in db.Albums
                            join artist in db.Artists on album.ArtistId equals artist.ArtistId
                            select new { album.Title, artist.Name };
                MyGrid.ItemsSource = query.ToArrayAsync().Result;
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

            //foreach (var item in albums)
            //{
            //    item.Album = $"+++ {item.Album}";
            //    ProgressBar1.Value++;
            //    MyGrid.Items.Refresh();
            //}
            for (int i = 0; i < MyGrid.Items.Count - 1; i++)
            {
                ProgressBar1.Value++;
            }
        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            ////TextBox t = (TextBox)sender;
            //string filter = Filter_Textbox.Text;
            //ICollectionView cv = CollectionViewSource.GetDefaultView(MyGrid.ItemsSource);
            //if (filter == "")
            //    cv.Filter = null;
            //else
            //{
            //    cv.Filter = o =>
            //    {
            //        return MyGrid.Items.Contains(filter);
            //        //Person p = o as Person;
            //        //if (t.Name == "txtId")
            //        //    return (p.Id == Convert.ToInt32(filter));
            //        //return (p.Name.ToUpper().StartsWith(filter.ToUpper()));
            //    };
            //}
        }
    }
}
