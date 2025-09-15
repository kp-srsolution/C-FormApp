using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChannelApp
{
    public partial class Form1 : Form
    {
        // Channels
        private List<string> Channel1 = new List<string>();
        private List<string> Channel2 = new List<string>();
        private List<string> Channel3 = new List<string>();
        private List<string> Channel4 = new List<string>();
        private List<string> Channel5 = new List<string>();

        // UI controls
        private TextBox txtCh1;
        private TextBox txtCh2;
        private TextBox txtCh3;
        private TextBox txtCh4;
        private TextBox txtCh5;
        private Button btnProcess;
        private Button btnDeleteTop10;
        private Button btnStartAuto;
        private Button btnStopAuto;
        private ListBox lstLog;

        // Automation timer
        private System.Windows.Forms.Timer autoTimer;

        private Random rng = new Random();

        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Channel App";
            this.Width = 1280;
            this.Height = 720;

            Label lbl1 = new Label() { Text = "Channel 1:", Left = 20, Top = 20, Width = 100 };
            Label lbl2 = new Label() { Text = "Channel 2:", Left = 20, Top = 60, Width = 100 };
            Label lbl3 = new Label() { Text = "Channel 3:", Left = 20, Top = 100, Width = 100 };
            Label lbl4 = new Label() { Text = "Channel 4:", Left = 20, Top = 140, Width = 100 };
            Label lbl5 = new Label() { Text = "Channel 5:", Left = 20, Top = 180, Width = 100 };

            txtCh1 = new TextBox() { Left = 130, Top = 20, Width = 500 };
            txtCh2 = new TextBox() { Left = 130, Top = 60, Width = 500 };
            txtCh3 = new TextBox() { Left = 130, Top = 100, Width = 500 };
            txtCh4 = new TextBox() { Left = 130, Top = 140, Width = 500 };
            txtCh5 = new TextBox() { Left = 130, Top = 180, Width = 500 };

            btnProcess = new Button() { Text = "Process", Left = 130, Top = 230, Width = 100 };
            btnProcess.Click += BtnProcess_Click;

            btnDeleteTop10 = new Button() { Text = "Delete Top 10", Left = 250, Top = 230, Width = 120 };
            btnDeleteTop10.Click += BtnDeleteTop10_Click;

            btnStartAuto = new Button() { Text = "Start Auto", Left = 390, Top = 230, Width = 100 };
            btnStartAuto.Click += BtnStartAuto_Click;

            btnStopAuto = new Button() { Text = "Stop Auto", Left = 510, Top = 230, Width = 100 };
            btnStopAuto.Click += BtnStopAuto_Click;

            lstLog = new ListBox() { Left = 20, Top = 280, Width = 1200, Height = 200 };

            this.Controls.Add(lbl1);
            this.Controls.Add(lbl2);
            this.Controls.Add(lbl3);
            this.Controls.Add(lbl4);
            this.Controls.Add(lbl5);

            this.Controls.Add(txtCh1);
            this.Controls.Add(txtCh2);
            this.Controls.Add(txtCh3);
            this.Controls.Add(txtCh4);
            this.Controls.Add(txtCh5);

            this.Controls.Add(btnProcess);
            this.Controls.Add(btnDeleteTop10);
            this.Controls.Add(btnStartAuto);
            this.Controls.Add(btnStopAuto);
            this.Controls.Add(lstLog);

            // Setup automation timer
            autoTimer = new System.Windows.Forms.Timer();
            autoTimer.Interval = 3000; // every 3 sec
            autoTimer.Tick += AutoTimer_Tick;
        }

        private void BtnProcess_Click(object sender, EventArgs e)
        {
            Method(txtCh1.Text, txtCh2.Text, txtCh3.Text, txtCh4.Text, txtCh5.Text);

            txtCh1.Clear();
            txtCh2.Clear();
            txtCh3.Clear();
            txtCh4.Clear();
            txtCh5.Clear();
        }

        // Start automation
        private void BtnStartAuto_Click(object sender, EventArgs e)
        {
            autoTimer.Start();
            lstLog.Items.Add("Automation started...");
        }

        // Stop automation
        private void BtnStopAuto_Click(object sender, EventArgs e)
        {
            autoTimer.Stop();
            lstLog.Items.Add("Automation stopped.");
        }

        // Automation tick
        private void AutoTimer_Tick(object sender, EventArgs e)
        {
            // Generate random strings for each channel
            string ch1 = GenerateRandomStrings();
            string ch2 = GenerateRandomStrings();
            string ch3 = GenerateRandomStrings();
            string ch4 = GenerateRandomStrings();
            string ch5 = GenerateRandomStrings();

            Method(ch1, ch2, ch3, ch4, ch5);

            lstLog.Items.Add($"Auto -> Ch1:[{ch1}] Ch2:[{ch2}] Ch3:[{ch3}] Ch4:[{ch4}] Ch5:[{ch5}]");
        }

        // Generate between 0 and 22 random strings
        private string GenerateRandomStrings()
        {
            int count = rng.Next(0, 23); // 0 to 22
            List<string> values = new List<string>();
            for (int i = 0; i < count; i++)
            {
                values.Add(RandomString(4)); // each string 4 chars
            }
            return string.Join(",", values);
        }

        // Helper random string
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rng.Next(s.Length)]).ToArray());
        }

        // Delete Top 10
        private void BtnDeleteTop10_Click(object sender, EventArgs e)
        {
            string channelInput = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter channel number (1-5) to delete top 10 items:",
                "Delete Top 10", "1");

            if (int.TryParse(channelInput, out int channelNum) && channelNum >= 1 && channelNum <= 5)
            {
                List<string> selectedChannel = GetChannelByNumber(channelNum);
                if (selectedChannel.Count == 0)
                {
                    MessageBox.Show($"Channel {channelNum} is empty!", "Info");
                    return;
                }

                int deleteCount = Math.Min(10, selectedChannel.Count);
                var removed = selectedChannel.Take(deleteCount).ToList();

                selectedChannel.RemoveRange(0, deleteCount);

                MessageBox.Show(
                    $"Removed {deleteCount} items from Channel {channelNum}:\n{string.Join(", ", removed)}",
                    "Delete Info");
            }
            else
            {
                MessageBox.Show("Invalid channel number!", "Error");
            }
        }

        private List<string> GetChannelByNumber(int num)
        {
            return num switch
            {
                1 => Channel1,
                2 => Channel2,
                3 => Channel3,
                4 => Channel4,
                5 => Channel5,
                _ => throw new ArgumentException("Invalid channel number")
            };
        }

        // Process method
        public void Method(string ch1str, string ch2str, string ch3str, string ch4str, string ch5str)
        {
            ProcessChannel(ch1str, Channel1, "Channel1");
            ProcessChannel(ch2str, Channel2, "Channel2");
            ProcessChannel(ch3str, Channel3, "Channel3");
            ProcessChannel(ch4str, Channel4, "Channel4");
            ProcessChannel(ch5str, Channel5, "Channel5");
        }

        private void ProcessChannel(string input, List<string> channel, string channelName)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                var items = input.Split(',')
                                 .Select(s => s.Trim())
                                 .Where(s => !string.IsNullOrEmpty(s));

                channel.AddRange(items);

                int threshold = 98; // change to 98 in real app

                while (channel.Count >= threshold)
                {
                    var toSave = channel.Take(threshold).ToList();
                    SaveChannelToFile(toSave, channelName);

                    channel.RemoveRange(0, threshold);
                }
            }
        }

        private void SaveChannelToFile(List<string> items, string channelName)
        {
            string fileName = $"{channelName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            File.WriteAllLines(fileName, items);
            lstLog.Items.Add($"{channelName} saved to {fileName} with {items.Count} items.");
        }
    }
}