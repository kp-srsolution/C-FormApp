using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ChannelApp
{
    public partial class Form1 : Form
    {
        // Five channel lists
        private List<string> Channel1 = new List<string>();
        private List<string> Channel2 = new List<string>();
        private List<string> Channel3 = new List<string>();
        private List<string> Channel4 = new List<string>();
        private List<string> Channel5 = new List<string>();

        // TextBoxes for input
        private TextBox txtCh1;
        private TextBox txtCh2;
        private TextBox txtCh3;
        private TextBox txtCh4;
        private TextBox txtCh5;

        // Buttons
        private Button btnProcess;
        private Button btnDeleteTop10;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Channel App";
            this.Width = 500;
            this.Height = 450;

            Label lbl1 = new Label() { Text = "Channel 1:", Left = 20, Top = 20, Width = 100 };
            Label lbl2 = new Label() { Text = "Channel 2:", Left = 20, Top = 60, Width = 100 };
            Label lbl3 = new Label() { Text = "Channel 3:", Left = 20, Top = 100, Width = 100 };
            Label lbl4 = new Label() { Text = "Channel 4:", Left = 20, Top = 140, Width = 100 };
            Label lbl5 = new Label() { Text = "Channel 5:", Left = 20, Top = 180, Width = 100 };

            txtCh1 = new TextBox() { Left = 130, Top = 20, Width = 300 };
            txtCh2 = new TextBox() { Left = 130, Top = 60, Width = 300 };
            txtCh3 = new TextBox() { Left = 130, Top = 100, Width = 300 };
            txtCh4 = new TextBox() { Left = 130, Top = 140, Width = 300 };
            txtCh5 = new TextBox() { Left = 130, Top = 180, Width = 300 };

            btnProcess = new Button() { Text = "Process", Left = 180, Top = 230, Width = 100 };
            btnProcess.Click += BtnProcess_Click;

            btnDeleteTop10 = new Button() { Text = "Delete Top 10", Left = 300, Top = 230, Width = 120 };
            btnDeleteTop10.Click += BtnDeleteTop10_Click;

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
        }

        private void BtnProcess_Click(object sender, EventArgs e)
        {
            Method(txtCh1.Text, txtCh2.Text, txtCh3.Text, txtCh4.Text, txtCh5.Text);

            // Clear inputs
            txtCh1.Clear();
            txtCh2.Clear();
            txtCh3.Clear();
            txtCh4.Clear();
            txtCh5.Clear();
        }

        // NEW: Button click for delete top 10
        private void BtnDeleteTop10_Click(object sender, EventArgs e)
        {
            // Simple InputBox replacement (choose channel 1–5)
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

        // Main method that processes all channels
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

                int threshold = 5; // change to 98 in real app

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
            MessageBox.Show($"{channelName} saved to {fileName}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
