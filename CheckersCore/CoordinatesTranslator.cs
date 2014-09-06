using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersCore
{
    public static class CoordinatesTranslator
    {
        private static readonly Dictionary<string, int> LiteralDictionary = new Dictionary<string, int>();

        static CoordinatesTranslator()
        {
            LiteralDictionary["1"] = 0;
            LiteralDictionary["2"] = 1;
            LiteralDictionary["3"] = 2;
            LiteralDictionary["4"] = 3;
            LiteralDictionary["5"] = 4;
            LiteralDictionary["6"] = 5;
            LiteralDictionary["7"] = 6;
            LiteralDictionary["8"] = 7;
            LiteralDictionary["a"] = 0;
            LiteralDictionary["b"] = 1;
            LiteralDictionary["c"] = 2;
            LiteralDictionary["d"] = 3;
            LiteralDictionary["e"] = 4;
            LiteralDictionary["f"] = 5;
            LiteralDictionary["g"] = 6;
            LiteralDictionary["h"] = 7;
        }

        public static Point Translate(string coordinates)
        {
            var strings = coordinates;
            
            return new Point();
        }
    }
}
