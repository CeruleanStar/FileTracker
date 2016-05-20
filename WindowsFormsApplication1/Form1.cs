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
        public const string dirName1 = "Registration";
        public const string dirPath2 = "\\\\tchospital\\files\\faxes\\registration\\to be scheduled";
        public const string dirName2 = "To Be Scheduled";
        public const string dirPath3 = "\\\\fs3\\it\\other";
        public const string dirName3 = "IT\\other";

        public bool isbox1checked = false;
        public bool isbox2checked = false;
        public bool isbox3checked = false;

        public string ballonDir = dirPath1;

        public Form1()
        {
            InitializeComponent();

            notifyIcon1.BalloonTipText = "File Watcher Started";
            notifyIcon1.ShowBalloonTip(1000);

            watch1();
            watch2();
            watch3();
            
            
            
            // type a comment real quick there
            this.label2.Text = fileCount1(dirPath1).ToString();
            this.label2.ForeColor = setColor(dirPath1);
            this.label7.Text = fileCount1(dirPath2).ToString();
            this.label7.ForeColor = setColor(dirPath2);

            this.FormBorderStyle = FormBorderStyle.None;

            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new System.Drawing.Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);
        }


        public void watch1()
        {
            FileSystemWatcher fsWatch = new FileSystemWatcher();
            fsWatch.Path = dirPath1;
            fsWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fsWatch.Filter = "*.*";
            fsWatch.Created += new FileSystemEventHandler((sender, e) => onCreated(sender, e, dirPath1, isbox1checked, dirName1));
            fsWatch.Deleted += new FileSystemEventHandler(onDeleted);
            fsWatch.EnableRaisingEvents = true;
        }

        public void watch2()
        {
            FileSystemWatcher fsWatch = new FileSystemWatcher();
            fsWatch.Path = dirPath2;
            fsWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fsWatch.Filter = "*.*";
            fsWatch.Created += new FileSystemEventHandler((sender, e) => onCreated(sender, e, dirPath2, isbox2checked, dirName2));
            fsWatch.Deleted += new FileSystemEventHandler(onDeleted);
            fsWatch.EnableRaisingEvents = true;
        }

        public void watch3()
        {
        
            FileSystemWatcher fsWatch = new FileSystemWatcher();
            fsWatch.Path = dirPath3;
            fsWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fsWatch.Filter = "*.*";
            fsWatch.Created += new FileSystemEventHandler((sender, e) => onCreated(sender, e, dirPath3, isbox3checked, dirName3));
            fsWatch.Deleted += new FileSystemEventHandler(onDeleted);
            fsWatch.EnableRaisingEvents = true;
       
        }

        public void onCreated(object source, FileSystemEventArgs e, string dirPathin, bool ischecked, string dirNamein)
        {
            if (ischecked)
            {
                //this causes the taskbar icon to flash until the window is focused
                FlashWindow.Flash(this);

                ballonDir = dirPathin;



                //this sets the ballootip title, text, and timer to popup when a file is created in any watched path
                notifyIcon1.BalloonTipTitle = " !!!!!!!!!! ALERT !!!!!!!!!! ";
                notifyIcon1.BalloonTipText = "\r A New File Was Created In \r \r " + dirNamein;
                notifyIcon1.ShowBalloonTip(1000);
            }

            //this is where you add label text to keep it up to date on creation
            

            this.label2.Text = fileCount1(dirPath1).ToString();
            this.label2.ForeColor = setColor(dirPath1);
            this.label7.Text = fileCount1(dirPath2).ToString();
            this.label7.ForeColor = setColor(dirPath2);

        }

        public void onDeleted(object source, FileSystemEventArgs e)
        {
            //this is where you add label text to keep it up to date on deletion
            this.label2.Text = fileCount1(dirPath1).ToString();
            this.label2.ForeColor = setColor(dirPath1);
            this.label7.Text = fileCount1(dirPath2).ToString();
            this.label7.ForeColor = setColor(dirPath2);

           

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

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            isbox1checked = !isbox1checked;
        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            isbox2checked = !isbox2checked;
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            isbox3checked = !isbox3checked;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", dirPath2);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", dirPath1);
        }

        public Color setColor(string dirPathin)
        {
            if (fileCount1(dirPathin) == 0)
            {
                return Color.Green;
            }
            else if (fileCount1(dirPathin) > 0 && fileCount1(dirPathin) < 5)
            {
                return Color.Orange;
            }
            else if (fileCount1(dirPathin) >= 5)
            {
                return Color.Red;
            }
            else
            {
                return Color.HotPink;
            }
        }




    }





}
