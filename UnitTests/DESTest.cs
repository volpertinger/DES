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
            ulong key = 32;
            var ss = new DES.DES.DES(key);
            ulong block = 0;
            ulong encrypted = 0;
            ulong plain = 0;

            // block right half check
            for (int i = 0; i < halfBlockSize; ++i)
            {
                block = 1ul << i;
                encrypted = ss.EncryptBlock(block);
                plain = ss.DecryptBlock(encrypted);
                Assert.AreEqual(block, plain);
                //Assert.AreNotEqual(plain, encrypted);
                //Assert.AreNotEqual(block, encrypted);
            }


            // block left half check
            for (int i = halfBlockSize; i < blockSize; ++i)
            {
                block = 1ul << i;
                encrypted = ss.EncryptBlock(block);
                plain = ss.DecryptBlock(encrypted);
                Assert.AreEqual(block, plain);
                //Assert.AreNotEqual(plain, encrypted);
                //Assert.AreNotEqual(block, encrypted);
            }

            // both blocks check
            for (int i = 0; i < halfBlockSize; ++i)
            {
                block = (1ul << (i + halfBlockSize)) + (1ul << i) + (ulong)(i / 2);
                encrypted = ss.EncryptBlock(block);
                plain = ss.DecryptBlock(encrypted);
                Assert.AreEqual(block, plain);
                //Assert.AreNotEqual(plain, encrypted);
                //Assert.AreNotEqual(block, encrypted);
            }
        }
    }
}
