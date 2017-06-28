using System;
using System.Collections.Generic;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ImportFiles();
            return;

            using (var con = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=Sheet Music;User Id=postgres;Password=Roger;"))
            {
                con.Open();
                using (var cmd = new NpgsqlCommand("Select * FROM pdfs_from_pfp WHERE Song LIKE @Parameter1", con))
                {
                    cmd.Parameters.AddWithValue("@Parameter1", "%"+SongTitle+"%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        bool foundSong = false;
                        while (reader.Read())
                        {
                            foundSong = true;
                            MessageBox.Show(reader.GetString(0));
                        }
                        if (foundSong == false)
                        {
                            MessageBox.Show("Song Not Found!");
                        }
                    }
                }
                con.Close();
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SongTitle == "Type Title Here")
                SongTitle = string.Empty;
        }

        private void ImportFiles()
        {
            var Files = Directory.EnumerateFiles(@"c:\pdfs\", "*.pdf");
            foreach (var item in Files)
            {
                MessageBox.Show(item);
            }
        }
    }
}
