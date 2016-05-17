using System;
using System.Collections.Generic;
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
using System.IO;
using System.Windows.Forms;

namespace FileTracker
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

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(TrackDirectory.Text.ToString()))
            {
                EditLastChange("[" + DateTime.Now + "] Directory Changed to: " + TrackDirectory.Text.ToString());
                FileTrackWatcher(TrackDirectory.Text.ToString());
            }
            else
            {
                TrackDirectory.Text = "Invalid Directory: " + TrackDirectory.Text.ToString();
                LatestChange.Text = "Invalid Directory: " + TrackDirectory.Text.ToString();
            }
        }

        public void FileTrackWatcher(string Dir)
        {
            // start the file system watcher
            var fs = new FileSystemWatcher(Dir, "*.*");
            // Set Notification Filters
            fs.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // enable events
            fs.EnableRaisingEvents = true;
            // Now event handlers
            fs.Created += new FileSystemEventHandler(Changes);
            fs.Changed += new FileSystemEventHandler(Changes);
            fs.Deleted += new FileSystemEventHandler(Changes);
            fs.Renamed += new RenamedEventHandler(Renamed);
        }

        private void Changes(object source, FileSystemEventArgs e)
        {
            EditLastChange("File: [" + e.Name + "] has been: [" + e.ChangeType + "]");
        }

        private void Renamed(object source, FileSystemEventArgs e)
        {
            EditLastChange("[" + DateTime.Now + "] File: [" + e.Name + "] has been [renamed]");
        }
        // make last change
        public void EditLastChange(string text)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.LatestChange.Text = text;
                this.listboxChangeLog.Items.Add(text);
            }));
        }
    }
}
