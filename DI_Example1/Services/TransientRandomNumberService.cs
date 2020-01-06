using DI_Example1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Example1.Services
{
    public class TransientRandomNumberService : ITransientRandomNumberService
    {
        private int _randomNumber;
        public TransientRandomNumberService()
        {
            Random rnd = new Random();
            _randomNumber = rnd.Next();
        }

        public int GetNumber()
        {
            return _randomNumber;
        }
    }
}
