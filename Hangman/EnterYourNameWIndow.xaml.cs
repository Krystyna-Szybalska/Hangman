using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Hangman
{
    public partial class EnterYourNameWIndow
    {
        public string HighScore { get; }

        public EnterYourNameWIndow(string highScore)
        {
            InitializeComponent();
            HighScore = highScore;
        }

        private void SaveToHighScoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (EnterYourNameTextBox.Text.Length < 1 || EnterYourNameTextBox.Text.Length > 20)
            {
                AdonisUI.Controls.MessageBox.Show("Your name should be from 1 to 20 characters long", "Info", AdonisUI.Controls.MessageBoxButton.OK);
            }
            SaveScore();
        }

        private void SaveScore()
        {
            string path = @"./Assets/HighScores.txt";
            string text = EnterYourNameTextBox.Text + HighScore;

            File.AppendAllText(path, text);

            AdonisUI.Controls.MessageBox.Show("Saved", "Info", AdonisUI.Controls.MessageBoxButton.OK);
            this.Close();
        }
    }
}
