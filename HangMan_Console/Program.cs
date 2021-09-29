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

            PrintColorMessage(ConsoleColor.Green, "Please enter your guess: ");

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
                    PrintColorMessage(ConsoleColor.Red, "\n\nThis is not a single letter!\n");
                    PrintColorMessage(ConsoleColor.Green, "Enter your guess again: ");
                }
                createGame.UserGuess = checkForSingleChar;
            }
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
    }
}
