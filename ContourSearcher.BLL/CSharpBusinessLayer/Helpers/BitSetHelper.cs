using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBusinessLayer.Helpers
{
    public static class BitSetHelper
    {
        public static bool GetValue(int bitSet, int placeNumber)
        {
            int v = (1 << placeNumber) & bitSet;
            return v != 0;
        }

        public static void SetValue(ref int bitSet, int placeNumber, bool value)
        {
            if(value)
                bitSet |= (1 << placeNumber);//Set bit
            else 
                bitSet &= ~(1 << placeNumber);//Clear Bit
        }
    }
}
