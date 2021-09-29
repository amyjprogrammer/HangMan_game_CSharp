using HangMan_game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HangMan_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            GreetUser();
            Thread.Sleep(1000);
            Console.Clear();

            //newing up Game
            var createGame = new Game();

            //create random word for the game
            createGame.CorrectWord = RandomWord();
            EmptyHangMan();

            Console.WriteLine(createGame.CorrectWord);
            Console.Write("The word is: ");
            DisplayDashesForCorrectWord();

            PrintColorMessage(ConsoleColor.Green, "\n\nPlease enter your guess: ");

            //making sure user entered a single letter
            bool checkUserAnswer = true;
            while (checkUserAnswer)
            {
                string checkForSingleChar = VerifyUserGaveSingleChar();
                bool verifyGuessForLetter = VerifyUserGaveLetter(checkForSingleChar);
                if (verifyGuessForLetter == true)
                {
                    checkUserAnswer = false;
                }
                else
                {
                    PrintColorMessage(ConsoleColor.Red, "\nThis is not a single letter!\n");
                    PrintColorMessage(ConsoleColor.Green, "Enter your guess again: ");
                }
                createGame.UserGuess = checkForSingleChar;
            }
            Console.WriteLine();
            Console.WriteLine();
            FullHangMan();

            Console.ReadKey();
        }
        //helper method to change the color of the text
        static void PrintColorMessage(ConsoleColor color, string message)
        {
            //Change text color
            Console.ForegroundColor = color;
            //text
            Console.Write(message);
            //reset color
            Console.ResetColor();
        }
        static void GreetUser()
        {
            Console.SetCursorPosition((Console.WindowWidth) / 2, Console.WindowHeight / 2);
            Console.Write("What is your name: ");
            string userInputName = Console.ReadLine();
            Console.Clear();
            PrintColorMessage(ConsoleColor.Blue, String.Format("{0," + ((Console.WindowWidth / 2) + (userInputName.Length / 2)) + "}", $"Welcome {userInputName}, let's play a game."));
        }
        static string VerifyUserGaveSingleChar()
        {
            bool checkForSingleLetter = true;
            while (checkForSingleLetter)
            {
                string checkLetter = Console.ReadLine();
                if(checkLetter.Length != 1)
                {
                    PrintColorMessage(ConsoleColor.Red, "\n\nPlease enter a single Letter!\n");
                    PrintColorMessage(ConsoleColor.Green, "Enter your guess again: ");
                    continue;
                }
                else
                {
                    return checkLetter;
                }
            }
            return "0";
        }
        static bool VerifyUserGaveLetter(string letter)
        {
            var isValid = letter.All(c => char.IsLetter(c));
            if (isValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static string RandomWord()
        {
            string[] correctWordsList = {"Halloween", "Hamster", "Program", "Riddle", "Answer", "Computer", "Daughter", "Frequent", "Language", "Maximize", "Multiple", "Official", "Original", "Possible", "Princess", "Unicorn", "Parallel", "Remember", "Research", "Scenario", "Accessory", "Balloon",  "Calculate", "Dwelling", "Eccentric" };

            Random randomNum = new Random();
            string randomWordForGame = correctWordsList[randomNum.Next(0, 24)];
            return randomWordForGame;
        }
        static void DisplayDashesForCorrectWord()
        {
            string wordUsedInGame = RandomWord();
            for (int i = 1; i < wordUsedInGame.Length + 1; i++)
            {
                Console.Write("_ ");
            }
        }
        static void FullHangMan()
        {
            Console.WriteLine(" ________________ ");
            Console.WriteLine(" |/   |");
            Console.WriteLine(" |    | ");
            Console.WriteLine(" |   ( ) ");
            Console.WriteLine(" |   /|\\ ");
            Console.WriteLine(" |    | ");
            Console.WriteLine(" |   /|\\ ");
            Console.WriteLine(" | ");
            Console.WriteLine(" | ");
            Console.WriteLine(" |_______ ");
        }
        static void EmptyHangMan()
        {
            Console.WriteLine(" ______________ ");
            Console.WriteLine(" |/ ");
            Console.WriteLine(" | ");
            Console.WriteLine(" | ");
            Console.WriteLine(" | ");
            Console.WriteLine(" | ");
            Console.WriteLine(" | ");
            Console.WriteLine(" | ");
            Console.WriteLine(" | ");
            Console.WriteLine(" |_______ \n");
        }
    }
}
