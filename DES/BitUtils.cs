namespace DES
{
    public static class BitUtils
    {
        // ------------------------------------------------------------------------------------------------------------
        // variabels
        // ------------------------------------------------------------------------------------------------------------
        private static readonly byte maxBitIndex = 31;
        private static readonly byte minBitIndex = 0;
        private static readonly uint firstBitMask = (uint)1 << maxBitIndex;
        private static readonly uint lastBitMask = 1;

        // ------------------------------------------------------------------------------------------------------------
        // public
        // ------------------------------------------------------------------------------------------------------------
        public static bool GetBit(uint number, byte index)
        {
            if (IsValidBit(index))
            {
                if (index > maxBitIndex / 2)
                {
                    return ((number >> (maxBitIndex - index)) & lastBitMask) != 0;
                }
                else
                {
                    return ((number << index) & firstBitMask) != 0;
                }
            }
            else
                throw new ArgumentException(String.Format("invalid bit index {0}. Right value is: {1} to {2}.",
                    index, minBitIndex, maxBitIndex));
        }

        // ------------------------------------------------------------------------------------------------------------
        // private
        // ------------------------------------------------------------------------------------------------------------
        private static bool IsValidBit(byte bit)
        {
            return bit >= minBitIndex && bit <= maxBitIndex;
        }
    }
}
