using System;
using System.Collections.Generic;
using System.Linq;

namespace GoCardless
{
    internal class Joiner
    {
        private readonly string _joinString;

        private Joiner(string joinString)
        {
            _joinString = joinString;
        }

        public static Joiner @on(string joinString)
        {
            
            return new Joiner(joinString);
        }

        public string @join<T>(IEnumerable<T> items)
        {
            return String.Join(_joinString, items.Select(e => e.ToString()));
        }
    }
}