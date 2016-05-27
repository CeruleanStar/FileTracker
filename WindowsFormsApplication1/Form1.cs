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

        public string balloonDir = ""; //initialize this string as an empty string -- string is for the balloontip text when it displays a directory name
        public int watcherIterator = 0;
        public fileWatcher[] fileWatcherArray = new fileWatcher[100];

        public Form1()
        {
            InitializeComponent();

            //we have to give the balloontiptext some value, so this is a startup notification
            notifyIcon1.BalloonTipText = "File Watcher Started";
            notifyIcon1.ShowBalloonTip(100);
                      
            this.FormBorderStyle = FormBorderStyle.None; //set the form to borderless

            //get the desktop size, set the height of the form to 3/4 desktop height and lock it to the bottom right of the screen
            Rectangle workingArea = Screen.GetWorkingArea(this);            
            this.Height = workingArea.Height * 3 / 4;
            this.Location = new System.Drawing.Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);

            //create a label for the name of the program
            Label name = new Label();
            name.AutoSize = true;
            name.Text = "File and Folder Watcher";
            name.BackColor = Color.Black;
            name.ForeColor = Color.LimeGreen;
            name.Location = new System.Drawing.Point(0, 0);
            this.Controls.Add(name);
        }

        //the fileCreated method, this is called when a file is created in a watched folder -- it requires the source, e, the directory path, the boolean value in the corresponding boolean array, the directory name, the count label, and the current file(s) label
        public void fileCreated(object source, FileSystemEventArgs e, string dirPathin, bool ischecked, string dirNamein, Label ctLabel, Label cflabel)
        {
            //checks the boolean value in the corresponding element of the global boolean array
            if (ischecked)
            {
                //this causes the taskbar icon to flash until the window is focused
                FlashWindow.Flash(this);
                
                //set the balloonDir global variable to the full path of the object, so that we can use this variable in the balloon's on click method
                balloonDir = e.FullPath;

                //this sets the ballootip title, text, and timer to popup when a file is created in any watched path
                notifyIcon1.BalloonTipTitle = " !!!!!!!!!! ALERT !!!!!!!!!! ";
                notifyIcon1.BalloonTipText = "\r A New File Was Created In \r \r " + dirNamein;
                notifyIcon1.ShowBalloonTip(1000);
            }
        }

        //this is the method to both bring the program to the front and open an explorer window to the path that generated the alert
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Activate();           
            System.Diagnostics.Process.Start(balloonDir);          
        }

        private void addPathBtn_Click(object sender, EventArgs e)
        {
            //pop up window to request Name and Path
            Prompt prompt = new Prompt();
            prompt.ShowDialog();
         
            fileWatcherArray[watcherIterator] = new fileWatcher(prompt.nameIn, prompt.pathIn, this);
            
            fileWatchConstruct(watcherIterator);
            watcherIterator++;        
        }

        private void fileWatchConstruct(int i)
        {
            //set name label values
            fileWatcherArray[i].nameLabel.AutoSize = true;
            fileWatcherArray[i].nameLabel.BackColor = Color.Black;
            fileWatcherArray[i].nameLabel.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, GraphicsUnit.Point, ((byte)(0)));
            fileWatcherArray[i].nameLabel.ForeColor = Color.CornflowerBlue;
            fileWatcherArray[i].nameLabel.Location = new System.Drawing.Point(35, 40 + (i * 40));
            this.Controls.Add(fileWatcherArray[i].nameLabel);

            //set count label values
            fileWatcherArray[i].countLabel.AutoSize = true;
            fileWatcherArray[i].countLabel.BackColor = Color.Black;
            fileWatcherArray[i].countLabel.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, GraphicsUnit.Point, ((byte)(0)));
            fileWatcherArray[i].countLabel.Location = new System.Drawing.Point(38, 60 + (i * 40));
            fileWatcherArray[i].updateCountLabel();
            this.Controls.Add(fileWatcherArray[i].countLabel);

            //set cf label values
            fileWatcherArray[i].cfLabel.AutoSize = true;
            fileWatcherArray[i].cfLabel.BackColor = Color.Black;
            fileWatcherArray[i].cfLabel.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, GraphicsUnit.Point, ((byte)(0)));
            fileWatcherArray[i].cfLabel.ForeColor = Color.White;
            fileWatcherArray[i].cfLabel.Location = new System.Drawing.Point(105, 60 + (i * 40));
            this.Controls.Add(fileWatcherArray[i].cfLabel);

            //set checkbox values

            fileWatcherArray[i].watchBox.AutoSize = true;
            fileWatcherArray[i].watchBox.Location = new System.Drawing.Point(18, 40 + (i*40));
            fileWatcherArray[i].watchBox.Size = new System.Drawing.Size(17, 17);           
            fileWatcherArray[i].watchBox.UseVisualStyleBackColor = true;
            this.Controls.Add(fileWatcherArray[i].watchBox);

            //set delete button values
            fileWatcherArray[i].delButton.FlatAppearance.BorderColor = Color.Black;
            fileWatcherArray[i].delButton.FlatStyle = FlatStyle.Flat;
            fileWatcherArray[i].delButton.ForeColor = Color.Red;
            fileWatcherArray[i].delButton.Location = new System.Drawing.Point(145, 40 +(i*40));
            fileWatcherArray[i].delButton.Size = new System.Drawing.Size(18, 21);
            fileWatcherArray[i].delButton.Font = new Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            fileWatcherArray[i].delButton.Text = "X";
            fileWatcherArray[i].delButton.UseVisualStyleBackColor = true;
            this.Controls.Add(fileWatcherArray[i].delButton);
            fileWatcherArray[i].delButton.Click += new EventHandler((sender, e) => onDelClick(fileWatcherArray[i], e));
         
        }

        public void flasher()
        { this.Invoke(new Action(() => FlashWindow.Flash(this))); }


        public void onFileCreated(object source, FileSystemEventArgs e, string dirPathin, bool ischecked, string dirNamein)
        {
           if (ischecked)
            {
                flasher();

                balloonDir = e.FullPath;

                notifyIcon1.BalloonTipTitle = " !!!!!!!!!! ALERT !!!!!!!!!! ";
                notifyIcon1.BalloonTipText = "\r A New File Was Created In \r \r " + dirNamein;
                notifyIcon1.ShowBalloonTip(1000);
            }
            updateLabels();
        }

        public void onFileDeleted()
        { updateLabels(); }

        private void updateLabels()
        {
            foreach (fileWatcher fWatch in fileWatcherArray)
            {
                if (fWatch != null)
                {
                    fWatch.nameLabel.Location = new System.Drawing.Point(35, 40 + (getArrayIndexValue(fWatch) * 40));
                    fWatch.countLabel.Location = new System.Drawing.Point(38, 60 + (getArrayIndexValue(fWatch) * 40));
                    fWatch.cfLabel.Location = new System.Drawing.Point(105, 60 + (getArrayIndexValue(fWatch) * 40));
                    fWatch.delButton.Location = new System.Drawing.Point(145, 40 + (getArrayIndexValue(fWatch) * 40));
                    fWatch.watchBox.Location = new System.Drawing.Point(18, 40 + (getArrayIndexValue(fWatch) * 40));

                    this.Invoke(new Action(() => fWatch.updateCountLabel()));
                    this.Invoke(new Action(() => fWatch.updateCFLabel()));
                }              
            }
        }

        private void updateArray(int i)
        {            
            for (int j = 0; j < 20; j++)
            {
                if (j >= i && j != 99)
                {
                    fileWatcherArray[j] = fileWatcherArray[j + 1];
                }
                if (j == 99)
                {
                    fileWatcherArray[j] = null;
                }
            }          
            updateLabels();            
        }

        private void onDelClick(fileWatcher source, EventArgs e)
        {
            watcherIterator--;
            updateArray((getArrayIndexValue(source)));
        }

        private int getArrayIndexValue(fileWatcher source)
        { return Array.IndexOf(fileWatcherArray, source); } 
    }

    //********* New Class fileWatcher
    public class fileWatcher
    {
        public Label nameLabel = new Label();
        public Label countLabel = new Label();
        public Label cfLabel = new Label();
        public CheckBox watchBox = new CheckBox();
        public FileSystemWatcher fsWatcher = new FileSystemWatcher();
        public Button delButton = new Button();
        public Form1 mainForm;
        public string dirName;
        public string dirPath;

        public fileWatcher(string dirNameIn, string dirPathIn, Form1 mainFormIn)
        {
            dirName = dirNameIn;
            dirPath = dirPathIn;
            mainForm = mainFormIn;

            fsWatcher.Path = dirPath;
            nameLabel.Text = dirName;

            delButton.Click += new EventHandler((sender, e) => onDelClick(sender, e));
            nameLabel.Click += new EventHandler((sender, e) => fsLabel_Click(sender, e, dirPath));
            updateCFLabel();
            fsWatcher.Created += new FileSystemEventHandler((sender, e) => onFileCreated(sender, e));
            fsWatcher.Deleted += new FileSystemEventHandler((sender, e) => onFileDeleted(sender, e));
            fsWatcher.EnableRaisingEvents = true;

            watchBox.Checked = true;
        }

        //event handlers for the filewatcher object
        //on-click of name label to open explorer to the path
        private void fsLabel_Click(object sender, EventArgs e, string dirPath)
        { System.Diagnostics.Process.Start("explorer.exe", dirPath); }

        //on-click of delBox to dispose the object
        public void onDelClick(object source, EventArgs e)
        { this.Dispose(); }

        //on-file created in the path
        public void onFileCreated(object source, FileSystemEventArgs e)
        { mainForm.onFileCreated(source, e, dirPath, this.watchBox.Checked, dirName); }

        //on-file deleted in the path
        public void onFileDeleted(object source, EventArgs e)
        { mainForm.onFileDeleted(); }

        //update the CF label for plurality
        public void updateCFLabel()
        {
            if(fileCount(dirPath) == 1)
            { cfLabel.Text = "Current File"; }
            else
            { cfLabel.Text = "Current Files"; }
        }

        //update the count label text
        public void updateCountLabel()
        {
            countLabel.ForeColor = setColor(dirPath);
            countLabel.Text = fileCount(dirPath).ToString();
        }

        //return the file count for the path, sans *.db files
        public int fileCount(string dirPathin)
        {
            var allfiles = Directory.GetFiles(dirPathin, "*", SearchOption.TopDirectoryOnly).Where(name => !name.EndsWith(".db"));
            int count = allfiles.Count();
            return count;
        }

        //return a color based on file count of the path
        public Color setColor(string dirPathin)
        {
            if (fileCount(dirPathin) == 0)
            { return Color.Green; }
            else if (fileCount(dirPathin) > 0 && fileCount(dirPathin) < 5)
            { return Color.DarkOrange; }
            else if (fileCount(dirPathin) >= 5)
            { return Color.Red; }
            else
            { return Color.HotPink; }            
        }

        public void Dispose()
        {
            nameLabel.Dispose();
            countLabel.Dispose();
            cfLabel.Dispose();
            fsWatcher.Dispose();
            watchBox.Dispose();
            delButton.Dispose();
        }
    }

    //****** New Class Prompt
    public class Prompt
    {
        public string nameIn = "";
        public string pathIn = "";

        public void ShowDialog()
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,               
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Enter Directory Name" };
            textLabel.AutoSize = true;  
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            textBox.TabIndex = 1;
            Label pathLabel = new Label() { Left = 50, Top = 80, Text = "Enter Directory Path" };
            TextBox dirBox = new TextBox() { Left = 50, Top = 110, Width = 400 };
            dirBox.TabIndex = 2;
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 170, DialogResult = DialogResult.OK };
            confirmation.TabIndex = 3;
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(pathLabel);
            prompt.Controls.Add(dirBox);
            prompt.AcceptButton = confirmation;

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                nameIn = textBox.Text;
                pathIn = dirBox.Text;              
            }
        }
    }
}
