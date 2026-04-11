using System.Collections;


namespace HausdorfDistanceProject
{
    internal class BitArrayExtension
    {
        public BitArrayExtension(BitArray bits , int length) 
        {
            this.bits = bits;
            this.Length = length;
        }
        public int Length { private set; get; }
        private BitArray bits;
        public bool this[int x, int y] { get => bits[x * Length + y];set => bits[x * Length + y] = value; }
    }
}
