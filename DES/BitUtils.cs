namespace DES
{
    public static class BitUtils
    {
        // ------------------------------------------------------------------------------------------------------------
        // variabels
        // ------------------------------------------------------------------------------------------------------------
        private const byte maxBitAmount = 64;
        private const byte minBitAmount = 0;
        private const byte maxBitIndex = 63;
        private const byte minBitIndex = 0;
        private const byte byteSize = 8;
        private const byte maxByteIndex = maxBitIndex / byteSize;
        private const byte minByteIndex = 0;
        private const ulong firstBitMask = 1;
        private const ulong lastBitMask = (ulong)1 << maxBitIndex;
        private const ulong maxMask = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_11111111;

        // ------------------------------------------------------------------------------------------------------------
        // public
        // ------------------------------------------------------------------------------------------------------------
        public static bool GetBit(ulong number, byte index)
        {
            BitValidation(index);
            if (index < maxBitIndex / 2)
            {
                return (number >> index & firstBitMask) != 0;
            }
            else
            {
                return (number << maxBitIndex - index & lastBitMask) != 0;
            }

        }

        public static ulong SetBit(ulong number, byte index)
        {
            BitValidation(index);
            if (!GetBit(number, index))
                number += GetPureBitValue(index);
            return number;
        }

        public static ulong ClearBit(ulong number, byte index)
        {
            BitValidation(index);
            if (GetBit(number, index))
                number -= GetPureBitValue(index);
            return number;
        }

        public static ulong SetBit(ulong number, byte index, bool bit)
        {
            if (bit)
                return SetBit(number, index);
            return ClearBit(number, index);
        }

        public static ulong ClearFirstBits(ulong number, byte bitsNumber)
        {
            BitAmountValidation(bitsNumber);
            for (byte i = 0; i < bitsNumber; ++i)
                number = ClearBit(number, i);
            return number;
        }

        public static ulong SetFirstBits(ulong number, byte bitsNumber)
        {
            BitAmountValidation(bitsNumber);
            for (byte i = 0; i < bitsNumber; ++i)
                number = SetBit(number, i);
            return number;
        }

        public static ulong BitsSwitch(ulong number, byte i, byte j)
        {
            var tmp = GetBit(number, i);
            number = SetBit(number, i, GetBit(number, j));
            number = SetBit(number, j, tmp);
            return number;
        }

        /// <summary>
        /// join first firstAmount bits with last lastAmount bits from number with bit amount = length
        /// </summary>
        public static ulong OuterJoin(ulong number, byte firstAmount, byte lastAmount, byte length = maxBitAmount)
        {
            CustomLengthValidation(length, firstAmount + lastAmount);
            ulong result = 0;
            for (byte i = 0; i < firstAmount; ++i)
            {
                result = SetBit(result, i, GetBit(number, i));
            }
            for (byte i = firstAmount, j = (byte)(length - lastAmount); i < firstAmount + lastAmount; ++i, ++j)
            {
                result = SetBit(result, i, GetBit(number, j));
            }
            return ToBitSize(result, maxBitAmount);
        }

        /// <summary>
        /// Get bits between i and j in number with bit amount = length
        /// </summary>
        public static ulong InnerJoin(ulong number, byte firstAmount, byte lastAmount, byte length = maxBitAmount)
        {
            CustomLengthValidation(length, firstAmount + lastAmount);
            ulong result = 0;
            for (byte i = firstAmount; i < length - lastAmount; ++i)
                result = SetBit(result, (byte)(i - firstAmount), GetBit(number, i));
            return ToBitSize(result, maxBitAmount);
        }

        public static byte GetByte(ulong number, byte index)
        {
            ByteValidation(index);
            var firstAmount = getByteStart(index);
            var lastAmount = (byte)(maxBitIndex - getByteEnd(index));
            return (byte)InnerJoin(number, firstAmount, lastAmount);
        }

        public static ulong SetByte(ulong number, byte index, byte byte_)
        {
            ByteValidation(index);
            var start = getByteStart(index);
            var end = getByteEnd(index);
            for (byte i = start, j = 0; i <= end; ++i, ++j)
                number = SetBit(number, i, GetBit(byte_, j));
            return number;
        }

        public static ulong BytesSwitch(ulong number, byte i, byte j)
        {
            var tmp = GetByte(number, i);
            number = SetByte(number, i, GetByte(number, j));
            number = SetByte(number, j, tmp);
            return number;
        }

        /// <summary>
        /// Returns maximum number (that is a power of 2) that an input number is divisible by
        /// </summary>
        public static ulong MaxNumberPower2(ulong number)
        {
            if (number == 0)
                return maxBitIndex;
            return number & ~(number - 1);
        }

        public static ulong CyclicShiftRight(ulong number, byte shift, byte length = maxBitAmount)
        {
            BitAmountValidation(shift);
            BitAmountValidation(length);
            var rightPart = number >> shift;
            var leftPart = number << length - shift;
            return ToBitSize(rightPart | leftPart, length);

        }

        public static ulong CyclicShiftLeft(ulong number, byte shift, byte length = maxBitAmount)
        {
            BitAmountValidation(shift);
            BitAmountValidation(length);
            var leftPart = number << shift;
            var rightPart = number >> length - shift;
            return ToBitSize(rightPart | leftPart, length);

        }

        public static ulong Permutation(ulong number, IList<byte> permutation)
        {
            var length = permutation.Count();
            BitAmountValidation(length);
            PermutationValidation(permutation);
            ulong result = 0;
            for (byte i = 0; i < length; ++i)
            {
                result = SetBit(result, i, GetBit(number, permutation[i]));
            }
            return result;
        }

        // ------------------------------------------------------------------------------------------------------------
        // private
        // ------------------------------------------------------------------------------------------------------------
        private static bool IsValidBit(byte bit)
        {
            return bit >= minBitIndex && bit <= maxBitIndex;
        }

        private static bool IsValidByte(byte byte_)
        {
            return byte_ >= minByteIndex && byte_ <= maxByteIndex;
        }

        private static bool IsValidBitAmount(int amount)
        {
            return amount >= minBitAmount && amount <= maxBitAmount;
        }

        private static void BitValidation(byte bit)
        {
            if (!IsValidBit(bit))
                throw new ArgumentException(string.Format("invalid bit index {0}. Right value is: {1} to {2}.",
                    bit, minBitIndex, maxBitIndex));
        }

        private static void ByteValidation(byte byte_)
        {
            if (!IsValidByte(byte_))
                throw new ArgumentException(string.Format("invalid byte index {0}. Right value is: {1} to {2}.",
                    byte_, minByteIndex, maxByteIndex));
        }

        private static void BitAmountValidation(int amount)
        {
            if (!IsValidBitAmount(amount))
                throw new ArgumentException(string.Format("invalid bit amount {0}. Right value is: {1} to {2}.",
                    amount, minBitAmount, maxBitAmount));
        }

        private static void PermutationValidation(IList<byte> permutation)
        {
            for (byte i = 0; i < permutation.Count; ++i)
            {
                if (permutation.Contains(i))
                    continue;
                throw new ArgumentException(string.Format("invalid permutation bit {0}. Permutation must contain" +
                    "all numbers from 0 to {1} once.",
                    i, permutation.Count));
            }
        }

        private static void CustomLengthValidation(int maxLength, int currentLength)
        {
            if (currentLength > maxLength || maxLength <= 1 || maxLength > maxBitAmount)
                throw new ArgumentException(string.Format("invalid maxLength {0} or currentLength {1}." +
                    "Length must be from 1 to {3}. And currentLength must be < maxLength",
                    maxLength, currentLength, maxBitAmount));
        }

        /// <summary>
        /// bit index of byte start
        /// </summary>
        private static byte getByteStart(byte index)
        {
            return (byte)(index * byteSize);
        }

        /// <summary>
        /// bit index of byte end
        /// </summary>
        private static byte getByteEnd(byte index)
        {
            return (byte)(getByteStart(index) + byteSize - 1);
        }
        private static ulong GetPureBitValue(byte bit)
        {
            return firstBitMask << bit;
        }

        private static ulong ToBitSize(ulong number, byte bitSize)
        {
            return number & (maxMask >> (maxBitAmount - bitSize));
        }
    }
}
