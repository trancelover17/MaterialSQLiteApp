using System.Windows;
using System.Linq;
using System.Diagnostics;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MaterialDesign
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Albums[] items;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            { 
                Stopwatch s = Stopwatch.StartNew();

                using (var db = new DatabaseContext())
                {
                    items = db.Albums.OrderBy(b => b.AlbumId).ToArrayAsync().Result;
                }
                s.Stop();
                var ts = s.Elapsed;

                Dispatcher.Invoke(() =>
                {
                    time_text.Text = $"Time taken is: {String.Format("{0:00}:{1:00}", ts.Seconds, ts.Milliseconds / 10)}";
                    MyGrid.ItemsSource = items;
                });
            });

        }
        private void CycleBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar1.Value = 0;
            ProgressBar1.Minimum = 0;
            ProgressBar1.Maximum = MyGrid.Items.Count - 1;

            Task.Run(() => {
                for (int i = 0; i < items.Length - 1; i++)
                {
                    Task.Delay(10).Wait();
                    items[i].Title = $"+++ {items[i].Title}";
                    Dispatcher.Invoke( () =>
                    {
                        ProgressBar1.Value++;
                    });
                }
            });            
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".db"; // Default file extension
            dlg.Filter = "SQLite databases (*.db;*.sdb;*.sqlite)|*.db;*.sdb;*sqlite"; // Filter files by extension

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                DatabaseContext.db_path = dlg.FileName;
                FilenameTextbox.Text = dlg.FileName;
            }
        }
    }
}
