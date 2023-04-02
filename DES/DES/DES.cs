namespace DES.DES
{
    public class DES
    {
        // ------------------------------------------------------------------------------------------------------------
        // fields
        // ------------------------------------------------------------------------------------------------------------
        private ulong MajorKey { get; init; }
        private List<ulong> MinorKeys { get; init; }

        // ------------------------------------------------------------------------------------------------------------
        // public
        // ------------------------------------------------------------------------------------------------------------
        public DES(ulong key)
        {
            MajorKey = key & Constants.keyMask;
            MinorKeys = GenerateKeys(MajorKey);
        }

        public bool Encrypt(FileStream ifs, FileStream ofs)
        {
            var buffer = new byte[Constants.blockLengthInBytes];
            int length;
            while ((length = ifs.Read(buffer, 0, Constants.blockLengthInBytes)) > 0)
            {
                var block = ByteArrayToBlock(buffer);
                var encrypt = EncryptBlock(block);
                var byteArray = BlockToByteArray(encrypt, Constants.blockLengthInBytes);
                ofs.Write(byteArray, 0, Constants.blockLengthInBytes);
                buffer = new byte[Constants.blockLengthInBytes];
            }
            return true;
        }

        public bool Decrypt(FileStream ifs, FileStream ofs)
        {
            var buffer = new byte[Constants.blockLengthInBytes];
            int length;
            while ((length = ifs.Read(buffer, 0, Constants.blockLengthInBytes)) > 0)
            {
                var block = ByteArrayToBlock(buffer);
                var decrypt = DecryptBlock(block);
                var byteArray = BlockToByteArray(decrypt, length);
                ofs.Write(byteArray, 0, length);
            }
            return true;
        }

        public ulong EncryptBlock(ulong block)
        {
            block = BitUtils.Permutation(block, Constants.initPermutation);
            var left = (block & Constants.blockLeftMask) >> Constants.blockPartLength;
            var right = block & Constants.blockRightMask;

            for (int i = 0; i < Constants.feistelRounds; ++i)
            {
                var newRight = left ^ kernelFunction(right, MinorKeys[i]);
                left = right;
                right = newRight;
            }

            ulong result = ((right << Constants.blockPartLength) & Constants.blockLeftMask) | (left & Constants.blockRightMask);
            result = BitUtils.Permutation(result, Constants.finalPermutation);
            return result;
        }

        public ulong DecryptBlock(ulong block)
        {
            block = BitUtils.Permutation(block, Constants.initPermutation);
            var left = (block & Constants.blockLeftMask) >> Constants.blockPartLength;
            var right = block & Constants.blockRightMask;

            for (int i = Constants.feistelRounds - 1; i >= 0; --i)
            {
                var newRight = left ^ kernelFunction(right, MinorKeys[i]);
                left = right;
                right = newRight;
            }

            ulong result = ((right << Constants.blockPartLength) & Constants.blockLeftMask) | (left & Constants.blockRightMask);
            result = BitUtils.Permutation(result, Constants.finalPermutation);
            return result;
        }

        // ------------------------------------------------------------------------------------------------------------
        // private
        // ------------------------------------------------------------------------------------------------------------

        private static ulong ByteArrayToBlock(byte[] bytes)
        {
            ulong result = 0;
            for (int i = bytes.Length - 1; i >= 0; --i)
            {
                result <<= Constants.byteSize;
                result += bytes[i];
            }
            return result;
        }

        private static byte[] BlockToByteArray(ulong block, int length)
        {
            var result = new byte[length];
            for (byte i = 0; i < length; ++i)
            {
                result[i] = BitUtils.GetByte(block, i);
            }
            return result;
        }
        private static List<ulong> GenerateKeys(ulong majorKey)
        {
            majorKey = BitUtils.Permutation(majorKey, Constants.keyInitPermutation);
            ulong left = majorKey & Constants.keyLeftMask;
            ulong right = majorKey & Constants.keyRightMask;

            var result = new List<ulong>();
            for (int i = 0; i < Constants.feistelRounds; ++i)
            {
                left = BitUtils.CyclicShiftLeft(left, Constants.keyInitShift[i], Constants.keyPartLength);
                right = BitUtils.CyclicShiftLeft(right, Constants.keyInitShift[i], Constants.keyPartLength);
                var minorKey = (left << Constants.keyPartLength) + right;
                minorKey = BitUtils.Permutation(minorKey, Constants.keyFinalPermutation);
                result.Add(minorKey);
            }
            return result;
        }

        private static ulong kernelFunction(ulong blockHalf, ulong minorKey)
        {
            blockHalf = BitUtils.Permutation(minorKey, Constants.expandPermutation);
            ulong sBlockInput = blockHalf ^ minorKey;
            ulong result = 0;
            for (int i = 0; i < Constants.kernelRounds; ++i)
            {
                var currentInput = sBlockInput & Constants.kernelMask;
                sBlockInput = sBlockInput >> Constants.kernelInputShift;
                var row = (int)BitUtils.OuterJoin(currentInput,
                    Constants.composeLeftBorderAmount,
                    Constants.composeRightBorderAmount,
                    Constants.composeRowSize);
                var column = (int)BitUtils.InnerJoin(currentInput,
                    Constants.composeLeftBorderAmount,
                    Constants.composeRightBorderAmount,
                    Constants.composeColSize);
                result += Constants.composeMatrix[i][row][column];
                result = result << Constants.kernelOutputShift;
            }
            result = BitUtils.Permutation(result, Constants.kernelPermutation);
            return result;
        }
    }
}
