using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using System.Drawing;

namespace The_Rubber_Ducky_Ninja
{
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();
            ApplyModernStyling();
        }

        private void ApplyModernStyling()
        {
            // Modern Color Scheme (Dark theme inspired)
            this.BackColor = Color.FromArgb(32, 32, 32);          // Dark background
            this.ForeColor = Color.FromArgb(220, 220, 220);       // Light text

            // Modern Font
            Font modernFont = new Font("Segoe UI", 9F, FontStyle.Regular);
            Font headerFont = new Font("Segoe UI", 10F, FontStyle.SemiBold);
            
            this.Font = modernFont;

            // Style Labels
            PathLabel.ForeColor = Color.FromArgb(160, 160, 160);
            label2.ForeColor = Color.FromArgb(160, 160, 160);
            label2.Font = new Font("Segoe UI", 8.25F);

            // Style Primary Action Buttons (Accent Blue)
            StylePrimaryButton(btnExecuteButton, Color.FromArgb(0, 120, 215));
            StylePrimaryButton(btnDebug, Color.FromArgb(16, 137, 62));  // Green for validation

            // Style Secondary Buttons
            StyleSecondaryButton(btnPath);
            StyleSecondaryButton(btnEncodeForm);
            StyleSecondaryButton(btnDelay);
            StyleSecondaryButton(btnUAC);

            // Style Exit Button (Red accent)
            StyleDangerButton(btnExit);

            // Style TextBox
            SetDelayTextBox.BackColor = Color.FromArgb(45, 45, 45);
            SetDelayTextBox.ForeColor = Color.White;
            SetDelayTextBox.BorderStyle = BorderStyle.FixedSingle;

            // Style MenuStrip
            menuStrip1.BackColor = Color.FromArgb(40, 40, 40);
            menuStrip1.ForeColor = Color.White;
            
            // Modern Window Settings
            this.FormBorderStyle = FormBorderStyle.FixedSingle;  // Remove 3D border
            this.MaximizeBox = false;  // Cleaner look
        }

        private void StylePrimaryButton(Button btn, Color accentColor)
        {
            btn.BackColor = accentColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(accentColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(accentColor, 0.1f);
            btn.Font = new Font("Segoe UI", 9F, FontStyle.SemiBold);
            btn.Cursor = Cursors.Hand;
            
            // Add some padding
            btn.Size = new Size(btn.Width, 32);
        }

        private void StyleSecondaryButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(60, 60, 60);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 100);
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 80, 80);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, 50, 50);
            btn.Cursor = Cursors.Hand;
            
            btn.Size = new Size(btn.Width, 32);
        }

        private void StyleDangerButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(196, 43, 28);  // Red
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 60, 45);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(170, 35, 20);
            btn.Cursor = Cursors.Hand;
            
            btn.Size = new Size(btn.Width, 32);
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            if (!File.Exists("duckencode.jar"))
            {
                MessageBox.Show("Error finding duckencode.jar. Please keep this file in the same folder as this program. The encoding feature has been disabled.");
                duckEncodeFound = false;
            }

            try
            {
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey subKey = rk.OpenSubKey("HKEY_LOCAL_MACHINE\\Software\\JavaSoft");
                                
            }
            catch (Exception)
            {
                MessageBox.Show("No java was detected. Any features that require it have been disbaled.");
                duckEncodeFound = false;
            }
        }

        //Variables
        private string FilePath = "";
        public string directoryPath = "";
        private bool duckEncodeFound = true;
        private DuckyScriptProcessing DuckyScriptProcessing = new DuckyScriptProcessing();
         
        
        //BUTTONS
        private void btnPath_Click(object sender, EventArgs e)
        {
            FindFile(); //Opens file browser
        }

        private void btnDelay_Click(object sender, EventArgs e) 
        {   //When delay button is pressed, validate it, the set it.
            int DefaultDelay;
            if (int.TryParse(SetDelayTextBox.Text, out DefaultDelay))
            {
                DefaultDelay = Convert.ToInt32(SetDelayTextBox.Text);
                DuckyScriptProcessing.SetDelay(DefaultDelay);
                MessageBox.Show("The delay between each command is now set to " + DefaultDelay + "ms");
            }
            else
            {
                MessageBox.Show("Not a valid number. Please enter non decimal numbers (100, 500, 1000)");
            }
        }

        private void btnEncodeForm_Click(object sender, EventArgs e)
        {
            
            File.Copy(FilePath,"script.txt", true); //copy the script to exe folder
            formEncoding formEncoding = new formEncoding();
            formEncoding.ShowDialog(); //show encoding form
            if (File.Exists("script.txt"))
            {
                File.Delete("script.txt");
            }
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            // Add visual feedback
            btnDebug.Text = "🔄 Validating...";
            btnDebug.Enabled = false;
            Application.DoEvents(); // Allow UI update
            
            if (DuckyScriptProcessing.validateCode(FilePath) == true) //Validate code
            {
                btnExecuteButton.Enabled = true;
                if (duckEncodeFound == true)
                {
                    btnEncodeForm.Enabled = true;
                }
                
                // Success feedback
                btnDebug.Text = "✅ Validation Complete";
                btnDebug.BackColor = Color.FromArgb(16, 137, 62); // Green
                
                // Show modern message box
                ShowModernMessage("✅ Success", "No problems found in code!", MessageBoxIcon.Information);
            }
            else
            {
                // Error feedback
                btnDebug.Text = "❌ Validation Failed";
                btnDebug.BackColor = Color.FromArgb(196, 43, 28); // Red
            }
            
            // Reset button after 2 seconds
            System.Windows.Forms.Timer resetTimer = new System.Windows.Forms.Timer();
            resetTimer.Interval = 2000;
            resetTimer.Tick += (s, args) => {
                btnDebug.Text = "✅ Validate Code";
                btnDebug.Enabled = true;
                ApplyModernStyling(); // Reapply styling
                resetTimer.Stop();
                resetTimer.Dispose();
            };
            resetTimer.Start();
        }

        private void btnExecuteButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(FilePath))
            {
                // Visual feedback
                btnExecuteButton.Text = "⏳ Executing...";
                btnExecuteButton.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                
                Thread.Sleep(500); //gives user a second to take hand off mouse
                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_D);
                Thread.Sleep(500);
                DuckyScriptProcessing.ReadFile(FilePath); //emulate code
                
                // Reset UI
                btnExecuteButton.Text = "▶️ Execute Script";
                btnExecuteButton.Enabled = true;
                this.Cursor = Cursors.Default;
                
                ShowModernMessage("🎯 Execution Complete", "Script execution finished successfully!", MessageBoxIcon.Information);
            } else
            {
                ShowModernMessage("❌ File Not Found", $"Your file {FilePath} can no longer be found. Please load the script again.", MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnUAC_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("UAC causes problems for The Rubber Ducky Ninja." +
                " For security reasons Windows doesn't allow programs to say Yes to UAC like a Ducky can. "+
                "It is highly recommended you disable UAC while testing scripts. Optionally, you can wait for UAC to trigger and say yes yourself quickly."
                + "\nWould you like to open the UAC control panel?", "More UAC, More Problems", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start(@"C:\Windows\System32\UserAccountControlSettings.exe");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                    throw;
                }
            }
            
        }
        //END OF BUTTONS
        //MENU STRIP
        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FindFile();
        }

        private void createSystemRestorePointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("restore.vbs");//Creates system restore point
        }

        private void editToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //Opens current file in notepad
            if (FilePath != "")
            { //Opens file in notepad
                try
                {
                    System.Diagnostics.Process.Start("notepad++.exe", FilePath);
                }
                catch (Exception)
                {
                    try
                    {
                        System.Diagnostics.Process.Start("notepad.exe", FilePath);
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show("Problem opening notepad. Error = " + exx);
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Please select a file before you try to edit it.");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Show();
        }

        //END OF MENU STRIP
        public void moveFile()
        {

        }


        private void FindFile() //Lets user select script file
        {
            Stream myStream = null;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Select DuckyScript File";
            theDialog.Filter = "DuckyScript files (*.txt)|*.txt|All files (*.*)|*.*";
            theDialog.RestoreDirectory = true;
            
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = theDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            FilePath = theDialog.FileName;
                            directoryPath = Path.GetDirectoryName(FilePath);
                            btnDebug.Enabled = true; //enable validate button
                            SetDelayTextBox.Enabled = true; //enable delay txt box
                            btnDelay.Enabled = true; //enable delay button 
                            PathLabel.Text = "DuckyScript Loaded!"; //display path
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}