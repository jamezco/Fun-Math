using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Net;
using static Google.Apis.Requests.BatchRequest;

namespace Fun_Math
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        private int correctAnswers = 0;
        private int correctAnswer;
        private System.Windows.Forms.Timer timer;
        private int seconds;

        private FirebaseClient firebase;

        IFirebaseConfig config = new FirebaseConfig
        {

            //You can find it in Service Account.
            AuthSecret = "Auth Secret here",
            //this one is on the top of the  database.
            BasePath = "Firebase URL here"

        };
        //Firebase Client
        IFirebaseClient client;

        public Form1()
        {
            client = new FireSharp.FirebaseClient(config);
            InitializeComponent();
            typeCombo.Items.Add("Addition");
            typeCombo.Items.Add("Subtract");
            typeCombo.Items.Add("Multiplication");
            diffCombo.Items.Add("Easy");
            diffCombo.Items.Add("Medium");
            diffCombo.Items.Add("Hard");
            diffCombo.SelectedIndex = 0;
            typeCombo.SelectedIndex = 0;
            if (typeCombo.SelectedIndex == 0 && diffCombo.SelectedIndex == 0)
            {
                GenerateProblemAddEasy();
            }
            this.AcceptButton = button1;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();


            FirebaseResponse response = client.Get("/Base/Scores/");
            var mathData = response.ResultAs<Dictionary<string, submit>>();

            var mathList = mathData.Where(x => x.Key != "001").Select(x => x.Value).ToList();

            var data = mathList
                .Select(x => new { x.Date, x.Time, x.Type, x.Difficulty, x.Score })
                .ToList();

            dataGridView1.DataSource = data;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;



        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            seconds++;
            label8.Text = TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int userAnswer;
            try
            {
                userAnswer = int.Parse(textBox1.Text);
            }
            catch (FormatException)
            {
                label2.Text = "Please enter a valid number";
                return;
            }

            if (userAnswer == correctAnswer)
            {
                correctAnswers++;
                label2.Text = "Correct! You have " + correctAnswers + " correct answers.";
                GenerateProblem();
            }
            else
            {
                //correctAnswers -= 2;
                correctAnswers--;
                label2.Text = "Incorrect. Try again. Current score " + correctAnswers;
            }
            textBox1.Text = "";
            label4.Text = " " + correctAnswers;
        }
        private void GenerateProblem()
        {
            if (typeCombo.SelectedIndex == 0 && diffCombo.SelectedIndex == 0)
            {
                GenerateProblemAddEasy();
            }
            if (typeCombo.SelectedIndex == 0 && diffCombo.SelectedIndex == 1)
            {
                GenerateProblemAddMedium();
            }
            if (typeCombo.SelectedIndex == 0 && diffCombo.SelectedIndex == 2)
            {
                GenerateProblemAddHard();
            }
            if (typeCombo.SelectedIndex == 1 && diffCombo.SelectedIndex == 0)
            {
                GenerateProblemSubtractEasy();
            }
            if (typeCombo.SelectedIndex == 1 && diffCombo.SelectedIndex == 1)
            {
                GenerateProblemSubtractMedium();
            }
            if (typeCombo.SelectedIndex == 1 && diffCombo.SelectedIndex == 2)
            {
                GenerateProblemSubtractHard();
            }
            if (typeCombo.SelectedIndex == 2 && diffCombo.SelectedIndex == 0)
            {
                GenerateProblemMultiEasy();
            }
        }
        private void GenerateProblemAddEasy()
        {
            int num1 = random.Next(100, 1000);
            int num2 = random.Next(100, 1000);
            correctAnswer = num1 + num2;
            label1.Text = num1 + " + " + num2;
        }
        private void GenerateProblemAddMedium()
        {
            int num1 = random.Next(1000, 10000);
            int num2 = random.Next(1000, 10000);
            correctAnswer = num1 + num2;
            label1.Text = num1 + " + " + num2;
        }
        private void GenerateProblemAddHard()
        {
            int num1 = random.Next(1000, 100000);
            int num2 = random.Next(1000, 100000);
            correctAnswer = num1 + num2;
            label1.Text = num1 + " + " + num2;
        }
        private void GenerateProblemSubtractEasy()
        {
            int num1 = random.Next(1000, 10000);
            int num2 = random.Next(100, Math.Min(num1, 999));
            correctAnswer = num1 - num2;
            label1.Text = num1 + " - " + num2;
        }
        private void GenerateProblemSubtractMedium()
        {
            int num1 = random.Next(10000, 100000);
            int num2 = random.Next(1000, Math.Min(num1, 9999));
            correctAnswer = num1 - num2;
            label1.Text = num1 + " - " + num2;
        }
        private void GenerateProblemSubtractHard()
        {
            int num1 = random.Next(100000, 1000000);
            int num2 = random.Next(10000, Math.Min(num1, 99999));
            correctAnswer = num1 - num2;
            label1.Text = num1 + " - " + num2;
        }
        private void GenerateProblemMultiEasy()
        {
            int num1 = random.Next(1, 10);
            int num2 = random.Next(1, 10);
            correctAnswer = num1 * num2;
            label1.Text = num1 + " * " + num2;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                button1_Click(sender, e);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            if (client == null)
            {
                MessageBox.Show("Client error: client object is null.");
                return;
            }

           

            FirebaseResponse responseCount = client.Get("Base/DBCount");
            int DBCount = responseCount.ResultAs<int>();
            DBCount++;
            FirebaseResponse responseCountUpdate = client.Set("Base/DBCount", DBCount);
            DateTime now = DateTime.Now;


            var submit = new submit
            {
                Type = typeCombo.SelectedItem.ToString(),
                Difficulty = diffCombo.SelectedItem.ToString(),
                Score = label4.Text,
                Time = label8.Text,
                count = DBCount,
                Date = now.ToString("dd-MM-yyyy hh:mm tt")

        };

            var response2 = client.Set("Base/Scores/" + DBCount, submit);

            FirebaseResponse response = client.Get("/Base/Scores/");
            var mathData = response.ResultAs<Dictionary<string, submit>>();

            var mathList = mathData.Where(x => x.Key != "001").Select(x => x.Value).ToList();

            var data = mathList
                .Select(x => new { x.Date, x.Time, x.Type, x.Difficulty, x.Score })
                .ToList();

            dataGridView1.DataSource = data;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }






    }
}
