using AdonisUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<string> wrongGuesses = new();
        private readonly List<char> currentCapitalLetters = new();
        private bool LetterGuessedCorrectly
        {
            get
            {
                if (currentCapitalLetters.Contains(Convert.ToChar(typeYourGuessTextbox.Text.ToUpper()))) return true;
                else return false;
            }
        }
        private bool WordGuessedCorrectly
        {
            get
            {
                if (currentCapital.ToUpper() == typeYourGuessTextbox.Text.ToUpper()) return true;
                else return false;
            }
        }
        private bool TheWordIsGuessed()
        {
            for (int i = 0; i < GuessingArea.Children.Count; i++)
            {
                TextBlock letterTextBlock = GuessingArea.Children[i] as TextBlock;
                if (letterTextBlock.Text == "_")
                {
                    return false;
                }
            }
            return true;
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            StartNewGame();
        }
        private void StartNewGame()
        {
            hintTextBlock.Visibility = Visibility.Collapsed;
            SelectNewCapitalToGuess();
            CreateGuessingArea();
            RefreshLifePoints();
        }
        private void SelectNewCapitalToGuess()
        {
            Words words = new();
            currentCapital = words.SelectRandomWord(words.GetListOfCapitals());
            hintTextBlock.Text = string.Format("Hint: It's the capital of {0}", words.GetListOfCountries()[words.ListIndex]);
            wrongGuesses.Clear();
            wrongGuessesTextBlock.Text = "";
            currentCapitalLetters.Clear();
            currentCapitalLetters.AddRange(currentCapital.ToUpper());
        }
        private void CreateGuessingArea()
        {
            GuessingArea.Children.Clear();
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
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Bottom
                    };

                    GuessingArea.Children.Add(letterLabel);
                    Canvas.SetTop(letterLabel, 0);
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
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Bottom,
                };

                   GuessingArea.Children.Add(letterLabel);
                   Canvas.SetTop(letterLabel, 0);
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
                HP5.Visibility = Visibility.Collapsed;
                AdonisUI.Controls.MessageBoxResult result = AdonisUI.Controls.MessageBox.Show("You've died! Do you want to start a new game?", "Game over!", AdonisUI.Controls.MessageBoxButton.YesNo);
                if (result == AdonisUI.Controls.MessageBoxResult.Yes)
                {
                    StartNewGame();
                }
                else if (result == AdonisUI.Controls.MessageBoxResult.No)
                {
                    this.Close();
                }
            }
        }
        private void CheckTypedLetter()
        {
            if ((typeYourGuessTextbox.Text).Length != 1 || !Char.IsLetter(Char.Parse(typeYourGuessTextbox.Text)))
            {
                AdonisUI.Controls.MessageBox.Show("Enter one letter from English alphabet", "Info", AdonisUI.Controls.MessageBoxButton.OK);
                typeYourGuessTextbox.Text = null;
            }

            else
            {
                if (wrongGuesses.Contains(typeYourGuessTextbox.Text.ToUpper()))
                {
                    AdonisUI.Controls.MessageBox.Show("Don't make the same mistake twice!", "Really?", AdonisUI.Controls.MessageBoxButton.OK);
                    typeYourGuessTextbox.Text = null;
                }

                else if (LetterGuessedCorrectly)
                {
                    List<string> guessedLetters = new();

                    if (guessedLetters.Contains((typeYourGuessTextbox.Text).ToUpper()))
                    {
                        AdonisUI.Controls.MessageBox.Show("You've already guessed that one.", "Info", AdonisUI.Controls.MessageBoxButton.OK);
                    }

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
                        guessedLetters.Add(letterTextBlock.Text.ToUpper());
                    }
                    typeYourGuessTextbox.Text = null;

                    if (TheWordIsGuessed())
                    {
                        AdonisUI.Controls.MessageBoxResult result = AdonisUI.Controls.MessageBox.Show("You've won! Do you want to start a new game?", "Congratulations!", AdonisUI.Controls.MessageBoxButton.YesNo);
                        if (result == AdonisUI.Controls.MessageBoxResult.Yes)
                        {
                            StartNewGame();
                        }
                        else if (result == AdonisUI.Controls.MessageBoxResult.No)
                        {
                            this.Close();
                        }
                    }
                }

                else
                {
                    wrongGuesses.Add(typeYourGuessTextbox.Text.ToUpper());
                    wrongGuessesTextBlock.Text += typeYourGuessTextbox.Text.ToUpper() + ' ';
                    typeYourGuessTextbox.Text = null;
                    LoseLifePoint();
                }
            }
        }
        private void CheckTypedWord()
        {
            if ((typeYourGuessTextbox.Text).Length != currentCapital.Length || !(typeYourGuessTextbox.Text).All(Char.IsLetter))
            {
                AdonisUI.Controls.MessageBox.Show(string.Format("Type in a {0}-letter word", currentCapital.Length), "Info", AdonisUI.Controls.MessageBoxButton.OK);
            }

            else
            {
                if (wrongGuesses.Contains(typeYourGuessTextbox.Text.ToUpper()))
                {
                    AdonisUI.Controls.MessageBox.Show("Don't make the same mistake twice!", "Really", AdonisUI.Controls.MessageBoxButton.OK);
                    typeYourGuessTextbox.Text = null;
                }

                else if (WordGuessedCorrectly)
                {
                    for (int i = 0; i < GuessingArea.Children.Count; i++)
                    {
                        TextBlock letterTextBlock = GuessingArea.Children[i] as TextBlock;
                        letterTextBlock.Text = string.Format("{0}", currentCapitalLetters[i]);
                    }
                    typeYourGuessTextbox.Text = null;

                    AdonisUI.Controls.MessageBoxResult result = AdonisUI.Controls.MessageBox.Show("You've won! Do you want to start a new game?", "Congratulations!", AdonisUI.Controls.MessageBoxButton.YesNo);
                    if (result == AdonisUI.Controls.MessageBoxResult.Yes)
                    {
                        StartNewGame();
                    }
                    else if (result == AdonisUI.Controls.MessageBoxResult.No)
                    {
                        this.Close();
                    }
                }

                else
                {
                    typeYourGuessTextbox.Text = null;
                    wrongGuesses.Add(typeYourGuessTextbox.Text.ToUpper());
                    wrongGuessesTextBlock.Text += typeYourGuessTextbox.Text.ToUpper() + ' ';
                    LoseLifePoint();
                    LoseLifePoint();
                }
            }
        }
        private void GuessLetterButton_Click(object sender, RoutedEventArgs e)
        {
            CheckTypedLetter();
        }
        private void GuessWordButton_Click(object sender, RoutedEventArgs e)
        {
            CheckTypedWord();
        }
    }
}
