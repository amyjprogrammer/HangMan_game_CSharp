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
            createGame.CorrectWord = RandomWord();
            //createGame.CorrectWord = "Halloween"; //starter word to make sure code worked
            Console.WriteLine(createGame.CorrectWord);

            //variable for the while loop
            bool notWon = true;

            StringBuilder correctWordDisplay = new StringBuilder(createGame.CorrectWord.Length);
            for (int i = 0; i < createGame.CorrectWord.Length; i++)
            {
                correctWordDisplay.Append('_');
            }

            int lettersShownToPlayer = 0;

            List<Game> listAllLetters = _gameRepo.GetDatabaseGameInfo();

            while (createGame.LivesLeft > 0 && notWon)
            {
                Console.Clear();
                HangManTitle();
                StartingToHang(createGame.LivesLeft);
                Console.Write("The word is: ");
                Console.WriteLine(correctWordDisplay);
                Console.Write("\nGuessed Letters: ");

                Game createNewLetter = new Game();

                foreach (var letter in listAllLetters)
                {
                    Console.Write(letter.UserGuess);
                    Console.Write(" ");
                }

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
                    createNewLetter.UserGuess = checkForSingleChar;
                }
                _gameRepo.StoringUserInputGame(createNewLetter);

                char userInputGuess = Convert.ToChar(createNewLetter.UserGuess.ToUpper());

                //instead of tons of for loops- contains is great!!  https://docs.microsoft.com/en-us/dotnet/api/system.string.contains?view=net-5.0
                if (createGame.CorrectWord.ToUpper().Contains(userInputGuess))
                {
                    Console.WriteLine("Great Guess!");

                    for (int i = 0; i < createGame.CorrectWord.Length; i++)
                    {
                        if (createGame.CorrectWord.ToUpper()[i] == userInputGuess)
                        {
                            //will add a number for every letter found (finally solved that)
                            lettersShownToPlayer++;
                            //will update the display to show the letter in the Correct Word
                            correctWordDisplay[i] = createGame.CorrectWord[i];
                        }
                    }
                    if (lettersShownToPlayer == createGame.CorrectWord.Length)
                    {
                        Console.Clear();
                        HangManTitle();
                        StartingToHang(createGame.LivesLeft);
                        Console.Write("The word is: ");
                        Console.WriteLine(correctWordDisplay);
                        Console.WriteLine("\nYou Win!");
                        notWon = false;
                    }
                }
                else
                {
                    Console.WriteLine($"That was not correct. The word did not contain {createNewLetter.UserGuess}.\n");
                    createGame.LivesLeft--;
                    if (createGame.LivesLeft == 0)
                    {
                        Console.Clear();
                        HangManTitle();
                        StartingToHang(createGame.LivesLeft);
                        Console.Write("The word is: ");
                        Console.WriteLine(correctWordDisplay);
                        Console.WriteLine($"\nYou lose! The word was {createGame.CorrectWord}.");
                    }
                    else
                    {
                        continue;
                    }
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
        static void HangManTitle()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("                         ************************    ");
            Console.WriteLine("                         *       HANGMAN!       *    ");
            Console.WriteLine("                         ************************    \n");
            Console.ResetColor();
        }
        static void UpperHangMan()
        {
            Console.WriteLine(" ________________ ");
            Console.WriteLine(" |/   |");
            Console.WriteLine(" |    | ");
        }
        static void LowerHangMan()
        {
            Console.WriteLine(" | ");
            Console.WriteLine(" | ");
            Console.WriteLine(" |_______ \n");
        }
        static void StartingToHang(int livesLeft)
        {
            if (livesLeft == 6)
            {
                UpperHangMan();
                Console.WriteLine(" | ");
                Console.WriteLine(" | ");
                Console.WriteLine(" | ");
                Console.WriteLine(" | ");
                LowerHangMan();
            }
            else if (livesLeft == 5)
            {
                UpperHangMan();
                PrintColorMessage(ConsoleColor.Green, " |   ( ) \n");
                Console.WriteLine(" | ");
                Console.WriteLine(" | ");
                Console.WriteLine(" | ");
                LowerHangMan();
            }
            else if (livesLeft == 4)
            {
                UpperHangMan();
                PrintColorMessage(ConsoleColor.Yellow, " |   ( ) \n");
                PrintColorMessage(ConsoleColor.Yellow, " |   /| \n");
                Console.WriteLine(" | ");
                Console.WriteLine(" | ");
                LowerHangMan();
            }
            else if (livesLeft == 3)
            {
                UpperHangMan();
                PrintColorMessage(ConsoleColor.Yellow, " |   ( ) \n");
                PrintColorMessage(ConsoleColor.Yellow, " |   /|\\ \n");
                Console.WriteLine(" | ");
                Console.WriteLine(" | ");
                LowerHangMan();
            }
            else if (livesLeft == 2)
            {
                UpperHangMan();
                PrintColorMessage(ConsoleColor.DarkYellow, " |   ( ) \n");
                PrintColorMessage(ConsoleColor.DarkYellow, " |   /|\\ \n");
                PrintColorMessage(ConsoleColor.DarkYellow, " |    | \n");
                Console.WriteLine(" | ");
                LowerHangMan();
            }
            else if (livesLeft == 1)
            {
                UpperHangMan();
                PrintColorMessage(ConsoleColor.Red, " |   ( ) \n");
                PrintColorMessage(ConsoleColor.Red, " |   /|\\ \n");
                PrintColorMessage(ConsoleColor.Red, " |    | \n");
                PrintColorMessage(ConsoleColor.Red, " |   /| \n");
                LowerHangMan();
            }
            else
            {
                UpperHangMan();
                PrintColorMessage(ConsoleColor.DarkRed, " |   ( ) \n");
                PrintColorMessage(ConsoleColor.DarkRed, " |   /|\\ \n");
                PrintColorMessage(ConsoleColor.DarkRed, " |    | \n");
                PrintColorMessage(ConsoleColor.DarkRed, " |   /|\\ \n");
                LowerHangMan();
            }
        }
    }
}
