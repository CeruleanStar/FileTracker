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
        public Form1()
        {
            InitializeComponent();
            
            notifyIcon1.BalloonTipText = "File Watcher Started";
            notifyIcon1.ShowBalloonTip(1000);

            watch1();
            this.label2.Text = fileCount1().ToString();
 
        }


        public void watch1()
        {
            FileSystemWatcher fsWatch = new FileSystemWatcher();
            fsWatch.Path = "[Folder Location]";
            fsWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fsWatch.Filter = "*.*";
            fsWatch.Created += new FileSystemEventHandler(onCreated);
            fsWatch.Deleted += new FileSystemEventHandler(onDeleted);
            fsWatch.EnableRaisingEvents = true;
        }


        public void onCreated(object source, FileSystemEventArgs e)
        {
            FlashWindow.Flash(this);
            this.label2.Text = fileCount1().ToString();
            notifyIcon1.BalloonTipTitle = " !!!!!!!!!! ALERT !!!!!!!!!! ";
            notifyIcon1.BalloonTipText = "A New File Was Created In [Folder Location]";
            notifyIcon1.ShowBalloonTip(1000);       
        }



        public void onDeleted(object source, FileSystemEventArgs e)
        {
            this.label2.Text = fileCount1().ToString();
        }


        public int fileCount1()
        {
            int count1 = Directory.GetFiles("[Folder Location]", "*", SearchOption.TopDirectoryOnly).Length;
            return count1;
        }


    }





}
