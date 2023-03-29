namespace DES
{
    public static class BitUtils
    {
        // ------------------------------------------------------------------------------------------------------------
        // variabels
        // ------------------------------------------------------------------------------------------------------------
        private const byte maxBitAmount = 32;
        private const byte minBitAmount = 0;
        private const byte maxBitIndex = 31;
        private const byte minBitIndex = 0;
        private const uint firstBitMask = 1;
        private const uint lastBitMask = (uint)1 << maxBitIndex;

        // ------------------------------------------------------------------------------------------------------------
        // public
        // ------------------------------------------------------------------------------------------------------------
        public static bool GetBit(uint number, byte index)
        {
            BitValidation(index);
            if (index < maxBitIndex / 2)
            {
                return ((number >> index) & firstBitMask) != 0;
            }
            else
            {
                return ((number << (maxBitIndex - index) & lastBitMask)) != 0;
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

        /// <summary>
        /// join first firstAmount bits with last lastAmount bits from number with bit amount = length
        /// </summary>
        public static uint OuterJoin(uint number, byte firstAmount, byte lastAmount, byte length = maxBitAmount)
        {
            CustomLengthValidation(length, firstAmount + lastAmount);
            uint result = 0;
            for (byte i = 0; i < firstAmount; ++i)
            {
                result = SetBit(result, i, GetBit(number, i));
            }
            for (byte i = firstAmount, j = (byte)(length - lastAmount); i < firstAmount + lastAmount; ++i, ++j)
            {
                result = SetBit(result, i, GetBit(number, j));
            }
            return result;
        }

        /// <summary>
        /// Get bits between i and j in number with bit amount = length
        /// </summary>
        public static uint InnerJoin(uint number, byte firstAmount, byte lastAmount, byte length = maxBitAmount)
        {
            CustomLengthValidation(length, firstAmount + lastAmount);
            uint result = 0;
            for (byte i = firstAmount; i < length - lastAmount; ++i)
                result = SetBit(result, (byte)(i - firstAmount), GetBit(number, i));
            return result;
        }

        // ------------------------------------------------------------------------------------------------------------
        // private
        // ------------------------------------------------------------------------------------------------------------
        private static bool IsValidBit(byte bit)
        {
            return bit >= minBitIndex && bit <= maxBitIndex;
        }

        private static bool IsValidBitAmount(int amount)
        {
            return amount >= minBitAmount && amount <= maxBitAmount;
        }

        private static void BitValidation(byte bit)
        {
            if (!IsValidBit(bit))
                throw new ArgumentException(String.Format("invalid bit index {0}. Right value is: {1} to {2}.",
                    bit, minBitIndex, maxBitIndex));
        }

        private static void BitAmountValidation(int amount)
        {
            if (!IsValidBitAmount(amount))
                throw new ArgumentException(String.Format("invalid bit amount {0}. Right value is: {1} to {2}.",
                    amount, minBitAmount, maxBitAmount));
        }

        private static void CustomLengthValidation(int maxLength, int currentLength)
        {
            if (currentLength > maxLength || maxLength <= 1 || maxLength > maxBitAmount)
                throw new ArgumentException(String.Format("invalid maxLength {0} or currentLength {1}." +
                    "Length must be from 1 to {3}. And currentLength must be < maxLength",
                    maxLength, currentLength, maxBitAmount));
        }

        private static uint GetPureBitValue(byte bit)
        {
            return firstBitMask << bit;
        }
    }
}
