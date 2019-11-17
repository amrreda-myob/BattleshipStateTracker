using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class BattleIsNotExistException : Exception
    {
        public BattleIsNotExistException(string message) 
            : base(message)
        {
            
        }
    }
}