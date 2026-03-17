using System.Collections;

namespace HausdorfDistanceProject
{
    internal class BitArrayExtension
    {
        public BitArrayExtension(BitArray bits , int length) 
        {
            this.bits = bits;
            this.length = length;
        }
        private int length;
        private BitArray bits;
        public bool this[int x, int y] { get => bits[x * length + y];set => bits[x * length + y] = value; }
    }
}
