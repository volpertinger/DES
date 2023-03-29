namespace DES
{
    public static class BitUtils
    {
        // ------------------------------------------------------------------------------------------------------------
        // variabels
        // ------------------------------------------------------------------------------------------------------------
        private static readonly byte maxBitAmount = 32;
        private static readonly byte minBitAmount = 0;
        private static readonly byte maxBitIndex = 31;
        private static readonly byte minBitIndex = 0;
        private static readonly uint firstBitMask = (uint)1 << maxBitIndex;
        private static readonly uint lastBitMask = 1;

        // ------------------------------------------------------------------------------------------------------------
        // public
        // ------------------------------------------------------------------------------------------------------------
        public static bool GetBit(uint number, byte index)
        {
            BitValidation(index);
            if (index > maxBitIndex / 2)
            {
                return ((number >> (maxBitIndex - index)) & lastBitMask) != 0;
            }
            else
            {
                return ((number << index) & firstBitMask) != 0;
            }

        }

        public static uint SetBit(uint number, byte index)
        {
            BitValidation(index);
            if (!GetBit(number, index))
                number += GetPureBitValue(index);
            return number;
        }

        public static uint ClearBit(uint number, byte index)
        {
            BitValidation(index);
            if (GetBit(number, index))
                number -= GetPureBitValue(index);
            return number;
        }

        public static uint SetBit(uint number, byte index, bool bit)
        {
            if (bit)
                return SetBit(number, index);
            return ClearBit(number, index);
        }

        public static uint ClearFirstBits(uint number, byte bitsNumber)
        {
            BitAmountValidation(bitsNumber);
            for (byte i = 0; i < bitsNumber; ++i)
                number = ClearBit(number, i);
            return number;
        }

        public static uint SetFirstBits(uint number, byte bitsNumber)
        {
            BitAmountValidation(bitsNumber);
            for (byte i = 0; i < bitsNumber; ++i)
                number = SetBit(number, i);
            return number;
        }

        public static uint BitsSwitch(uint number, byte i, byte j)
        {
            var tmp = GetBit(number, i);
            number = SetBit(number, i, GetBit(number, j));
            number = SetBit(number, j, tmp);
            return number;
        }

        // ------------------------------------------------------------------------------------------------------------
        // private
        // ------------------------------------------------------------------------------------------------------------
        private static bool IsValidBit(byte bit)
        {
            return bit >= minBitIndex && bit <= maxBitIndex;
        }

        private static bool IsValidBitAmount(byte amount)
        {
            return amount >= minBitAmount && amount <= maxBitAmount;
        }

        private static void BitValidation(byte bit)
        {
            if (!IsValidBit(bit))
                throw new ArgumentException(String.Format("invalid bit index {0}. Right value is: {1} to {2}.",
                    bit, minBitIndex, maxBitIndex));
        }

        private static void BitAmountValidation(byte amount)
        {
            if (!IsValidBitAmount(amount))
                throw new ArgumentException(String.Format("invalid bit amount {0}. Right value is: {1} to {2}.",
                    amount, minBitAmount, maxBitAmount));
        }

        private static uint GetPureBitValue(byte bit)
        {
            return lastBitMask << (maxBitIndex - bit);
        }
    }
}
