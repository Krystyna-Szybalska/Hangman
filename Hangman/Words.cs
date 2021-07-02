using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class Words
    {
        readonly private List<string> capitals = new();
        readonly private List<string> countries = new();
        public List<string> GetListOfCapitals()
        {
            return capitals;
        }
        public List<string> GetListOfCountries()
        {
            return countries;
        }
        public int ListIndex { get; set; }

        public Words()
        {
            string[] countriesAndCapitals = File.ReadAllLines("Assets/countries_and_capitals.txt");
            foreach (var item in countriesAndCapitals)
            {
                string[] countryAndCapital = item.Split(" | ");
                countries.Add(countryAndCapital[0].Trim());
                capitals.Add(countryAndCapital[1].Trim());
            }
        }

        public string SelectRandomWord(List<string> list)
        {
            var random = new Random();
            int index = random.Next(list.Count);
            string word = list[index];
            ListIndex = index;
            return word;
        }
    }
}
