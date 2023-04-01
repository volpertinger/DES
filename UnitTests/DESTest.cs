namespace UnitTests
{
    [TestClass]
    public class DESTest
    {
        private static int blockSize = 64;
        private static int halfBlockSize = blockSize / 2;
        [TestMethod]
        public void BlockEncryptDecrypt()
        {
            var keys = new List<ulong>();
            var testBlocks = new List<ulong>() { ulong.MinValue, ulong.MaxValue, 10, 100, 1000000000, 127, 1023, 2047,
                4095, 4097, 11235123, 32461281, 27272727819199020, 3789323232, 217748648, 9918838178,
                0b01111111_01111111_01111111_01111111_01111111_01111111_01111111_01111111,
                0b11111111_11111111_11111111_11111111_00000000_00000000_00000000_00000000,
                0b00000000_00000000_00000000_00000000_11111111_11111111_11111111_11111111};
            var random = new Random();
            for (int i = 0; i < 100; ++i)
            {
                keys.Add(((uint)random.Next(int.MaxValue) << halfBlockSize) + (uint)random.Next(int.MaxValue));
            }
            for (int i = 0; i < keys.Count; ++i)
            {
                var ss = new DES.DES.DES(keys[i]);
                for (int j = 0; j < halfBlockSize; ++j)
                {
                    // block right half check
                    var block = 1ul << j;
                    EncryptBlockTest(ss, block);

                    // block left half check
                    block = 1ul << (j + halfBlockSize);
                    EncryptBlockTest(ss, block);

                    // both blocks check
                    block = (1ul << (j + halfBlockSize)) + (1ul << j) + (ulong)(j / 2);
                    EncryptBlockTest(ss, block);
                }
                // other tests
                for (int j = 0; j < testBlocks.Count; ++j)
                {
                    var block = testBlocks[j];
                    EncryptBlockTest(ss, block);
                }
            }

        }

        // ------------------------------------------------------------------------------------------------------------
        // Utils
        // ------------------------------------------------------------------------------------------------------------
        public static void EncryptBlockTest(DES.DES.DES des, ulong block)
        {
            var encrypted = des.EncryptBlock(block);
            var plain = des.DecryptBlock(encrypted);
            Assert.AreEqual(block, plain);
            Assert.AreNotEqual(plain, encrypted);
            Assert.AreNotEqual(block, encrypted);
        }
    }
}
