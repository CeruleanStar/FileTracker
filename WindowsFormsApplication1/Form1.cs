using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using System.Windows.Threading;



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public const string dirPath1 = "\\\\tchospital\\files\\faxes\\registration";
        public const string dirPath2 = "\\\\tchospital\\files\\faxes\\registration\\to be scheduled";
        public const string dirPath3 = "[directory path 3]";

        public string ballonDir = dirPath1;

        public Form1()
        {
            InitializeComponent();

            notifyIcon1.BalloonTipText = "File Watcher Started";
            notifyIcon1.ShowBalloonTip(1000);

            watch1();
            watch2();
            // watch3();

            this.linkLabel1.Text = dirPath1;
            this.label5.Text = dirPath2;
            
            // type a comment real quick there
            this.label2.Text = fileCount1(dirPath1).ToString();
            this.label7.Text = fileCount1(dirPath2).ToString();

        }


        public void watch1()
        {
            FileSystemWatcher fsWatch = new FileSystemWatcher();
            fsWatch.Path = dirPath1;
            fsWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fsWatch.Filter = "*.*";
            fsWatch.Created += new FileSystemEventHandler((sender, e) => onCreated(sender, e, dirPath1));
            fsWatch.Deleted += new FileSystemEventHandler(onDeleted);
            fsWatch.EnableRaisingEvents = true;
        }

        public void watch2()
        {
            FileSystemWatcher fsWatch = new FileSystemWatcher();
            fsWatch.Path = dirPath2;
            fsWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fsWatch.Filter = "*.*";
            fsWatch.Created += new FileSystemEventHandler((sender, e) => onCreated(sender, e, dirPath2));
            fsWatch.Deleted += new FileSystemEventHandler(onDeleted);
            fsWatch.EnableRaisingEvents = true;
        }

        public void watch3()
        {
            FileSystemWatcher fsWatch = new FileSystemWatcher();
            fsWatch.Path = dirPath3;
            fsWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fsWatch.Filter = "*.*";
            fsWatch.Created += new FileSystemEventHandler((sender, e) => onCreated(sender, e, dirPath3));
            fsWatch.Deleted += new FileSystemEventHandler(onDeleted);
            fsWatch.EnableRaisingEvents = true;
        }

        public void onCreated(object source, FileSystemEventArgs e, string dirPathin)
        {
            //this causes the taskbar icon to flash until the window is focused
            FlashWindow.Flash(this);

            ballonDir = dirPathin;
            //this is where you add label text to keep it up to date on creation
            this.label2.Text = fileCount1(dirPath1).ToString();
            this.label7.Text = fileCount1(dirPath2).ToString();


            //this sets the ballootip title, text, and timer to popup when a file is created in any watched path
            notifyIcon1.BalloonTipTitle = " !!!!!!!!!! ALERT !!!!!!!!!! ";
            notifyIcon1.BalloonTipText = "\r A New File Was Created In \r " + dirPathin;
            notifyIcon1.ShowBalloonTip(1000);
        }

        public void onDeleted(object source, FileSystemEventArgs e)
        {
            //this is where you add label text to keep it up to date on deletion
            this.label2.Text = fileCount1(dirPath1).ToString();
            this.label7.Text = fileCount1(dirPath2).ToString();



        }


        public int fileCount1(string countPath)
        {
            var allfiles = Directory.GetFiles(countPath, "*", SearchOption.TopDirectoryOnly).Where(name => !name.EndsWith(".db"));
            int count1 = allfiles.Count();
            return count1;
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Activate();
           
            System.Diagnostics.Process.Start("explorer.exe", ballonDir);
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", dirPath1);
        }
    }





}
