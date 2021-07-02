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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hangman
{
    public partial class MainWindow
    {
        private string currentCapital;
        private List<char> currentCapitalLetters = new();
        private List<string> wrongGuesses = new();
        private bool LetterGuessed
        {
            get
            {
                if (currentCapitalLetters.Contains(Convert.ToChar(typeYourGuessTextbox.Text.ToUpper()))) return true;
                else return false;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            SelectNewCapitalToGuess();
            CreateGuessingArea();
            RefreshLifePoints();
        }
        private void SelectNewCapitalToGuess()
        {
            Words words = new();
            currentCapital = words.SelectRandomWord(words.GetListOfCapitals());
            hintTextBlock.Text = string.Format("Hint: It's the capital of {0}", words.GetListOfCountries()[words.listIndex]);
            wrongGuesses.Clear();
            currentCapitalLetters.Clear();
            currentCapitalLetters.AddRange(currentCapital.ToUpper());
        }
        private void CreateGuessingArea()
        {
           int letterX = 400 / (currentCapital.Length+1); 

            for (int i = 0; i < currentCapital.Length; i++)
            {
                if (currentCapital[i] == ' ')
                {
                    TextBlock letterLabel = new()
                    {
                        MinWidth = 20,
                        Height = 150,
                        Text = " ",
                        FontSize = 75,
                        TextAlignment = TextAlignment.Center
                    };

                    GuessingArea.Children.Add(letterLabel);
                    Canvas.SetBottom(letterLabel, 0);
                    Canvas.SetLeft(letterLabel, letterX);
                    letterX += 800 / (currentCapital.Length + 1);
                }

                else
                {
                    TextBlock letterLabel = new()
                    {
                        MinWidth = 20,
                        Height = 150,
                        FontSize = 75,
                        Text = "_",
                        TextAlignment = TextAlignment.Center
                };

                    GuessingArea.Children.Add(letterLabel);
                   Canvas.SetBottom(letterLabel, 0);
                   Canvas.SetLeft(letterLabel, letterX);
                   letterX += 800 / (currentCapital.Length + 1);
                }
            }
        }
        private void RefreshLifePoints()
        {
            HP1.Visibility = Visibility.Visible;
            HP2.Visibility = Visibility.Visible;
            HP3.Visibility = Visibility.Visible;
            HP4.Visibility = Visibility.Visible;
            HP5.Visibility = Visibility.Visible;
        }
        private void LoseLifePoint()
        {
            if (HP1.Visibility == Visibility.Visible)
            {
                HP1.Visibility = Visibility.Collapsed;
            }

            else if (HP2.Visibility == Visibility.Visible)
            {
                HP2.Visibility = Visibility.Collapsed;
            }

            else if (HP3.Visibility == Visibility.Visible)
            {
                HP3.Visibility = Visibility.Collapsed;
            }

            else if (HP4.Visibility == Visibility.Visible)
            {
                HP4.Visibility = Visibility.Collapsed;
                hintTextBlock.Visibility = Visibility.Visible;
            }

            else
            {
                /*show messagebox you've died, would you like to restart the game?
                  currentCapitalLetters.Clear();
                  wrongGuesses.Clear(); */
            }
        }
        private void CheckTypedLetter()
        {
            if ((typeYourGuessTextbox.Text).Length != 1 || !Char.IsLetter(Char.Parse(typeYourGuessTextbox.Text)))
            {
                MessageBox.Show("Enter one letter from English alphabet");
                typeYourGuessTextbox.Text = null;
            }

            else
            {
                if (wrongGuesses.Contains(typeYourGuessTextbox.Text.ToUpper()))
                {
                    MessageBox.Show("Don't make the same mistake twice");
                    typeYourGuessTextbox.Text = null;
                }

                else if (LetterGuessed)
                {
                    List<int> indexList = new();
                    for (int i = 0; i < currentCapital.Length; i++)
                    {
                        if (currentCapitalLetters[i] == Char.Parse((typeYourGuessTextbox.Text).ToUpper()))
                        {
                            indexList.Add(i);
                        }
                    }

                    for (int i = 0; i < indexList.Count; i++)
                    {
                        TextBlock letterTextBlock = GuessingArea.Children[indexList[i]] as TextBlock;
                        letterTextBlock.Text = string.Format("{0}", typeYourGuessTextbox.Text.ToUpper());
                    }
                }

                else
                {
                    wrongGuesses.Add(typeYourGuessTextbox.Text.ToUpper());
                    LoseLifePoint();
                }
            }
        }
        private void GuessLetterButton_Click(object sender, RoutedEventArgs e)
        {
            CheckTypedLetter();
        }
    }
}
