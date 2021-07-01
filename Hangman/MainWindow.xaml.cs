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
                if (currentCapital[i-1] == ' ')
                {
                    var col = new DataGridTextColumn
                    {
                        Header = " ",
                        MinWidth = 20,
                    };
                }

                else
                {
                    var col = new DataGridTextColumn
                    {
                        Header = "_",
                        MinWidth = 20,
                    };

                    letterGrid.Columns.Add(col);
                }
            }

            GuessingArea.Children.Add(letterGrid);
            Canvas.SetLeft(letterGrid, 25);
            Canvas.SetTop(letterGrid, 25);
        }
    }
}
