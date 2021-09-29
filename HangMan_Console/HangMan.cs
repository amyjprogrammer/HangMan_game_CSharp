using HangMan_game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HangMan_Console
{
    public class HangMan
    {
        private GameRepo _gameRepo = new GameRepo();

        public void Run()
        {
            RunGame();
        }

        public void RunGame()
        {

            GreetUser();
            Thread.Sleep(2000);
            Console.Clear();

            //newing up Game
            var createGame = new Game();

            //starting player off with 6 lives
            createGame.LivesLeft = 6;

            //create random word for the game
            //Add this back in at the end
            /*createGame.CorrectWord = RandomWord();*/
            createGame.CorrectWord = "Halloween";
            HangManTitle();
            EmptyHangMan();

            Console.WriteLine(createGame.CorrectWord);
            Console.Write("The word is: ");

            //go back to make sure letter shows after the guess
            DisplayDashesForCorrectWord();

            while (createGame.LivesLeft > 0)
            {
                PrintColorMessage(ConsoleColor.Green, "\n\nPlease enter one letter for your guess: ");

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
                _gameRepo.StoringUserInputGame(createGame);

                List<Game> listofletters = _gameRepo.GetDatabaseGameInfo();

                //need to get everything to Upper Case
                //instead of tons of for loops- contains is great!!  https://docs.microsoft.com/en-us/dotnet/api/system.string.contains?view=net-5.0
                if (createGame.CorrectWord.ToUpper().Contains(createGame.UserGuess.ToUpper()))
                {
                    Console.WriteLine("Great Guess!");

                    for (int i = 0; i < createGame.CorrectWord.Length; i++)
                    {
                        //I want this to diplay the letter in the word
                    }
                }
                else
                {
                    Console.WriteLine($"That was not correct. The word did not contain {createGame.UserGuess}.");
                    createGame.LivesLeft--;
                }
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
            PrintColorMessage(ConsoleColor.Yellow, String.Format("{0," + ((Console.WindowWidth / 2) + (userInputName.Length / 2)) + "}", $"Welcome {userInputName}, let's play a game of HangMan."));
        }
        static string VerifyUserGaveSingleChar()
        {
            bool checkForSingleLetter = true;
            while (checkForSingleLetter)
            {
                string checkLetter = Console.ReadLine();
                if (checkLetter.Length != 1)
                {
                    PrintColorMessage(ConsoleColor.Red, "\n\nPlease enter a single Letter.\n");
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

            string[] correctWordsList = { "Halloween", "Hamster", "Program", "Riddle", "Answer", "Computer", "Daughter", "Frequent", "Language", "Maximize", "Multiple", "Official", "Original", "Possible", "Princess", "Unicorn", "Parallel", "Remember", "Research", "Scenario", "Accessory", "Balloon", "Calculate", "Dwelling", "Eccentric" };

            Random randomNum = new Random();
            string randomWordForGame = correctWordsList[randomNum.Next(0, correctWordsList.Length - 1)];
            return randomWordForGame;
        }
        static void DisplayDashesForCorrectWord()
        {
            /*what cool docs and idea.. https://docs.microsoft.com/en-us/dotnet/api/system.text.stringbuilder?view=net-5.0*//* and https://www.tutorialsteacher.com/csharp/csharp-stringbuilder*/
            //Add after getting everthing working
            /*StringBuilder display = new StringBuilder(RandomWord().Length);
            *//*string wordUsedInGame = RandomWord();*/
            string wordUsedInGame = "Halloween";
            StringBuilder display = new StringBuilder(wordUsedInGame.Length);
            for (int i = 0; i < wordUsedInGame.Length; i++)
            {
                display.Append('_');
            }
            Console.Write(display);
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
        static void HangManTitle()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("                         ************************    ");
            Console.WriteLine("                         *       HANGMAN!       *    ");
            Console.WriteLine("                         ************************    \n");
            Console.ResetColor();
        }
    }
}
