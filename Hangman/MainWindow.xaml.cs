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
        public MainWindow()
        {
            InitializeComponent();
            SelectCapitalToGuess();
            CreateGuessingArea();
            RefreshLifePoints();

        }

        private void SelectCapitalToGuess()
        {
            Words words = new Words();
            currentCapital = words.SelectRandomWord(words.GetListOfCapitals());
        }
        private void CreateGuessingArea()
        {
            DataGrid letterGrid = new DataGrid
            {
                MinWidth = 20,
                Height = 100,
                Name = "letterGrid",
            };

            for (int i = 0; i < currentCapital.Length; i++)
            {
                if (currentCapital[i] == ' ')
                {
                    var col = new DataGridTextColumn
                    {
                        Header = " ",
                        Width = '*',
                    };
                }

                else
                {
                    var col = new DataGridTextColumn
                    {
                        Header = "_",
                        Width = '*',
                    };

                    letterGrid.Columns.Add(col);
                }
            }

            GuessingArea.Children.Add(letterGrid);
            Canvas.SetLeft(letterGrid, 25);
            Canvas.SetTop(letterGrid, 25);
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
                //show messagebox you've died, would you like to restart the game?
            }
        }
    }
}
