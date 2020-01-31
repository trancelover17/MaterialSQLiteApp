using System.Windows;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Collections;

namespace MaterialDesign
{    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Albums> items = new List<Albums>();
        //static string tableCommand = "select * from Albums";
        static string dbpath = string.Empty;
        static SqliteConnection db;

        public MainWindow()
        {
            InitializeComponent();
        }

        async void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            // There is no way (at least, I didn't find it) to load data via EF Core asynchronously.
            // Because of using 'DbContext' class obtaining data from DB will be always run synchronously, and
            // the UI thread will be freeze until the reading ends. So, that's why we may use Task.Run()
            // for running reading in another thread from threap pool. Or, use Microsoft.Data.Sqlite library
            // which supports async methods for SQLite databases access.

            if (string.IsNullOrEmpty(dbpath) || tablesCombobox.Text == string.Empty)
            {
                MessageBox.Show("DB file or table isn't choosen!");
                return;
            }
            Stopwatch s = Stopwatch.StartNew();

            await PerformSQLiteAsync(new SqliteCommand("select * from Albums", db));

            s.Stop();
            var ts = s.Elapsed;

            time_text.Text = $"Time taken is: {string.Format("{0:00}:{1:00}", ts.Seconds, ts.Milliseconds / 10)}";
            MyGrid.ItemsSource = items;
        }
        async static Task PerformSQLiteAsync(SqliteCommand tableCommand)
        {
            await db.OpenAsync();

            SqliteDataReader query = await tableCommand.ExecuteReaderAsync();

            while (await query.ReadAsync())
            {
                items.Add(new Albums { AlbumId = query.GetInt32(0), Title = query.GetString(1), ArtistId = query.GetInt32(2) });
            }
            await db.CloseAsync();
        }
        async static Task<string[]> GetTablesAsync()
        {
            var query = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY name";
            await db.OpenAsync();
            SqliteDataReader dbreader = await new SqliteCommand(query, db).ExecuteReaderAsync();
            List<string> tables = new List<string>();

            while (await dbreader.ReadAsync())
            {
                //items.Add(new Albums { AlbumId = dbreader.GetInt32(0), Title = dbreader.GetString(1), ArtistId = dbreader.GetInt32(2) });
                tables.Add(dbreader.GetString(0));
            }
            await db.CloseAsync();
            return tables.ToArray();
        }
        void CycleBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar1.Value = 0;
            ProgressBar1.Minimum = 0;
            ProgressBar1.Maximum = MyGrid.Items.Count - 1;

            Task.Run(() => {
                for (int i = 0; i < items.Count - 1; i++)
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

        private async void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "SQLite databases (*.db;*.sdb;*.sqlite)|*.db;*.sdb;*sqlite"; // Filter files by extension

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                dbpath = dlg.FileName;
                db = new SqliteConnection($"Filename={dlg.FileName}");
                FilenameTextbox.Text = dlg.FileName;
            }
            //string[] tables = { "123", "456", "789" };
            this.DataContext = new ViewModel(await GetTablesAsync());
        }
    }
}