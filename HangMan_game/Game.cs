using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangMan_game
{
    public class Game
    {
        //constructor
        public Game() { }
        public Game(string userGuess, string correctWord, int livesLeft)
        {
            UserGuess = userGuess;
            CorrectWord = correctWord;
            LivesLeft = livesLeft;
        }

        public string UserGuess { get; set; }
        public string CorrectWord { get; set; }
        public int LivesLeft { get; set; }
    }
}
