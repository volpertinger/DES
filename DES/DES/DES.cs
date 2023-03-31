namespace DES.DES
{
    sealed class DES : IDES
    {
        // ------------------------------------------------------------------------------------------------------------
        // fields
        // ------------------------------------------------------------------------------------------------------------
        private ulong MajorKey { get; set; }
        private List<ulong> MinorKeys { get; set; }

        // ------------------------------------------------------------------------------------------------------------
        // public
        // ------------------------------------------------------------------------------------------------------------
        public DES(ulong key)
        {
            MajorKey = key & Constants.keyMask;
            MinorKeys = GenerateKeys(MajorKey);
        }

        public bool Encrypt(string inputFilePath, string outputFilePath)
        {
            throw new NotImplementedException();
        }

        public bool Decrypt(string inputFilePath, string outputFilePath)
        {
            throw new NotImplementedException();
        }

        // ------------------------------------------------------------------------------------------------------------
        // private
        // ------------------------------------------------------------------------------------------------------------
        private static uint GetLeftPart(ulong number)
        {
            return (uint)(number & Constants.leftMask);
        }

        private static uint GetRightPart(ulong number)
        {
            return (uint)(number & Constants.RightMask);
        }

        private static List<ulong> GenerateKeys(ulong majorKey)
        {
            majorKey = BitUtils.Permutation(majorKey, Constants.keyInitPermutation);
            ulong left = majorKey & Constants.keyLeftMask;
            ulong right = majorKey & Constants.keyRightMask;

            var result = new List<ulong>();
            // <= for init shift
            for (int i = 0; i <= Constants.feistelRounds; ++i)
            {
                left = BitUtils.CyclicShiftLeft(left, Constants.keyInitShift[i], Constants.keyPartLength);
                right = BitUtils.CyclicShiftLeft(right, Constants.keyInitShift[i], Constants.keyPartLength);
                var minorKey = (left << Constants.keyPartLength) + right;
                minorKey = BitUtils.Permutation(minorKey, Constants.keyFinalPermutation);
                result.Add(minorKey);
            }
            return result;
        }
    }
}
