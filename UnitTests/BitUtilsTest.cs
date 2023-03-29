using DES;

namespace UnitTests
{
    [TestClass]
    public class BitUtilsTest
    {
        private readonly byte maxBit = 31;
        [TestMethod]
        public void GetBit()
        {
            for (byte i = 0; i <= maxBit; ++i)
            {
                for (byte j = 0; j <= maxBit; ++j)
                {
                    if (j == i)
                    {
                        Assert.AreEqual(true, BitUtils.GetBit(1u << i, (byte)(maxBit - j)));
                    }
                    else
                    {
                        Assert.AreEqual(false, BitUtils.GetBit(1u << i, (byte)(maxBit - j)));
                    }
                }
            }
        }
        [TestMethod]
        public void SetBit()
        {
            // 0 -> 1
            for (byte i = 0; i < maxBit; ++i)
            {
                Assert.AreEqual(1u << i, BitUtils.SetBit(0, (byte)(maxBit - i)));
            }
            // 1 -> 1
            for (byte i = 0; i < maxBit; ++i)
            {
                var number = 1u << i;
                Assert.AreEqual(number, BitUtils.SetBit(number, (byte)(maxBit - i)));
            }
        }

        [TestMethod]
        public void ClearBit()
        {
            // 1 -> 0
            Assert.AreEqual(1u, BitUtils.ClearBit(3, 30));
            Assert.AreEqual(0u, BitUtils.ClearBit(1, 31));
            Assert.AreEqual(16u, BitUtils.ClearBit(24, 28));
            Assert.AreEqual(0u, BitUtils.ClearBit(1u << 31, 0));
            // 0 -> 0
            Assert.AreEqual(3u, BitUtils.ClearBit(3, 0));
            Assert.AreEqual(1u, BitUtils.ClearBit(1, 1));
            Assert.AreEqual(24u, BitUtils.ClearBit(24, 2));
            Assert.AreEqual(1u << 31, BitUtils.ClearBit(1u << 31, 2));
        }

        [TestMethod]
        public void SetFirst()
        {
            var number = (1u << 31) + (1u << 30) + (1u << 29);
            Assert.AreEqual(number, BitUtils.SetFirstBits(0, 3));
            Assert.AreEqual(number, BitUtils.SetFirstBits(number, 3));
        }

        [TestMethod]
        public void ClearFirst()
        {
            var number = (1u << 31) + (1u << 30) + (1u << 29);
            Assert.AreEqual(0u, BitUtils.ClearFirstBits(number, 3));
            Assert.AreEqual(0u, BitUtils.ClearFirstBits(number, 18));
            Assert.AreEqual(1u, BitUtils.ClearFirstBits(number + 1, 18));
        }
    }
}