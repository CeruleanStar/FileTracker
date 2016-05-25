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

        //public bool[] isboxchecked = new bool[20]; //this is a boolean array to keep track of whether checkboxes are checked or not
        public string balloonDir = ""; //initialize this string as an empty string -- string is for the balloontip text when it displays a directory name
        public int labelPosition = 40; //this is the dynamic label position variable to make sure labels don't overlap -- this is for the directory name labels
        public int countLabelPosition = 40; //this is the count label position variable for dynamic count labels
        public int chkBoxPosition = 40; //this is for the dynamic checkbox positions
        public int cfLabelPosition = 40; //this is for the 'current file(s)' dynamic labels
        public int watcherIterator = 40;
        public int posMultiplier = 1;
        //public string[] pathArray = new string[20]; //declare the string array that will hold the paths, and set it to half the elements of the array from the config file
        //public string[] nameArray = new string[20]; //declare the string array that will hodl the path Name and set it to half the elements of the array from the config file
        
            

        public Form1()
        {
            InitializeComponent();

            //we have to give the balloontiptext some value, so this is a startup notification
            notifyIcon1.BalloonTipText = "File Watcher Started";
            notifyIcon1.ShowBalloonTip(100);

            //check if the config.txt file exists in the same directory with the program - if not, display a message and exit the program

            /* ----------------------------------
            if (!File.Exists("config.txt"))
            {
                System.Windows.Forms.MessageBox.Show("config.txt file does not exist -- please create a config.txt file within the same folder as the program");
                closeApp.closeApplication();
            }
            -------------------------------------*/


            //string[] textFile = File.ReadAllLines("config.txt"); //declare a string array and put in each line from the config file      

            /*
            //set the entire boolean array to true, as all of the checkboxes will start the program as 'checked' (true)
            for (int i = 0; i < isboxchecked.Length; i++)
            { isboxchecked[i] = true; }
            */

            //checks if the config file has the correct number of lines by checking if the string array is an even number
            //if config file has an odd number of lines, then pop up a message and close the program
            /* ---------------------------
            if (textFile.Length % 2 != 0)
            {
                System.Windows.Forms.MessageBox.Show("The config file is not an even number of lines -- The format for the config file is Directory name, new line, Directory Path -- check for white space at the end of file -- close the program and verify the config file is correct");
                closeApp.closeApplication();
            }
            ------------------------------*/

            /*------------------
            int j = 0; //counter variable used to separate the input string array into the pathArray and the nameArray
            //the next 2 for loops will separate out the textFile array into 2 new arrays, one containing all of the directory paths, and the other the directory names
            for (int i = 0; i < textFile.Length / 2; i++)
            {
                nameArray[i] = textFile[j];
                j += 2;
            }
            j = 1;
            for (int i = 0; i < textFile.Length / 2; i++)
            {
                pathArray[i] = textFile[j];
                j = j + 2;
            }
            ----------------------*/

            //this foreach loop will test every path in the pathArray to see if the path exists, if not it will close the program
            /* -----------------------------
            foreach (string path in pathArray)
            {
                if (!Directory.Exists(path))
                {
                    System.Windows.Forms.MessageBox.Show("the path " + path + " does not appear to exist -- check the spelling of the path in the config file");
                    closeApp.closeApplication();
                }
            }
            ---------------------*/
            /*---------------------------
            using (StreamWriter sw = new StreamWriter(@"C:\nhs\outfile.txt", false))
            {
                foreach (string line in textFile)
                {
                    sw.WriteLine(line);
                }
                sw.WriteLine("******next is the name array******");
                foreach (string line in nameArray)
                {
                    sw.WriteLine(line);
                }
                sw.WriteLine("******next is the path array******");
                foreach (string line in pathArray)
                {
                    sw.WriteLine(line);
                }
            }
            -------------------------------*/

            //this for loop will try to create each file watcher object and if it has an exception when attempting to create one, it is likely due to the user not having permissions to view a directory in the config file
            //if an exception happens when attempting to create a watcher object, it will display a message regarding folder permissions and close the program

            /*---------------------------
            for (int i = 0; i < pathArray.Length; i++)
            {
                try
                { 
                    watcher(pathArray[i], nameArray[i], i);
                }
                catch
                {
                    System.Windows.MessageBox.Show("error with " + pathArray[i] + " --- the current user likely does not have sufficient access to this location");
                    closeApp.closeApplication();
                }
            }
            ----------------------------*/

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


        //this is the filewatcher creation method, it requires paramaters of the directory path to watch, the name of that directory(can be anything you want)
        //and the corresponding value of the isboxchecked global boolean array for the checkbox that will be created with the filewatcher object
        public void watcher(string dirPath, string dirName, int chkbox)
        {
            
            //create the countlabel for the filewatcher
            Label countLabel = new Label();
            countLabel.AutoSize = true;
            countLabel.BackColor = Color.Black;    
            countLabel.ForeColor = setColor(dirPath, countLabel); //the count label color changes based on number of files in the directory -- the filecount method requires a label object parameter which is why we pass one here
            countLabel.Location = new System.Drawing.Point(38, countLabelPosition * posMultiplier + 20); //sets the location of the count label, the countLabelPos method will change the label position global variable every time this label is created so that labels don't overlap
            countLabel.Text = fileCount(dirPath, countLabel).ToString();    //set the text of the count label to the number corresponding to the filecount of watched folder       
            this.Controls.Add(countLabel);

            //create the 'current file(s)' label in similar fashion to the countlabel
            Label cfLabel = new Label();
            cfLabel.AutoSize = true;
            cfLabel.BackColor = System.Drawing.Color.Black;
            cfLabel.ForeColor = System.Drawing.SystemColors.Control;
            cfLabel.Location = new System.Drawing.Point(85, cfLabelPosition * posMultiplier + 20);        
            cfLabel.Text = "Current Files";
            this.Controls.Add(cfLabel);


            //create the name label based on the dirname and create an on-click event handler to call the fsLabel_Click method
            Label nameLabel = new Label();
            nameLabel.AutoSize = true;
            nameLabel.BackColor = Color.Black;
            nameLabel.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, GraphicsUnit.Point, ((byte)(0)));
            nameLabel.ForeColor = Color.CornflowerBlue;
            nameLabel.Location = new System.Drawing.Point(35, labelPosition * posMultiplier);                       
            nameLabel.Text = dirName;
            nameLabel.Click += new EventHandler((sender, e) => this.fsLabel_Click(sender, e, dirPath));
            this.Controls.Add(nameLabel);

            //create the checkbox and an event handler for the checkoxes state change, which calls the checkBx_CheckStateChanged method
            CheckBox checkBx = new CheckBox();
            checkBx.AutoSize = true;
            checkBx.Checked = true;
            checkBx.CheckState = CheckState.Checked;
            checkBx.Location = new System.Drawing.Point(14, chkBoxPosition * posMultiplier);
            checkBx.Size = new System.Drawing.Size(15, 14);
            checkBx.UseVisualStyleBackColor = true;
            this.Controls.Add(checkBx);

            //create the filewatcher object
            FileSystemWatcher fsWatch = new FileSystemWatcher();
            fsWatch.Path = dirPath; //sets the path to incoming dirPath parameter
            fsWatch.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fsWatch.Filter = "*.*";
            fsWatch.Created += new FileSystemEventHandler((sender, e) => fileCreated(sender, e, dirPath, checkBx.Checked, dirName, countLabel, cfLabel)); //the event handler for file creation -- it calls the fileCreated method which requires the sender, e, the directory path the file was created in, the corresponding isbheckbox array value, the directory name, the count label, and the current files label to be passed
            fsWatch.Deleted += new FileSystemEventHandler((sender, e) => deleted(sender, e, countLabel, dirPath, cfLabel)); //the event handler for deleted files calls the deleted method, which requires the sender, e, the countlabel, the directory path, and the count label
            //fsWatch.Changed += new FileSystemEventHandler((sender, e) => fileChanged(sender, e, dirPath));
            fsWatch.EnableRaisingEvents = true; //allows the object ot raise events

            CheckBox delBx = new CheckBox();
            delBx.AutoSize = true;
            delBx.Checked = false;
            delBx.Location = new System.Drawing.Point(175, chkBoxPosition * posMultiplier);
            delBx.Size = new System.Drawing.Size(15, 14);
            delBx.CheckStateChanged += new EventHandler((sender, e) => deleteBox(sender, e, nameLabel, countLabel, cfLabel, checkBx, delBx, fsWatch));
            this.Controls.Add(delBx);



        }

        public void deleteBox(object source, EventArgs e, Label nameLabel, Label countLabel, Label cfLabel, CheckBox trackBox, CheckBox delBox, FileSystemWatcher fsWatcher)
        {
            nameLabel.Dispose();
            countLabel.Dispose();
            cfLabel.Dispose();
            trackBox.Dispose();
            fsWatcher.Dispose();
            delBox.Dispose();
            //positionDec();
            posMultiplier--;
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
            //call the updateDynLabels method, which will update the dynamic count label and current file(s) label -- it requires the count label, the directory path, and the current file(s) label as parameters
            updateDynLabels(ctLabel, dirPathin, cflabel);
        }

        //the deleted method, this is called when a file is deleted inside a watched folder -- it requires the sender, e, count label, directory path, and the current file(s) label as parameters and all it does is call the updateDynLabels method
        //but is left as a separate method in order to add more options later (like logging)
        public void deleted(object sender, EventArgs e, Label ctLabel, string dirPath, Label cflabel)
        {
            updateDynLabels(ctLabel, dirPath, cflabel);
        }

        //this method will update the 2 labels that require dynamic updates - count label and current file(s) label
        public void updateDynLabels(Label localLabel, string dirPath, Label cflabel)
        {
            //first we set the count label text to the string value of the filecount of the directory -- filecount method requires the current file(s) label, which is why we have passed it along
            localLabel.Text = fileCount(dirPath, cflabel).ToString();
            //here we change the color of the count label based on the setColor method, the setColor method will call filecount, so we need the current file(s) label passed along
            localLabel.ForeColor = setColor(dirPath, cflabel);
        }

        //this is the path Name label on click method, it will open windows file explorer to the path associated with the name
        private void fsLabel_Click(object sender, EventArgs e, string dirPath)
        {
            System.Diagnostics.Process.Start("explorer.exe", dirPath);
        }

        //this method will update the name/link label position variable
        public int labelPosInc()
        {
            labelPosition += 40;
            return labelPosition;
        }

        //this method will update the count label position variable
        public int countLabelPosInc()
        {
            countLabelPosition += 40;
            return countLabelPosition;
        }

        //this method will update the checkbox position variable
        public int checkBoxPosInc()
        {
            chkBoxPosition += 40;
            return chkBoxPosition;         
        }

        //this method will update the current file(s) position variable
        public int cfLabelPosInc()
        {
            cfLabelPosition += 40;
            return cfLabelPosition;
        }

        public void positionDec()
        {
            labelPosition -= 40;
            countLabelPosition -= 40;
            chkBoxPosition -= 40;
            cfLabelPosition -= 40;
        }

        //fileCount method will get the file count of the path, minus any *.db files -- it will also update the current file(s) lable to display singular or plural
        public int fileCount(string countPath, Label cfLabel)
        {
            var allfiles = Directory.GetFiles(countPath, "*", SearchOption.TopDirectoryOnly).Where(name => !name.EndsWith(".db"));
            int count = allfiles.Count();
            if (count == 1)
            {
                cfLabel.Text = "Current File"; 
            }
            else
            {               
                cfLabel.Text = "Current Files";               
            }
            return count;
        }








        //this is the method to both bring the program to the front and open an explorer window to the path that generated the alert
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Activate();           
            System.Diagnostics.Process.Start(balloonDir);          
        }

 
        //this method  will set the color of the count label based on the number of files in the path
        public Color setColor(string dirPathin, Label lbl)
        {
            if (fileCount(dirPathin, lbl) == 0)
            {return Color.Green;}
            else if (fileCount(dirPathin, lbl) > 0 && fileCount(dirPathin, lbl) < 5)
            {return Color.DarkOrange;}
            else if (fileCount(dirPathin, lbl) >= 5)
            {return Color.Red;}
            else
            {return Color.HotPink;}
        }

        private void addPathBtn_Click(object sender, EventArgs e)
        {
            //pop up window to request Name and Path
            Prompt prompt = new Prompt();
            prompt.ShowDialog();


            fileWatcher[] fileWatcherArray = new fileWatcher[20];
            fileWatcherArray[watcherIterator] = new fileWatcher(prompt.nameIn, prompt.pathIn);
            this.Controls.Add(fileWatcherArray[watcherIterator].nameLabel);
            watcherIterator++;
            /*
            watcher(prompt.pathIn, prompt.nameIn, watcherIterator);
            watcherIterator++;
            posMultiplier++;
            */
            
        }
    }

    public class fileWatcher
    {
        public Label nameLabel = new Label();
        public Label countLabel = new Label();
        public Label cfLabel = new Label();
        public CheckBox watchBox = new CheckBox();
        public Button deleteBtn = new Button();
        public FileSystemWatcher fsWatcher = new FileSystemWatcher();

        public string dirName;
        public string dirPath;

        public fileWatcher(string dirNameIn, string dirPathIn)
        {
            dirName = dirNameIn;
            dirPath = dirPathIn;

            nameLabel.Text = dirName;

            
            nameLabel.AutoSize = true;
            nameLabel.BackColor = Color.Black;
            nameLabel.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, GraphicsUnit.Point, ((byte)(0)));
            nameLabel.ForeColor = Color.CornflowerBlue;
            nameLabel.Location = new System.Drawing.Point(35, 40);

            //nameLabel.Click += new EventHandler((sender, e) => this.fsLabel_Click(sender, e, dirPath));

        }



        public int fileCount(string dirPathin)
        {
            var allfiles = Directory.GetFiles(dirPathin, "*", SearchOption.TopDirectoryOnly).Where(name => !name.EndsWith(".db"));
            int count = allfiles.Count();
            return count;
        }

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



    }

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
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 170, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            Label pathLabel = new Label() { Left = 50, Top = 80, Text = "Enter Directory Path" };
            TextBox dirBox = new TextBox() { Left = 50, Top = 110, Width = 400 };

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
