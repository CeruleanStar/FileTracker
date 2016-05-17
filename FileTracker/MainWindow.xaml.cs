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
            labelWelcome.Content = "Welcome, " + Environment.UserName + "!";
            DebugLog("[" + DateTime.Now + "] [" + Environment.UserName + "] has opened FileTracker");
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(TrackDirectory.Text.ToString()))
            {
                EditLastChange("Directory Changed to: " + TrackDirectory.Text.ToString());
                FileTrackWatcher(TrackDirectory.Text.ToString());
            }
            else
            {
                EditLastChange("Invalid Directory: " + TrackDirectory.Text.ToString());
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
            EditLastChange("File: [" + e.Name + "] has been [renamed]");
        }
        // make last change
        public void EditLastChange(string text)
        {
            text = "[" + DateTime.Now + "] " + text;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.LatestChange.Text = text;
                this.listboxChangeLog.Items.Add(text);
                this.DebugLog(text);
                NotifyIcon ico = new NotifyIcon();
                ico.ShowBalloonTip(3000, text, "Click to see log", ToolTipIcon.Info);
            }));
        }

        public void DebugLog(string text)
        {
            string LogPath = "C:\\Users\\" + Environment.UserName + "\\FileTracker.txt";
            TextWriter Logger = new StreamWriter(LogPath, true);
            try
            {
                if (File.Exists(LogPath))
                {
                    Logger.WriteLine(text);
                }
                else
                {
                    File.Create(LogPath);
                    Logger.WriteLine(text);
                }
            }
            catch (Exception)
            {
                // silent failure
            }
            Logger.Close();
        }
    }
}
