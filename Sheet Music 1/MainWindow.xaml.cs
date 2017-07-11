using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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
using Npgsql;
namespace Sheet_Music_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        

        public String SongTitle
        {
            get { return (String)GetValue(SongTitleProperty); }
            set { SetValue(SongTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SongTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SongTitleProperty =
            DependencyProperty.Register("SongTitle", typeof(String), typeof(MainWindow), new PropertyMetadata("Type Title Here"));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TitleSearch(object sender, RoutedEventArgs e)
        {
            Search("Select * FROM pdfs_from_pfp WHERE Song ILIKE @Parameter1", SongTitle);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SongTitle == "Type Title Here")
                SongTitle = string.Empty;
        }

        private void ComposerSearch(object sender, RoutedEventArgs e)
        {
            Search("SELECT * FROM pdfs_from_pfp WHERE Music ILIKE @Parameter1", Composer.Text);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DataTable Results = new DataTable();
            Results.Clear();
            using (var con = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=Sheet Music;User Id=postgres;Password=Roger;"))
                {
                    con.Open();
                    using (var cmd = new NpgsqlCommand("Select * FROM pdfs_from_pfp WHERE words ILIKE @Parameter3", con))
                    {
                        cmd.Parameters.AddWithValue("@Parameter3", "%" + Writer + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                        bool foundSong = false;
                        while (reader.Read())
                        {
                            foundSong = true;
                            if (Results.Columns.Count == 0)
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Results.Columns.Add(reader.GetName(i));
                                }
                            }
                            DataRow dr = Results.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dr[i] = reader.GetValue(i);
                            }
                            Results.Rows.Add(dr);

                        }
                        if (foundSong == false)
                        {
                            MessageBox.Show("Song Not Found!");
                        }
                    }
                        con.Close();
                    }
                }
            dataGrid.ItemsSource = Results.DefaultView;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            {
                DataTable Results = new DataTable();
                Results.Clear();
                using (var con = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=Sheet Music;User Id=postgres;Password=Roger;"))
                {
                    con.Open();
                    using (var cmd = new NpgsqlCommand("Select * FROM pdfs_from_pfp WHERE first_line ILIKE @Parameter4", con))
                    {
                        cmd.Parameters.AddWithValue("@Parameter4", "%" + FirstLine + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            bool foundSong = false;
                            while (reader.Read())
                            {
                                foundSong = true;
                                if (Results.Columns.Count == 0)
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        Results.Columns.Add(reader.GetName(i));
                                    }
                                }
                                DataRow dr = Results.NewRow();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dr[i] = reader.GetValue(i);
                                }
                                Results.Rows.Add(dr);

                            }
                            if (foundSong == false)
                            {
                                MessageBox.Show("Song Not Found!");
                            }
                        }
                        con.Close();
                    }
                }
                dataGrid.ItemsSource = Results.DefaultView;
            }
            //var Files = Directory.EnumerateFiles(@"C:\Users\Roger\Documents\Visual Studio 2017\Projects\Sheet Music 1\Pdf bin\", "*.pdf");
            //foreach (var item in Files)
            //{
            //   // dataGrid;
            //}
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = dataGrid.SelectedItem as DataRowView;
            
            System.Diagnostics.Process.Start(@"C:\Users\Roger\Documents\Visual Studio 2017\Projects\Sheet Music 1\Pdf bin\" + row["song"] + ".pdf");
        }

        private void Search(string query, string param1)
        {
            DataTable Results = new DataTable();
            Results.Clear();
            using (var con = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=Sheet Music;User Id=postgres;Password=Roger;"))
            {
                con.Open();
                using (var cmd = new NpgsqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Parameter1", "%" + param1 + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        bool foundSong = false;
                        while (reader.Read())
                        {
                            foundSong = true;
                            if (Results.Columns.Count == 0)
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Results.Columns.Add(reader.GetName(i));
                                }
                            }
                            DataRow dr = Results.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dr[i] = reader.GetValue(i);
                            }
                            Results.Rows.Add(dr);

                        }
                        if (foundSong == false)
                        {
                            MessageBox.Show("No Songs Found!");
                        }
                    }
                }
                con.Close();
            }
            dataGrid.ItemsSource = Results.DefaultView;
        }

    }
}
