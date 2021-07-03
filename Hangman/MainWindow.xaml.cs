using AdonisUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
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
        public bool LetterGuessed
        {
            get
            {
                if (currentCapitalLetters.Contains(Convert.ToChar(typeYourGuessTextbox.Text.ToUpper()))) return true;
                else return false;
            }
        }
        public bool WordGuessed
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
        private int _guessingTries = 0;
        private static Stopwatch _stopwatch = new();
        private long _guessingTime;
        private string _guessedWord;
        private string _highScoreString;
        public string HighScoreString
        {
            get
            {
                return _highScoreString;
            }
            set
            {
                _highScoreString = " | " + DateTime.Today.ToString() + " | " + _guessingTime.ToString() + " | " + _guessingTries + " | " + _guessedWord;
            }
        } 

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            StartNewGame();
        }
        public void StartNewGame()
        {
            _guessingTries = 0;
            hintTextBlock.Visibility = Visibility.Collapsed;
            wrongGuessesTextBlock.Visibility = Visibility.Collapsed;
            SelectNewCapitalToGuess();
            CreateGuessingArea();
            RefreshLifePoints();
            _stopwatch.Restart();
        }
        private void SelectNewCapitalToGuess()
        {
            Words words = new();
            currentCapital = words.SelectRandomWord(words.GetListOfCapitals());
            hintTextBlock.Text = string.Format("Hint: It's the capital of {0}", words.GetListOfCountries()[words.ListIndex]);
            wrongGuesses.Clear();
            wrongGuessesList.Text = "";
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
                        TextAlignment = TextAlignment.Justify,
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
                        TextAlignment = TextAlignment.Justify,
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
                _stopwatch.Stop();
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

                else if (LetterGuessed)
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
                        _guessingTries++;
                    }
                    
                    typeYourGuessTextbox.Text = null;

                    if (TheWordIsGuessed())
                    {
                        _stopwatch.Stop();
                        _guessingTime = _stopwatch.ElapsedMilliseconds / 1000;
                        _guessedWord = currentCapital;

                        ShowWinningMessageBox();
                    }
                }

                else
                {
                    wrongGuesses.Add(typeYourGuessTextbox.Text.ToUpper());
                    wrongGuessesTextBlock.Visibility = Visibility.Visible;
                    wrongGuessesList.Text += typeYourGuessTextbox.Text.ToUpper() + ' ';
                    typeYourGuessTextbox.Text = null;
                    _guessingTries++;
                    LoseLifePoint();
                }
            }
        }
        private void CheckTypedWord()
        {
            if ((typeYourGuessTextbox.Text).Length != currentCapital.Length)
            {
                AdonisUI.Controls.MessageBox.Show(string.Format("Enter a {0}-letter word", currentCapital.Length), "Info", AdonisUI.Controls.MessageBoxButton.OK);
            }

            else
            {
                if (wrongGuesses.Contains(typeYourGuessTextbox.Text.ToUpper()))
                {
                    AdonisUI.Controls.MessageBox.Show("Don't make the same mistake twice!", "Really", AdonisUI.Controls.MessageBoxButton.OK);
                    typeYourGuessTextbox.Text = null;
                }

                else if (WordGuessed)
                {
                    _guessingTries++;
                    _stopwatch.Stop();
                    _guessingTime = _stopwatch.ElapsedMilliseconds / 1000;
                    _guessedWord = currentCapital;

                    for (int i = 0; i < GuessingArea.Children.Count; i++)
                    {
                        TextBlock letterTextBlock = GuessingArea.Children[i] as TextBlock;
                        letterTextBlock.Text = string.Format("{0}", currentCapitalLetters[i]);
                    }
                    typeYourGuessTextbox.Text = null;

                    ShowWinningMessageBox();
                }

                else
                {
                    typeYourGuessTextbox.Text = null;
                    wrongGuesses.Add(typeYourGuessTextbox.Text.ToUpper());
                    wrongGuessesTextBlock.Visibility = Visibility.Visible;
                    wrongGuessesList.Text += typeYourGuessTextbox.Text.ToUpper() + ' ';
                    _guessingTries++;
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
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (typeYourGuessTextbox.Text.Length == 1) CheckTypedLetter();
                else if (typeYourGuessTextbox.Text.Length == currentCapital.Length) CheckTypedWord();
                else
                {
                    AdonisUI.Controls.MessageBox.Show(string.Format("Enter one letter of the English alphabet or a {0}-letter word", currentCapital.Length), "Info", AdonisUI.Controls.MessageBoxButton.OK);
                }
            }
        }
        private void ShowWinningMessageBox()
        {

            AdonisUI.Controls.MessageBoxResult result1 = AdonisUI.Controls.MessageBox.Show("You've won! Do you want to save your score?", "Congratulations!", AdonisUI.Controls.MessageBoxButton.YesNo);
            if (result1 == AdonisUI.Controls.MessageBoxResult.Yes)
            {
                EnterYourNameWIndow window = new();
                window.Show();
                StartNewGame();
            }
            else if (result1 == AdonisUI.Controls.MessageBoxResult.No)
            {
                AdonisUI.Controls.MessageBoxResult result2 = AdonisUI.Controls.MessageBox.Show("Do you want to start a new game?", "Congratulations!", AdonisUI.Controls.MessageBoxButton.YesNo);
                if (result2 == AdonisUI.Controls.MessageBoxResult.Yes)
                {
                    StartNewGame();
                }
                else if (result2 == AdonisUI.Controls.MessageBoxResult.No)
                {
                    this.Close();
                }
            }
        }
    }
}
