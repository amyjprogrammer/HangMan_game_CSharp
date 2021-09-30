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

            bool continueGame = true;

            while (continueGame)
            {
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

                //code below was an attempt to add a space in correctWordDisplay
                /* int wordLengthTwice = createGame.CorrectWord.Length * 2;
                 StringBuilder correctWordDisplay = new StringBuilder(wordLengthTwice);*/
                int correctWordLength = createGame.CorrectWord.Length;

                StringBuilder correctWordDisplay = new StringBuilder(correctWordLength);
                for (int i = 0; i < correctWordLength; i++)
                {
                    correctWordDisplay.Append('_');
                }

                int lettersShownToPlayer = 0;


                while (createGame.LivesLeft > 0 && notWon)
                {
                    List<Game> listAllLetters = _gameRepo.GetDatabaseGameInfo();
                    Console.Clear();
                    HangManTitle();
                    StartingToHang(createGame.LivesLeft);
                    Console.Write("The word is: ");
                    Console.WriteLine(correctWordDisplay);
                    Console.Write("\nGuessed Letters: ");

                    //newing up 
                    Game createNewLetter = new Game();

                    //showing player letters already guessed
                    foreach (var letter in listAllLetters)
                    {
                        Console.Write(letter.UserGuess.ToUpper());//changed to upper, because word shows upper
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
                            if (_gameRepo.GetUserLetter(checkForSingleChar) != null)
                            {
                                PrintColorMessage(ConsoleColor.Red, "\nYou already guessed this letter.\n");
                                PrintColorMessage(ConsoleColor.Green, "Enter your guess again: ");
                            }
                            else
                            {
                                checkUserAnswer = false;
                            }
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
                    string correctWordUpper = createGame.CorrectWord.ToUpper();

                    //instead of tons of for loops- contains is great!!  https://docs.microsoft.com/en-us/dotnet/api/system.string.contains?view=net-5.0
                    if (correctWordUpper.Contains(userInputGuess))
                    {
                        //this is used when player guessed the right letter
                        string rightComments = PlayerGuessedRightLetterComments();
                        Console.WriteLine($"\n{rightComments}  The word did contain {userInputGuess}.");
                        Thread.Sleep(3000);

                        for (int i = 0; i < correctWordLength; i++)
                        {
                            if (correctWordUpper[i] == userInputGuess)
                            {
                                //will add a number for every letter found (finally solved that)
                                lettersShownToPlayer++;
                                //will update the display to show the letter in the Correct Word
                                correctWordDisplay[i] = correctWordUpper[i];
                            }
                        }
                        if (lettersShownToPlayer == correctWordLength)
                        {
                            Console.Clear();
                            HangManTitle();
                            StartingToHang(createGame.LivesLeft);
                            Console.Write("The word is: ");
                            Console.WriteLine(correctWordDisplay);
                            Console.WriteLine("\nYou Win!");
                            notWon = false;
                            Thread.Sleep(2000);
                            Console.Clear();
                            PrintColorMessage(ConsoleColor.DarkGreen, "\nDid you cheat? Rematch? [Y or N] : ");
                            string answer = Console.ReadLine().ToUpper();
                            if (answer == "Y")
                            {
                                //clear info - https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.clear?view=net-5.0
                                listAllLetters.Clear();
                                _gameRepo.DeleteStoredGameInfo(createNewLetter);
                                continue;
                            }
                            else if (answer == "N")
                            {
                                continueGame = false;
                            }
                            else
                            {
                                continueGame = false;
                            }
                        }
                    }
                    //used when player gave a letter not in the word
                    else
                    {
                        string wrongComments = PlayerGuessedWrongLetterComments();
                        Console.Write($"\n{wrongComments} The word did not contain {userInputGuess}.\n");
                        Thread.Sleep(3000);
                        createGame.LivesLeft--;
                        if (createGame.LivesLeft == 0)
                        {
                            Console.Clear();
                            HangManTitle();
                            StartingToHang(createGame.LivesLeft);
                            Console.Write("The word is: ");
                            Console.WriteLine(correctWordDisplay);
                            Console.WriteLine($"\nYou lose! The word was {createGame.CorrectWord}.");
                            Thread.Sleep(2000);
                            Console.Clear();
                            PrintColorMessage(ConsoleColor.DarkGreen, "\nDo you want to redeem your honor and try again? [Y or N] : ");
                            string answer = Console.ReadLine().ToUpper();
                            if (answer == "Y")
                            {
                                listAllLetters.Clear();
                                _gameRepo.DeleteStoredGameInfo(createNewLetter);
                                continue;
                            }
                            else if (answer == "N")
                            {
                                continueGame = false;
                            }
                            else
                            {
                                continueGame = false;
                            }
                        }
                        else
                        {
                            continue;
                        }
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
            string prompt = @"



MMMMMMMM               MMMMMMMM                          tttt                              iiii
M:::::::M             M:::::::M                       ttt:::t                             i::::i
M::::::::M           M::::::::M                       t:::::t                              iiii
M:::::::::M         M:::::::::M                       t:::::t
M::::::::::M       M::::::::::M  aaaaaaaaaaaaa  ttttttt:::::ttttttt   rrrrr   rrrrrrrrr  iiiiiii xxxxxxx      xxxxxxx
M:::::::::::M     M:::::::::::M  a::::::::::::a t:::::::::::::::::t   r::::rrr:::::::::r i:::::i  x:::::x    x:::::x
M:::::::M::::M   M::::M:::::::M  aaaaaaaaa:::::at:::::::::::::::::t   r:::::::::::::::::r i::::i   x:::::x  x:::::x
M::::::M M::::M M::::M M::::::M           a::::atttttt:::::::tttttt   rr::::::rrrrr::::::ri::::i    x:::::xx:::::x
M::::::M  M::::M::::M  M::::::M    aaaaaaa:::::a      t:::::t          r:::::r     r:::::ri::::i     x::::::::::x
M::::::M   M:::::::M   M::::::M  aa::::::::::::a      t:::::t          r:::::r     rrrrrrri::::i      x::::::::x
M::::::M    M:::::M    M::::::M a::::aaaa::::::a      t:::::t          r:::::r            i::::i      x::::::::x
M::::::M     MMMMM     M::::::Ma::::a    a:::::a      t:::::t    ttttttr:::::r            i::::i     x::::::::::x
M::::::M               M::::::Ma::::a    a:::::a      t::::::tttt:::::tr:::::r           i::::::i   x:::::xx:::::x
M::::::M               M::::::Ma:::::aaaa::::::a      tt::::::::::::::tr:::::r           i::::::i  x:::::x  x:::::x
M::::::M               M::::::M a::::::::::aa:::a       tt:::::::::::ttr:::::r           i::::::i x:::::x    x:::::x
MMMMMMMM               MMMMMMMM  aaaaaaaaaa  aaaa         ttttttttttt  rrrrrrr           iiiiiiiixxxxxxx      xxxxxxx








 ";
            PrintColorMessage(ConsoleColor.DarkCyan, prompt);
            Thread.Sleep(1500);
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
                    PrintColorMessage(ConsoleColor.Red, "\nPlease enter a single Letter.\n");
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
        static string PlayerGuessedWrongLetterComments()
        {
            string[] commentList = { "Sit there in your wrongness and be wrong.", "Well that's a funny way of saying 'insert right answer here'.", "User Error.", "Try better next time.", "My sources say no.", "Very Doubtful.", "Well if thats your best try...", "Isn't it the thought that counts."};

            Random randomNum = new Random();
            string randomWordWrong = commentList[randomNum.Next(0, commentList.Length - 1)];
            return randomWordWrong;
            //wanted to refactor, but this was a last minute addition
        }
        static string PlayerGuessedRightLetterComments()
        {
            string[] commentList = { "Signs point to yes.", "Spot on!", "Did you look at my paper for the right answer?", "I guess you have to pick right eventually.", "Okay, Einstein.", "I'm afraid you're right.", "Nailed it!" };

            Random randomNum = new Random();
            string randomWordCorrect = commentList[randomNum.Next(0, commentList.Length - 1)];
            return randomWordCorrect;
            //wanted to refactor, but this was a last minute addition
        }
    }
}
