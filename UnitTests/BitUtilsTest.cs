using DES;

namespace UnitTests
{
    [TestClass]
    public class BitUtilsTest
    {
        [TestMethod]
        public void GetBit()
        {
            byte maxBit = 31;
            for (byte i = 0; i <= maxBit; ++i)
            {
                for (byte j = 0; j <= maxBit; ++j)
                {
                    if (j == i)
                    {
                        Assert.AreEqual(true, BitUtils.GetBit((uint)1 << i, (byte)(maxBit - j)));
                    }
                    else
                    {
                        Assert.AreEqual(false, BitUtils.GetBit((uint)1 << i, (byte)(maxBit - j)));
                    }
                }
            }
        }
    }
}