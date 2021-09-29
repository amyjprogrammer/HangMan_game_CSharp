using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangMan_game
{
    public class GameRepo
    {
        //Field
        protected readonly List<Game> _gameRepo = new List<Game>();

        //Create
        public bool StoringUserInputGame(Game content)
        {
            int amountStored = _gameRepo.Count;

            _gameRepo.Add(content);

            bool infoAdded = (_gameRepo.Count > amountStored) ? true : false;
            return infoAdded;
        }
        //Read
        public List<Game> GetDatabaseGameInfo()
        {
            return _gameRepo;
        }
        //getting by User guess
        public Game GetUserLetter(string letterUserGuessed)
        {
            foreach (Game letter in _gameRepo)
            {
                if(letter.UserGuess == letterUserGuessed)
                {
                    return letter;
                }
            }
            return null;
        }
        //Delete
        public bool DeleteStoredGameInfo(Game singleUserGameInfo)
        {
            bool result = _gameRepo.Remove(singleUserGameInfo);
            return result;
        }
    }
}
